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
    public List<NewKorpuset> NewKorpuset => _korpus;
 

    private void Awake()
    {
        Instance = this;
    }
}
