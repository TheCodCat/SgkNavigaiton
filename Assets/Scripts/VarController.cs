using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewKorpuset
{
    public string NameKorpus;
    public GameObject KorpusPrefab;
}
public class VarController : MonoBehaviour
{
    public static VarController Instance;

    [SerializeField] private List<NewKorpuset> _korpus = new List<NewKorpuset>();
    [SerializeField] private DataKorpus _dataKorpus;
    public List<NewKorpuset> NewKorpuset => _korpus;

    public DataKorpus SetKorpus(int indexkorpus)
    {
        _dataKorpus = Instantiate(NewKorpuset[indexkorpus].KorpusPrefab).GetComponent<DataKorpus>();
        return _dataKorpus;
    }
    public DataKorpus GetKorpus()
    {
        return _dataKorpus;
    }
    private void Awake()
    {
        Instance = this;
    }
}
