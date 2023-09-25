using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Dictionary<string, GameObject> UIObjects = new Dictionary<string, GameObject>();
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("UI"))
            {
                UIObjects.Add(obj.name, obj);
                Debug.Log(obj.name);
                obj.SetActive(false);
            }
        }
    }
}