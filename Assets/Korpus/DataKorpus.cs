using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
[RequireComponent(typeof(DataKorpus)),RequireComponent(typeof(SetBounsens)),RequireComponent(typeof(BoxCollider)),RequireComponent(typeof(NavMeshSurface))]
public class DataKorpus : MonoBehaviour
{
    [SerializeField] private List<Cabinet> _kabinetList = new List<Cabinet>();
    [SerializeField] private List<Etage> _etageList = new List<Etage>();
    [SerializeField] private string _nameKorpus;
    public List<Cabinet> KabinetList => _kabinetList;
    public List<Etage> EtageList => _etageList;
    public string NameKorpus => _nameKorpus;
}
