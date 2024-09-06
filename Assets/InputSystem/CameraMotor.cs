using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class CameraMotor : MonoBehaviour
{
    public static CameraMotor Instance;
    [Header("общие переменные")]
    [SerializeField] private Vector2 _oldPos, _pos,_pos2;
    [SerializeField] private Vector3 _dir, _pos2t;
    [SerializeField] private CinemachineVirtualCamera _cameraCimenachin;
    [SerializeField] private CinemachineConfiner _cinemachineConfiner;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;
    [Header("телефон переменные")]
    [SerializeField] private float _oldZoomingTouch;
    [SerializeField] private float _zoomingTouch;
    [SerializeField] private float _koeficientZoom;
    public CinemachineVirtualCamera CameraCimenachin => _cameraCimenachin;

    public float _rad, _oldrad,_rotate;
    public Vector2 _oldRadial, _radial;

    [SerializeField] private bool _isUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TypePlatform();
    }
    private void Update()
    {
        _isUI = EventSystem.current.IsPointerOverGameObject();
    }

    public void SetConfiner(Collider coll)
    {
        _cinemachineConfiner.m_BoundingVolume = coll;
    }

    private void TypePlatform()
    {
            InputManager.OnPositionTouch += MoveTouch;
            InputManager.OnTouch2 += ZoomTouch;
            InputManager.OnTouch2 += RotateTouch;
    }

    public void MovomentToEtage(Vector3 newPos)
    {
        var position = _cameraCimenachin.transform.position;
        _cameraCimenachin.transform.position = new Vector3(position.x,newPos.y,position.z);
    }
    public void MovomentToPos(Vector3 newPos)
    {
        _cameraCimenachin.transform.position = newPos;
    }
    private void MoveTouch(InputAction.CallbackContext ctx)
    {
        if (AppController.Instance.DataKorpus == null) return;

        switch (ctx.ReadValue<TouchState>().phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:

                _pos = ctx.ReadValue<TouchState>().position;
                _oldPos = _pos;

                break;

            case UnityEngine.InputSystem.TouchPhase.Moved:
                if (_isUI) return;
                _pos = ctx.ReadValue<TouchState>().position;
                if (_cameraCimenachin.transform.position == _cam.transform.position)
                {
                    _dir = _cam.ScreenToWorldPoint(_oldPos) - (_cam.ScreenToWorldPoint(_pos));
                    _cameraCimenachin.transform.Translate(_dir,Space.World);
                }
                else _cameraCimenachin.transform.position = _cam.transform.position;                
                _oldPos = _pos;

                break;
        }
    }

    private void ZoomTouch(InputAction.CallbackContext ctx)
    {
        if (AppController.Instance.DataKorpus == null) return;
        switch (ctx.ReadValue<TouchState>().phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:

                _oldZoomingTouch = Vector2.Distance(_pos,ctx.ReadValue<TouchState>().position);
                break;
            case UnityEngine.InputSystem.TouchPhase.Moved:

                _pos2t = ctx.ReadValue<TouchState>().position;
                _zoomingTouch = Vector2.Distance(_pos, ctx.ReadValue<TouchState>().position);
 
                _cameraCimenachin.m_Lens.OrthographicSize -= (_zoomingTouch - _oldZoomingTouch) * _koeficientZoom;

                _oldZoomingTouch = _zoomingTouch;

                _cameraCimenachin.m_Lens.OrthographicSize = Mathf.Clamp(_cameraCimenachin.m_Lens.OrthographicSize, _minSize, _maxSize);
                break;
        }
    }

    private void RotateTouch(InputAction.CallbackContext ctx)
    {
        _pos2 = ctx.ReadValue<TouchState>().position;

        if (AppController.Instance.DataKorpus == null) return;

        switch (ctx.ReadValue<TouchState>().phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:

                _oldRadial = _pos2 - _pos;
                _oldrad = Mathf.Atan2(_oldRadial.y, _oldRadial.x) * Mathf.Rad2Deg;
                break;

            case UnityEngine.InputSystem.TouchPhase.Moved:
                _radial = _pos2 - _pos;
                _rad = Mathf.Atan2(_radial.y, _radial.x) * Mathf.Rad2Deg;
                _rotate = _oldrad - _rad;
                _cameraCimenachin.transform.Rotate(0,0,_rotate);
                _oldrad = _rad;
                break;
        }
    }

   public void ResetPosition()
    {
        _cameraCimenachin.transform.position = new Vector3(0,_cameraCimenachin.transform.position.y,0);
    }
    public void ResetRotation()
    {
        _cameraCimenachin.transform.rotation = Quaternion.Euler(90,0,0);
    }
}