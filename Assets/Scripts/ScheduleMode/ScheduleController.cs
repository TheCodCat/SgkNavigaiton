using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.LegacyImplementation;
using ClientSamgkOutputResponse.Interfaces.Schedule;
using static UnityEngine.ParticleSystem;


public class ScheduleController : MonoBehaviour
{
    private DateTime _time;
    ClientSamgkApi _api = new ClientSamgkApi();
    IResultOutGroup _currentGroup;
    IResultOutScheduleFromDate _currentScheduleFromDate;
    List<int> _listNameKorpus = new List<int>();

    [SerializeField] private List<Group> _groups;
    [SerializeField] private Dropdown _groupsDropdown;
    [SerializeField] private Text _numParsText;

    [SerializeField] private AppController _appController;
    [SerializeField] private Navigation _navigation;
    [SerializeField,Range(0,6)] private int _numberPars;

    private async void Start()
    {
        _time = DateTime.Now;
        var groupsData = await _api.Groups.GetGroupsAsync();

        await GetAllGroups((List<IResultOutGroup>)groupsData);
    }

    private async Task GetAllGroups(List<IResultOutGroup> resultOut)
    {
        foreach (var group in resultOut)
        {
            Debug.Log(group);
            if (group.Course > 4) continue;
            _groups.Add(new Group((int)group.Id, group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }

        await Task.Yield();
    }

    public async void GetGroupList(int id)
    {
        if(id == 0) return;

        _currentGroup = await _api.Groups.GetGroupAsync(_groups[_groupsDropdown.value].Id);
        Debug.Log(_currentGroup.Id);
    }

    public async void GetGroupCabsPositonAsync()
    {
        _currentScheduleFromDate = await _api.Schedule.GetScheduleAsync(new DateOnlyLegacy(_time.Year,_time.Month,_time.Day),_currentGroup);

        if (_currentScheduleFromDate.Lessons.Count == 0)
        {
            Debug.Log("пар нет");
            return;
        }

        for (int i = 1; i < VarController.Instance.NewKorpuset.Count; i++)
        {
            //Debug.Log(_currentScheduleFromDate.Lessons[0].Cabs[0].Adress);
            if (_currentScheduleFromDate.Lessons[0].Cabs[0].Campus == VarController.Instance.NewKorpuset[i].NameKorpus)
            {
                VarController.Instance.SetKorpus(i);
                _appController.SetActiveEtage();
                GetParsPositionAsync(true);
                return;
            }
        }
    }

    public void GetParsPositionAsync(bool newKorpus)
    {
        Vector3[] kabs = new Vector3[2];
        _numParsText.text = $"{_numberPars + 1}";
        if (_numberPars == 0 || newKorpus)
        {
            foreach (var kabinet in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory == kabinet.NameKabinet)
                {
                    kabs[0] = VarController.Instance.GetKorpus().KabinetList[1].PositionKabinet.position;
                    kabs[1] = kabinet.PositionKabinet.position;
                }
            }
        }
        else
        {
            //первый кабинет
            Debug.Log($"Первая пара в -- {_currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory}");
			foreach (var first in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Auditory == first.NameKabinet)
                {
                    Debug.LogError(first.NameKabinet);
                    kabs[0] = first.PositionKabinet.position; 
                }
            }
            //следующий кабинет
            Debug.Log($"Вторая пара в -- {_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory}");
            foreach (var last in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Auditory == last.NameKabinet)
				{
                    Debug.LogError(last.NameKabinet);
				    kabs[1] = last.PositionKabinet.position;
                }
            }
        }
        _navigation.Destination(kabs[0], kabs[1]);
    }

    public bool ChangeNumberPars()
    {
        if (VarController.Instance.GetKorpus().NameKorpus != _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Campus)
        {
            Debug.Log("Пара в другом кабинете");
            for (int i = 1; i < VarController.Instance.NewKorpuset.Count; i++)
            {
                //Debug.Log(_currentScheduleFromDate.Lessons[0].Cabs[0].Adress);
                if (_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Campus == VarController.Instance.NewKorpuset[i].NameKorpus)
                {
                    VarController.Instance.SetKorpus(i);
                    _appController.SetActiveEtage();
                    return true;
                }
            }
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
