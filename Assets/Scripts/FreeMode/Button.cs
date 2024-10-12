using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public static Button instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _panel;
    bool IsOpenVar = true;

    private void Awake()
    {
        instance = this;
    }
    public void GetNavigation()
    {
        AppController.Instance.SetPosition();
    }
    public void VarPanel()
    {
        IsOpenVar = !IsOpenVar;
        _animator.SetBool("IsOpen",IsOpenVar);
    }
    public void OpenClosePanel()
    {
        _panel.SetActive(!_panel.gameObject.activeSelf);
    }
    public void OpenClosePanel(GameObject gameObj)
    {
        gameObj.SetActive(!gameObj.activeSelf);
    }

}
