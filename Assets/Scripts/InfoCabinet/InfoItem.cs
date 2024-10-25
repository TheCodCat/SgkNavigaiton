using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoItem : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnPositionTouchDelta += MovePanel;
    }
    private void OnDisable()
    {
        InputManager.OnPositionTouchDelta -= MovePanel;
    }
    private void MovePanel(InputAction.CallbackContext context)
    {
        Debug.Log(CameraMotor.Instance.IsStop);
        if (CameraMotor.Instance.IsStop) return;

        transform.position += (Vector3)context.ReadValue<Vector2>();
    }
}
