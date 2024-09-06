using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public static Button instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        instance = this;
    }
    bool IsOpenVar;
    public void LoadingScenes(string NameScene)
    {
        StartCoroutine(LoadingScenes());

        IEnumerator LoadingScenes()
        {
            SceneManager.LoadSceneAsync(NameScene);
            yield return new WaitForSeconds(0);
        }
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
}
