using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSamgk;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

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
        foreach (var group in groupsData)
        {
            _groups.Add(new Group(group.Id,group.Name));
            _groupsDropdown.options.Add(new Dropdown.OptionData(group.Name));
        }
    }

    public void GetGroupList(int id)
    {
        Debug.Log(id);
        //IResultOutGroup _currentGroup = await _api.Groups.GetGroupAsync(id);
        //Debug.Log(_currentGroup);
        //var _listPars = await _api.Schedule.GetScheduleAsync(new ClientSamgkOutputResponse.LegacyImplementation.DateOnlyLegacy(_time.Year,_time.Month,_time.Day), _currentGroup);
    }
}
