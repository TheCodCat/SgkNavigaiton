using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private InputSystem _inputSystem;
    public static UnityAction<InputAction.CallbackContext> OnPositionTouch;
    public static UnityAction<InputAction.CallbackContext> OnTouch2;
	public static UnityAction<InputAction.CallbackContext> OnPositionTouchDelta;

	private void Awake()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();
    }
    private void OnDisable()
    {
        _inputSystem.Disable();
    }
    private void Start()
    {
        TypePlatform();
    }
    private void TypePlatform()
    {
        _inputSystem.TouchScreen.PrimaryTouch.performed += ctx => MoveTouch(ctx);
        _inputSystem.TouchScreen.ZoomTouch.performed += ctx => ZoomTouch(ctx);
        _inputSystem.TouchScreen.PrimaryTouchDelta.performed += ctx => DeltaTouch(ctx);
    }
    private void MoveTouch(InputAction.CallbackContext context)
    {
        OnPositionTouch?.Invoke(context);
    }
    private void ZoomTouch(InputAction.CallbackContext context)
    {
        OnTouch2?.Invoke(context);
    }
	private void DeltaTouch(InputAction.CallbackContext context)
	{
		OnPositionTouchDelta?.Invoke(context);
	}

}