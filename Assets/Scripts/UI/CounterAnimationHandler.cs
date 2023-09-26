using System;
using System.Collections;
using UnityEngine;

public class CounterAnimationHandler : MonoBehaviour
{
    private Transform canvasTransform;
    private GameObject[] childObjects;
    
    void Start()
    {
        canvasTransform = GetComponent<Transform>();
        Invoke("StartAnimation", 0.5f);
    }
    
    void StartAnimation()
    {
        childObjects = new GameObject[canvasTransform.childCount];
        for (int i = 0; i < canvasTransform.childCount; i++)
        {
            childObjects[i] = canvasTransform.GetChild(i).gameObject;
        }
        
        StartCoroutine(ActivateChild1());
    }

    IEnumerator ActivateChild1()
    {
        childObjects[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ActivateChild2());
    }
    
    IEnumerator ActivateChild2()
    {
        childObjects[2].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ActivateChild3());
    }
    
    IEnumerator ActivateChild3()
    {
        childObjects[3].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveAndDestroyFirstChild());
    }
    
    IEnumerator MoveAndDestroyFirstChild()
    {
        //게임 시작 
        GameManager.Instance.EnterState(GameManager.GameState.Battle);
        if (childObjects.Length > 0)
        {
            Transform firstChildTransform = transform;
            LeanTween.moveY(firstChildTransform.gameObject, firstChildTransform.position.y + 300f, .2f)
                .setOnComplete(() => Destroy(transform.gameObject));
        }
        yield return null;
    }
}