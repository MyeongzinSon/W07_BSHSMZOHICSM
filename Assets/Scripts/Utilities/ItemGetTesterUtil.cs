using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetTesterUtil : MonoBehaviour
{
	[Header("대상 플레이어")]
	public CharacterStats target;
	
	[Header("강화 내용의 스크립터블 오브젝트")]
	public CharacterStatsData tkdaeBro;


	public void UseTkdaehyeongShuriken()
	{
		target.AddCharacterStats(tkdaeBro);
	}
}
