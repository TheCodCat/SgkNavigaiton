using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.LegacyImplementation;
using ClientSamgkOutputResponse.Interfaces.Schedule;


public class ScheduleController : MonoBehaviour
{
    [SerializeField] DateTime _time;
    ClientSamgkApi _api = new ClientSamgkApi();
    IResultOutGroup _currentGroup;
    IResultOutScheduleFromDate _currentScheduleFromDate;

    [SerializeField] private List<Group> _groups;
    [SerializeField] private Dropdown _groupsDropdown;

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
            if (_currentScheduleFromDate.Lessons[0].Cabs[0].Adress[0].ToString().ToLower() == VarController.Instance.NewKorpuset[i].NameKorpus.ToLower())
            {
                VarController.Instance.SetKorpus(i);
                _appController.SetActiveEtage();
                GetParsPositionAsync();
                return;
            }
        }
    }
    public void GetParsPositionAsync()
    {
        Vector3[] kabs = new Vector3[2];
        if(_numberPars == 0)
        {
            foreach (var kabinet in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[0].Cabs[0].Adress == $"{VarController.Instance.GetKorpus().NameKorpus}/{kabinet.NameKabinet}")
                {
                    kabs[0] = VarController.Instance.GetKorpus().KabinetList[1].PositionKabinet.position;
                    kabs[1] = kabinet.PositionKabinet.position;
                }
            }
        }
        else
        {
            //первый кабинет
            foreach (var first in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[_numberPars - 1].Cabs[0].Adress == $"{VarController.Instance.GetKorpus().NameKorpus}/{first.NameKabinet}")
                    kabs[0] = first.PositionKabinet.position;
            }
            //следующий кабинет
            foreach (var last in VarController.Instance.GetKorpus().KabinetList)
            {
                if (_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Adress == $"{VarController.Instance.GetKorpus().NameKorpus}/{last.NameKabinet}")
                    kabs[1] = last.PositionKabinet.position;
            }
        }
        _navigation.Destination(kabs[0], kabs[1]);
    }

    public void ChangeNumberPars()
    {
        if (VarController.Instance.GetKorpus().NameKorpus.ToLower() != _currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Adress[0].ToString().ToLower())
        {
            Debug.Log("Пара в другом кабинете");
            for (int i = 1; i < VarController.Instance.NewKorpuset.Count; i++)
            {
                //Debug.Log(_currentScheduleFromDate.Lessons[0].Cabs[0].Adress);
                if (_currentScheduleFromDate.Lessons[_numberPars].Cabs[0].Adress[0].ToString().ToLower() == VarController.Instance.NewKorpuset[i].NameKorpus.ToLower())
                {
                    VarController.Instance.SetKorpus(i);
                    _appController.SetActiveEtage();
                    GetParsPositionAsync();
                    return;
                }
            }
        }
    }

    public void ChangeNumberParsPlus()
    {
        _numberPars = (_numberPars + 1) % _currentScheduleFromDate.Lessons.Count;
        GetParsPositionAsync();
        ChangeNumberPars();
    }
    public void ChangeNumberParsMinus()
    {
        _numberPars = (_numberPars - 1) % _currentScheduleFromDate.Lessons.Count;
        GetParsPositionAsync();
        ChangeNumberPars();
    }
}
