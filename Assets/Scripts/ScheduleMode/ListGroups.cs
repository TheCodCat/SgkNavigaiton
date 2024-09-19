using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using ClientSamgkOutputResponse.Interfaces.Groups;
using ClientSamgkOutputResponse.Interfaces.Identity;


public class ListGroups : MonoBehaviour
{
    [SerializeField] DateTime _time;
    ClientSamgkApi _api = new ClientSamgkApi();
    
    [SerializeField] private List<Group> _groups;
    [SerializeField] private Dropdown _groupsDropdown;

    private async void Start()
    {
        _time = DateTime.Now;
        var groupsData = await _api.Groups.GetGroupsAsync();

        //GetAllGroups(groupsData);


        foreach (var group in groupsData)
        {
            _groups.Add(new Group(group.Id, group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }
    }

    private async Task GetAllGroups(List<IResultOutGroup> resultOut)
    {
        foreach (var group in resultOut)
        {
            _groups.Add(new Group(group.Id, group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }

        await Task.Yield();
    }

    public async void GetGroupList(int id)
    {
        Debug.Log(_groups[id].Id);
        await GetGroupListAsync(_groups[id].Id);
    }
    public async Task GetGroupListAsync(long id)
    {
        IResultOutGroup _currentGroup = await _api.Groups.GetGroupAsync((int)id);
        Debug.Log(_currentGroup);
        var _listPars = await _api.Schedule.GetScheduleAsync(new ClientSamgkOutputResponse.LegacyImplementation.DateOnlyLegacy(_time.Year, _time.Month, _time.Day), _currentGroup);
        foreach (var item in _listPars.Lessons)
        {
            Debug.Log(item.SubjectDetails.SubjectName);
        }
    }
}
