using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(DataKorpus)),RequireComponent(typeof(SetBounsens)),RequireComponent(typeof(BoxCollider)),RequireComponent(typeof(NavMeshSurface))]
public class DataKorpus : MonoBehaviour
{
    [SerializeField] private List<Kabinet> _kabinetList = new List<Kabinet>();
    [SerializeField] private List<Etage> _etageList = new List<Etage>();
    public List<Kabinet> KabinetList => _kabinetList;
    public List<Etage> EtageList => _etageList;
}
