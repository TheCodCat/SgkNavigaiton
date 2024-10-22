using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.Interfaces.Schedule;
using System.Linq;
using UnityEngine.Events;
using Cysharp.Threading.Tasks.Linq;
using Assets.Scripts.Extern;
using FuzzySharp;

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

    private async UniTask GetAllGroups(IList<IResultOutGroup> resultOut)
    {
        foreach (var group in resultOut)
        {
            if (group.Course > 4) continue;

            _allListGroups.Add(new Group((int)group.Id, group.Name));
            _dropListGroups.Add(new Group((int)group.Id, group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }
        await UniTask.Yield();
    }

    public async void GetGroupList(int id)
    {
        Debug.Log(id);
        _currentGroup = await _api.Groups.GetGroupAsync(_dropListGroups[id >= 0 ? id : 0].Name);
        Debug.Log(_currentGroup?.Id);
    }

    public async void GetGroupCabsPositonAsync()
    {
        if (_currentGroup is null) return;

        _currentScheduleFromDate = await _api.Schedule.GetScheduleAsync(DateTime.Now,_currentGroup);

        if (_currentScheduleFromDate.IsDist())
        {
            Debug.Log("Дистант");
            _notification.SendNotification("Сегодня дистант.");
            return;
        }

        var campus = VarController.Instance.Campuset.SingleOrDefault(x => x?.NameKorpus == _currentScheduleFromDate.Lessons[0].Cabs[0].Campus);
        var indexCampus = campus == null ? 0 : VarController.Instance.Campuset.IndexOf(campus);

        VarController.Instance.SetKorpus(indexCampus);
        _appController.SetActiveEtage();
        GetParsPositionAsync();
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
                await UniTask.Yield();
            }
        }
        else
        {
			for (int i = 0; i < _allListGroups.Count; i++)
			{
                _dropListGroups.Add(_allListGroups[i]);
                _groupsDropdown.options.Add(new Dropdown.OptionData(_allListGroups[i].Name));
                await UniTask.Yield();
            }
		}

        _groupsDropdown.RefreshShownValue();

        await UniTask.Yield();
	}

    public async void GetGroupDropdown(string needgroup)
    {
        //Debug.Log(needgroup);

        if(needgroup.Count() > 0)
        {
            var myGroup = _dropListGroups.FirstOrDefault(g => g.Name.ToLower() == needgroup.ToLower());

            var indexGroup = _dropListGroups.IndexOf(myGroup);

            _groupsDropdown.SetValueWithoutNotify(indexGroup);
            _groupsDropdown.onValueChanged?.Invoke(indexGroup);
        }

        await UniTask.Yield();
    }

    public void GetParsPositionAsync(bool newKorpus = true)
    {

        var isDist = _currentScheduleFromDate.IsDist();

        if (isDist) return;

        Vector3[] kabs = new Vector3[2];
        _numParsText.text = $"{_numberPars + 1}";
        if (_numberPars == 0 || newKorpus)
        {
            var cab = VarController.Instance.GetKorpus()?.KabinetList.SingleOrDefault(x => x?.NameKabinet == _currentScheduleFromDate?.Lessons[_numberPars].Cabs[0].Auditory);

            kabs[0] = VarController.Instance.GetKorpus().KabinetList[1].PositionKabinet.position;
            kabs[1] = cab.PositionKabinet.position;
        }
        else
        {
            //первый кабинет
            Debug.Log($"Первая пара в -- {_currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory}");

            var first = VarController.Instance.GetKorpus().KabinetList.SingleOrDefault(x => x.NameKabinet == _currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory);

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
    
}
