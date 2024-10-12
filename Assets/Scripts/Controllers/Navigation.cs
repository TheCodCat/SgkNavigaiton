using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UIElements;

/// <summary>
/// Класс с логикой навигации и прокладывания пути
/// </summary>
public class Navigation : MonoBehaviour
{
    public static Navigation Instance;
    public static UnityAction<Vector3,Vector3> OnGeneration;
    [SerializeField] private SplineContainer _lineRenderer;
    [SerializeField] private NavMeshPath _path;
    [SerializeField] private Transform[] _punkt;
    [SerializeField] private List<Vector3> _punktPosition;
    [SerializeField] private float _wayPointUP;
    [SerializeField] private float _punktUP;
    [SerializeField] private Vector3[] _wayPoints;

    [SerializeField] private CinemachineVirtualCamera _camera;

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
        _path = new NavMeshPath();
        OnGeneration += Destination;
    }

    public void Destination(Vector3 startP,Vector3 endP)
    {
        ClearNavigation();
        _punktPosition.Clear();
        _punktPosition.Add(startP);
        _punktPosition.Add(endP);

        NavMesh.CalculatePath(startP, endP, NavMesh.AllAreas, _path);

        if (_path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log("<color=green>Путь расчитан</color>");
            _wayPoints = _path.corners;
            for (int _way = 0; _way < _wayPoints.Length; _way++)
            {
                Vector3 _myPos = _wayPoints[_way];

                _wayPoints[_way] = new Vector3(_myPos.x, _myPos.y + _wayPointUP, _myPos.z);
                _lineRenderer[0].Add(new BezierKnot(_wayPoints[_way]));
            }

            for (int i = 0; i < _punktPosition.Count; i++)
            {
                _punkt[i].gameObject.SetActive(true);
                _punkt[i].position = _punktPosition[i] + new Vector3(0, _punktUP, 0);
            }
        }
        else Debug.Log("<color=red>Произошла ошибка</color>");
    }

/*    public void Destination(params Vector3[] vectors)
    {
        _punktPosition.Clear();
        ClearNavigation();
        NavMesh.CalculatePath(VarController.Instance.GetKorpus().KabinetList[1].PositionKabinet.position, vectors[0], NavMesh.AllAreas, _path);

        if (_path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log("<color=green>Путь расчитан</color>");
            _wayPoints = _path.corners;
            for (int _way = 0; _way < _wayPoints.Length; _way++)
            {
                Vector3 _myPos = _wayPoints[_way];

                _wayPoints[_way] = new Vector3(_myPos.x, _myPos.y + _wayPointUP, _myPos.z);
                _lineRenderer[0].Add(new BezierKnot(_wayPoints[_way]));
            }
        }
        else Debug.Log("<color=red>Произошла ошибка</color>");

        for (int j = 0; j < vectors.Length - 1; j++)
        {
            NavMesh.CalculatePath(vectors[j], vectors[j + 1], NavMesh.AllAreas, _path);

            if (_path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("<color=green>Путь расчитан</color>");
                _wayPoints = _path.corners;
                for (int _way = 0; _way < _wayPoints.Length; _way++)
                {
                    Vector3 _myPos = _wayPoints[_way];

                    _wayPoints[_way] = new Vector3(_myPos.x, _myPos.y + _wayPointUP, _myPos.z);
                    _lineRenderer[0].Add(new BezierKnot(_wayPoints[_way]));
                }
            }
            else Debug.Log("<color=red>Произошла ошибка</color>");
        }

    }*/
    public void ClearNavigation()
    {
        _lineRenderer[0].Clear();
        for (int i = 0; i < _punktPosition.Count; i++)
        {
            _punkt[i].gameObject.SetActive(false);
        }
    }
}
