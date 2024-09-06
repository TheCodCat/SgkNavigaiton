using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KabinetItem : MonoBehaviour
{
    [SerializeField] private Text _itemText;
    [SerializeField] private int _indexKorpus;
    [SerializeField] private int _indexKabinet;
    [SerializeField] private int _etage;
    private Vector3 _newPos;
    private DataKorpus _dataKorpus;

    public void SetItemKabinet(string kabinet,int etage,int indexKorpus,int indexKabinet)
    {
        _itemText.text = $"{kabinet} - {etage} этаж";
        _indexKorpus = indexKorpus;
        _indexKabinet = indexKabinet;
        _etage = etage - 1;

        _dataKorpus = AppController.Instance.GetBD();
        _newPos.x = _dataKorpus.KabinetList[_indexKabinet].PositionKabinet.position.x;
        _newPos.y = CameraMotor.Instance.CameraCimenachin.transform.position.y;
        _newPos.z = _dataKorpus.KabinetList[_indexKabinet].PositionKabinet.position.z;
    }
    public void GetKabinet()
    {
        _indexKorpus = AppController.Instance.GetKorpus();
        AppController.Instance.EtageNavToggle(_etage);
        CameraMotor.Instance.MovomentToPos(new Vector3(_newPos.x, CameraMotor.Instance.CameraCimenachin.transform.position.y, _newPos.z));
        Button.instance.OpenClosePanel();
    }
}
