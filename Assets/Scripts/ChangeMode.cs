using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{
	[SerializeField] private GameObject _FreeMode;
	[SerializeField] private GameObject _ScheduleMode;

	public void ActiveFreeMode(bool active)
	{
		_ScheduleMode.SetActive(false);
		_FreeMode.SetActive(true);
	}

	public void ActiveScheduleMode(bool active)
	{
		_FreeMode.SetActive(false);
		_ScheduleMode.SetActive(true);
	}
}
