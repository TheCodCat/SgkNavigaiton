using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GenerationPanelMove : MonoBehaviour,IDragHandler, IEndDragHandler
{
	[SerializeField] private Vector2 _prymaryposition;
	[SerializeField] private RectTransform _myTransform;
	[SerializeField] private float _speedMovePanel;
	[Header("Контрольные позиции")]
	[SerializeField] private Vector2 _firstPostion;
	[SerializeField] private float _distanceToPositionFirst;
	[Space]
	[SerializeField] private Vector2 _secondPostion;

	private void OnEnable()
	{
		InputManager.OnPositionTouchDelta += GetPrymaryTouch;
	}
	private void OnDisable()
	{
		InputManager.OnPositionTouchDelta -= GetPrymaryTouch;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 _position = transform.position;
		_position.x += _prymaryposition.x;
		_position.x = math.clamp(_position.x, _firstPostion.x, _secondPostion.x);
		_myTransform.position = _position;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		if (Mathf.Abs(_myTransform.anchoredPosition.x) > _distanceToPositionFirst)
		{
			_myTransform.anchoredPosition = _firstPostion;
		}
		else
			_myTransform.anchoredPosition = _secondPostion;
	}

	public void GetPrymaryTouch(InputAction.CallbackContext context)
	{
		_prymaryposition = context.ReadValue<Vector2>();
	}

}
