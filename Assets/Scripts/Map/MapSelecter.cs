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

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {   
            Destroy(this.gameObject);
        }

        maps = FindObjectsOfType<OnMapActive>().ToList();

        StartRandomMap();
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

    public void StartRandomMap()
    {
        int num = Random.Range(0, maps.Count);
        if (maps[num] != null)
        {
            maps[num].Init();
            maps[num] = null;
        }
        else
        {
            StartRandomMap();
        }
    }
}
