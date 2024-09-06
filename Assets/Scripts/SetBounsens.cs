using UnityEngine;

public class SetBounsens : MonoBehaviour
{
    Collider _col;

    private void OnEnable()
    {
        _col = GetComponent<Collider>();
        CameraMotor.Instance.SetConfiner(_col);
    }
}
