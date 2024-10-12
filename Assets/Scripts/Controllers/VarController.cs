using System.Collections.Generic;
using UnityEngine;

public class VarController : MonoBehaviour
{
    public static VarController Instance;

	//[SerializeField] private List<NewKorpuset> _korpus = new List<NewKorpuset>();
	//public List<NewKorpuset> NewKorpuset => _korpus;
	[SerializeField] private List<DataKorpus> _campuset = new List<DataKorpus>();
    [SerializeField] private DataKorpus _dataKorpus;
    public List<DataKorpus> Campuset => _campuset;
    private void Awake()
    {
        Instance = this;
    }
	public DataKorpus SetKorpus(int indexkorpus)
    {
        if (_dataKorpus != null)
        {
            _dataKorpus.gameObject.SetActive(false);
            _dataKorpus = null;
        }
        _dataKorpus = _campuset[indexkorpus];
        _dataKorpus.gameObject.SetActive(true);
        return _dataKorpus;
    }
    public DataKorpus GetKorpus()
    {
        return _dataKorpus;
    }
    public bool IsNewKorpus(int indexkorpus)
    {
        return _dataKorpus.NameKorpus == _campuset[indexkorpus].NameKorpus ? false : true;
    }
}
