using UnityEngine;

public class InfoToCabinet : MonoBehaviour
{
    [SerializeField] private InfoItem _itemP;
    [SerializeField] private InfoItem _panel;
    [SerializeField] private bool _isOpen;

    public void InfoToOpen()
    {
        if(_panel is null)
        {
            _panel = Instantiate(_itemP);
            _panel.transform.SetParent(InfoParent.Instance.Parent);
        }
        _isOpen = !_isOpen;
        if (_isOpen)
        {
            _panel.transform.position = InfoParent.Instance.MyPositionToCanvas(transform.position);
            _panel.gameObject.SetActive(true);
        }
        else
        {
            _panel.gameObject.SetActive(false);
        }
    }
}