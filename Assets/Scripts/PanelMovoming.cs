using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PanelMovoming : MonoBehaviour,IDragHandler, IEndDragHandler
{
	public enum TypeMove
	{
		Horizontal, Vertical
	}

	[SerializeField] private TypeMove _typeMove;
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
		Vector2 position = transform.position;
		switch (_typeMove)
		{
			case TypeMove.Horizontal:
				position.x += _prymaryposition.x;
				position.x = math.clamp(position.x, _firstPostion.x, _secondPostion.x);
				break;

			case TypeMove.Vertical:
				position.y += _prymaryposition.y;
                position.y = math.clamp(position.y, _firstPostion.y, _secondPostion.y);
                break;
		}
		_myTransform.position = position;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		switch (_typeMove)
		{
			case TypeMove.Horizontal:
				if (Mathf.Abs(_myTransform.anchoredPosition.x) > _distanceToPositionFirst)
				{
					_myTransform.anchoredPosition = _firstPostion;
				}
				else
					_myTransform.anchoredPosition = _secondPostion;
				break;

			case TypeMove.Vertical:
                if (Mathf.Abs(_myTransform.anchoredPosition.y) > _distanceToPositionFirst)
                {
                    _myTransform.anchoredPosition = _firstPostion;
                }
                else
                    _myTransform.anchoredPosition = _secondPostion;
                break;
		}
	}

	public void GetPrymaryTouch(InputAction.CallbackContext context)
	{
		_prymaryposition = context.ReadValue<Vector2>();
	}

}
