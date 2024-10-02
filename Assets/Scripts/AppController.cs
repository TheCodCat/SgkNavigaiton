using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text a;
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
    private void Start()
    {
        SetKorpus();
    }
    public void SetKorpus()
    {
        foreach (var korpus in VarController.Instance.NewKorpuset)
        {
            _korpus.options.Add(new Dropdown.OptionData(korpus.NameKorpus));
        }
    }
    public void KorpusSpawn(int indexkorpus)
    {
        if (_korpusParent.childCount != 0)
        {
            for (int i = _korpusParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_korpusParent.GetChild(i).gameObject);
                a.gameObject.SetActive(true);
            }
        }
        if (VarController.Instance.NewKorpuset[indexkorpus].KorpusPrefab != null)
        {
            _varController.SetKorpus(indexkorpus);
            VarController.Instance.GetKorpus().transform.parent = _korpusParent.transform;
            UIController.Instance.KabinetPanel(VarController.Instance.GetKorpus());
            a.gameObject.SetActive(false);
        }
    }
    public void SetKabinetToKorpus()
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
        return VarController.Instance.NewKorpuset[_korpus.value].NameKorpus;
    }
}