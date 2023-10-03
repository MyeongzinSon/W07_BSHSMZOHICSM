using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControllerSelector : MonoBehaviour
{
	//Static Variables
	public const int maxPlayerNum = 2;
	//public static int[] inputDeviceId = new int[maxPlayerNum];

	public static InputDevice[] inputDevices = new InputDevice[maxPlayerNum];
	
	//현재 선택된 플레이어
	public int currentPlayer = 0;

	[Header("각 Selector UI 게임오브젝트")]
	public GameObject[] panels;
	public ControllerSelectionUI[] selectionScripts;
	public Image[] blacks;

	private void Start()
	{
		foreach (var i  in selectionScripts)
		{
			i.selector = this;
		}
		currentPlayer = -1;
		ChangeFocus();
	}


	public void ChangeFocus()
	{
		currentPlayer += 1;
		for (int i = 0; i < maxPlayerNum; i++)
		{
			if (i == currentPlayer)
			{
				panels[i].transform.localScale = new Vector3(0.9f, 0.9f, 1f);
				selectionScripts[i].gameObject.SetActive(true);
				blacks[i].gameObject.SetActive(false);
			}
			else
			{
				panels[i].transform.localScale = new Vector3(0.6f, 0.6f, 1f);
				selectionScripts[i].gameObject.SetActive(false);
				blacks[i].gameObject.SetActive(true);
			}
		}

		if (currentPlayer >= maxPlayerNum)
		{
			Debug.Log($"Device 0 : {inputDevices[0].name}");
			Debug.Log($"Device 1 : {inputDevices[1].name}");

			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}
}
