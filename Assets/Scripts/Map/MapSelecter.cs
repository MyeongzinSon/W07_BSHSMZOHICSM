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

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
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
