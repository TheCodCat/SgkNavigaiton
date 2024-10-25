using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoParent : MonoBehaviour
{
    public static InfoParent Instance;
    [SerializeField] private Camera _camera;
    public Transform Parent {
        get
        {
            Debug.Log("asda");
            return transform;
        }
        private set { } }

    private void Awake()
    {
        Instance = this;
    }
    public Vector2 MyPositionToCanvas(Vector3 transform)
    {
        return _camera.WorldToScreenPoint(transform);
    }
}
