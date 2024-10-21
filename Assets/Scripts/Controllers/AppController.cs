using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс основной логики приложения
/// </summary>
public class AppController : MonoBehaviour
{
    public static AppController Instance;
    [SerializeField] private CameraMotor _cameraMotor;
    [SerializeField] private VarController _varController;
    [Header("Выподающие списки")]
    [SerializeField] private Dropdown _korpus;
    [SerializeField] private List<Dropdown> _kabinets;
    //[SerializeField] private List<Dropdown> _dropdowns;
    [Header("Родитель")]
    [SerializeField] private Transform _korpusParent;
    [SerializeField] private int _indexEtage;
    [SerializeField] private Vector3[] _posToEtage;
    [SerializeField] private List<Toggle> _toggles;
    public Vector3[] PosToEtage => _posToEtage;

    public ToggleGroup toggleGroup;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    private async void Start()
    {
        await SetKorpus();
    }
    public async Task SetKorpus()
    {
        foreach (var korpus in _varController.Campuset)
        {
            if(korpus is null) _korpus.options.Add(new Dropdown.OptionData(string.Empty));
            else _korpus.options.Add(new Dropdown.OptionData(korpus.NameKorpus));
        }
        await Task.Yield();
    }
    public async void KorpusSpawn(int indexkorpus)
    {
        if (_varController.Campuset[indexkorpus] != null)
        {
            DataKorpus dataKorpus = _varController.SetKorpus(indexkorpus);
            dataKorpus.transform.SetParent(_korpusParent);
            UIController.Instance.KabinetPanel(_varController.GetKorpus());
        }
        await Task.Yield ();
    }
    public async void SetKabinetToKorpus()
    {
        foreach (var item in _kabinets)
        {
            item.ClearOptions();
        }
        if (_varController.GetKorpus() == null) return;
        foreach (var kabinets in _varController.GetKorpus().KabinetList)
        {
            foreach (var kabinet in _kabinets)
            {
                kabinet.options.Add(new Dropdown.OptionData(kabinets.NameKabinet));
            }
        }
        await Task.Yield();
    }

    public void EtageSpawnToggle(bool IsEtage)
    {
        int _etage = int.Parse(toggleGroup.ActiveToggles().FirstOrDefault().name);
        _indexEtage = _etage;
        _cameraMotor.MovomentToEtage(_posToEtage[_etage]);
    }

    public void EtageNavToggle(int IsEtage)
    {
        _toggles[IsEtage].isOn = true;
    }

    public void SetActiveEtage()
    {
        //выключение активных этажей
        foreach (var item in _toggles)
        {
            item.interactable = false;
        }
        //включение активных этажей
        if (_varController.GetKorpus() == null) return;
        for (int i = 0; i < _varController.GetKorpus().EtageList.Count; i++)
        {
            _toggles[i].interactable = true;
        }
    }

    public void SetPosition()
    {
        foreach (var item in _kabinets)
        {
            if (item.value == 0)
            {
                return;
            }
        }
        Navigation.OnGeneration.Invoke(_varController.GetKorpus().KabinetList[_kabinets[0].value].PositionKabinet.position, _varController.GetKorpus().KabinetList[_kabinets[1].value].PositionKabinet.position);
    }

    public int GetKorpusValue()
    {
        return _korpus.value;
    }

    public string GetKorpusName()
    {
        return VarController.Instance.Campuset[_korpus.value].NameKorpus;
    }
}