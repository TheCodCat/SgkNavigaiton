using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.Interfaces.Schedule;
using System.Linq;
using UnityEngine.Events;

public class ScheduleController : MonoBehaviour
{
    public static event UnityAction OnGetGroups;
    ClientSamgkApi _api = new ClientSamgkApi();
    IList<IResultOutGroup> _allGroups;
    IList<IResultOutGroup> Allgroups 
    { 
        get 
        { 
            return _allGroups;
        }
        set
        {
            _allGroups = value;
            OnGetGroups?.Invoke();
        } 
    }
    IResultOutGroup _currentGroup;
    IResultOutScheduleFromDate _currentScheduleFromDate;

    private IList<Group> _allListGroups = new List<Group>() { new Group(0, "Группа не выбрана") };
    [SerializeField] private List<Group> _dropListGroups;
    [SerializeField] private Dropdown _groupsDropdown;
    [SerializeField] private Text _numParsText;

    [Header("Зависимости")]
    [SerializeField] private Notification _notification;
    [SerializeField] private AppController _appController;
    [SerializeField] private Navigation _navigation;
    [SerializeField,Range(0,6)] private int _numberPars;

    private async void Start()
    {
        Allgroups = await _api.Groups.GetGroupsAsync();
		await GetAllGroups(Allgroups);
    }

    private async Task GetAllGroups(IList<IResultOutGroup> resultOut)
    {
        foreach (var group in resultOut)
        {
            if (group.Course > 4) continue;

            _allListGroups.Add(new Group((int)group.Id, group.Name));
            _dropListGroups.Add(new Group((int)group.Id, group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }

        await Task.Yield();
    }

    public async void GetGroupList(int id)
    {
        if(id == 0) return;

        _currentGroup = await _api.Groups.GetGroupAsync(_dropListGroups[id].Name);
        Debug.Log(_currentGroup.Id);
    }

    public async void GetGroupCabsPositonAsync()
    {
        _currentScheduleFromDate = await _api.Schedule.GetScheduleAsync(DateTime.Now,_currentGroup);

        if (_currentScheduleFromDate.Lessons.Count is 0)
        {
            Debug.Log("пар нет");
            _notification.SendNotification("Сегодня пар нет.");
            return;
        }

        for (int i = 1; i < VarController.Instance.Campuset.Count; i++)
        {
            //Debug.Log(_currentScheduleFromDate.Lessons[0].Cabs[0].Adress);
            if (_currentScheduleFromDate.Lessons[0].Cabs[0].Campus == VarController.Instance.Campuset[i].NameKorpus)
            {
                VarController.Instance.SetKorpus(i);
                _appController.SetActiveEtage();
                GetParsPositionAsync();
                return;
            }
        }
    }

    public void GetParsPositionAsync(bool newKorpus = true)
    {
        Vector3[] kabs = new Vector3[2];
        _numParsText.text = $"{_numberPars + 1}";
        if (_numberPars == 0 || newKorpus)
        {
            var cab = VarController.Instance.GetKorpus().KabinetList.Where(x => x.NameKabinet == _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory).First();

            kabs[0] = VarController.Instance.GetKorpus().KabinetList[1].PositionKabinet.position;
            kabs[1] = cab.PositionKabinet.position;
        }
        else
        {
            //первый кабинет
            Debug.Log($"Первая пара в -- {_currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory}");

            var first = VarController.Instance.GetKorpus().KabinetList.Where(x => x.NameKabinet == _currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory).First();

            Debug.LogError(first.NameKabinet);
            kabs[0] = first.PositionKabinet.position; 

            //следующий кабинет
            Debug.Log($"Вторая пара в -- {_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory}");

            var last = VarController.Instance.GetKorpus().KabinetList.Where(x => x.NameKabinet == _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory).First();

            Debug.LogError(last.NameKabinet);
			kabs[1] = last.PositionKabinet.position;
        }
        _navigation.Destination(kabs[0], kabs[1]);
    }

    public bool ChangeNumberPars()
    {
        if (VarController.Instance.GetKorpus().NameKorpus != _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Campus)
        {
            //_notification.SendNotification("Пара в другом кабинете");
            var campus = VarController.Instance.Campuset.FindIndex(c => c?.NameKorpus == _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Campus);

            VarController.Instance.SetKorpus(campus);
            _appController.SetActiveEtage();
            return true;
        }
        return false;
    }

    public void ChangeNumberParsPlus()
    {
        if (VarController.Instance.GetKorpus() == null || _groupsDropdown.value == 0) return;
        _numberPars = (_numberPars + 1) % _currentScheduleFromDate.Lessons.Count;
        GetParsPositionAsync(ChangeNumberPars());
    }

    public void ChangeNumberParsMinus()
    {
        if (VarController.Instance.GetKorpus() == null || _groupsDropdown.value == 0) return;
        _numberPars = (_numberPars - 1) % _currentScheduleFromDate.Lessons.Count;
        if(_numberPars < 0) _numberPars = 0;
        GetParsPositionAsync(ChangeNumberPars());
    }
    
    public async void GetNameGroup(string needGroup)
    {
        if (_dropListGroups.Count < 1) return;

        _groupsDropdown.ClearOptions();
        _dropListGroups.Clear();

        if(!string.IsNullOrEmpty(needGroup))
        {
            _groupsDropdown.options.Add(new Dropdown.OptionData(_allListGroups[0].Name));
            _dropListGroups.Add(new Group(_allListGroups[0].Id, _allListGroups[0].Name));

            var obj = _allListGroups.Where(x => x.Name.Contains(needGroup, StringComparison.InvariantCultureIgnoreCase));
            foreach (var item in obj)
            {
                _dropListGroups.Add(item);
                _groupsDropdown.options.Add(new Dropdown.OptionData(item.Name));
            }
        }
        else
        {
			for (int i = 0; i < _allListGroups.Count; i++)
			{
                _dropListGroups.Add(_allListGroups[i]);
                _groupsDropdown.options.Add(new Dropdown.OptionData(_allListGroups[i].Name));
            }
		}

        _groupsDropdown.RefreshShownValue();

        await Task.Yield();
	}

    public async void GetGroupDropdown(string needgroup)
    {
        //Debug.Log(needgroup);

        if(needgroup.Count() > 0)
        {
            var myGroup = _dropListGroups.Where(g => g.Name.Contains(needgroup, StringComparison.CurrentCultureIgnoreCase));
            if(myGroup.Count() <= 1)
            {
                var indexGroup = _dropListGroups.IndexOf(myGroup.FirstOrDefault());

                _groupsDropdown.SetValueWithoutNotify(indexGroup);
                _groupsDropdown.onValueChanged?.Invoke(indexGroup);
            }

            //Debug.Log(indexGroup);
        }

        await Task.Yield();
    }
}
