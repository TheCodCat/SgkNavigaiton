using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.Interfaces.Identity;
using static UnityEditor.Progress;
using ClientSamgkOutputResponse.LegacyImplementation;
using ClientSamgkOutputResponse.Interfaces.Schedule;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


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

        foreach (var item in _currentScheduleFromDate.Lessons)
        {
            Debug.Log(item.Cabs[0].Adress);
            
        }
        for (int i = 0; i < VarController.Instance.NewKorpuset.Count; i++)
        {

            if (_currentScheduleFromDate.Lessons[0].Cabs[0].Adress[0].ToString().ToLower() == VarController.Instance.NewKorpuset[i].NameKorpus.ToLower())
            {
                VarController.Instance.SetKorpus(i);
                _appController.SetActiveEtage();
                return;
            }
        }
    }

}
