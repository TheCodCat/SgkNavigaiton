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


public class ScheduleController : MonoBehaviour
{
    [SerializeField] DateTime _time;
    ClientSamgkApi _api = new ClientSamgkApi();
    IResultOutGroup _currentGroup;

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
        Debug.Log(_groups[_groupsDropdown.value].Id);

        _currentGroup = await _api.Groups.GetGroupAsync(_groups[_groupsDropdown.value].Id);
        Debug.Log(_currentGroup.Id);
    }
    public async void ButtonDestination()
    {
        if(_appController.DataKorpus != null)
            await GetGroupCabsPositonAsync();
    }

    public async Task GetGroupCabsPositonAsync()
    {
        var _listPars = await _api.Schedule.GetScheduleAsync(new DateOnlyLegacy(_time.Year, _time.Month, _time.Day), _currentGroup);

        Vector3[] post = new Vector3[_listPars.Lessons.Count];

        for (int i = 0; i < _listPars.Lessons.Count; i++)
        {
            var nameKorpus = VarController.Instance.NewKorpuset[_appController.GetKorpusValue()].NameKorpus;
            for (int j = 0; j < _appController.DataKorpus.KabinetList.Count; j++)
            {
                if ($"{nameKorpus}/{_appController.GetBD().KabinetList[j].NameKabinet}" == _listPars.Lessons[i].Cabs[0].Adress)
                {
                    post[i] = _appController.GetBD().KabinetList[j].PositionKabinet.position;
                    break;
                    //Debug.Log(_appController.GetBD().KabinetList[j].NameKabinet);
                }
                else post[i] = _appController.GetBD().KabinetList[1].PositionKabinet.position;
            }
        }
        _navigation.Destination(post);
    }

}
