using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _itemPrefab;

    private void Awake()
    {
        Instance = this;
    }
    public void KabinetPanel(DataKorpus dataKorpus)
    {
        for (int i = 1; i < dataKorpus.KabinetList.Count; i++)
        {
            KabinetItem _myItem = Instantiate(_itemPrefab).GetComponent<KabinetItem>();
            _myItem.SetItemKabinet(dataKorpus.KabinetList[i].NameKabinet, dataKorpus.KabinetList[i].Etage, AppController.Instance.GetKorpusValue(), i);
            _myItem.transform.SetParent(_content);
        }
    }
    public void ClearKabinetPanel()
    {
        for (int i = _content.childCount - 1; i >= 0 ; i--)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
    }
}
