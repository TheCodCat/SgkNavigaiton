using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{
	[SerializeField] private float _value;
	[SerializeField] private Scrollbar _scrollbar;
	public void ChangeValueScrollbar( float value)
	{
		Debug.Log(value);
	}
}
