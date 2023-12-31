using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapSelecter : MonoBehaviour
{
    [SerializeField]private List<OnMapActive> maps;

    private static MapSelecter instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {   
            Destroy(this.gameObject);
        }

        //maps = FindObjectsOfType<OnMapActive>().ToList();

        StartMap();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static MapSelecter Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void StartMap()
    {
        // num = Random.Range(0, maps.Count);
        int num = GameManager.Instance.stageCount-1;
        if (maps[num] != null)
        {
            maps[num].Init();
            //maps[num] = null;
        }
        else
        {
            StartMap();
        }
    }
}
