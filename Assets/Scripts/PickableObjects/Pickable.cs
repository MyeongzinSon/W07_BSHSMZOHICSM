using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        Pick(_collision);
    }

    public virtual void Pick(Collider2D _collision)
    {
        //Ǯ���϶� ������ �Ǿ�� �ϴ°�?
        gameObject.SetActive(false);
        //ADD Pick Effect or Sound
    }
}
