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
        //풀피일때 삭제가 되어야 하는가?
        gameObject.SetActive(false);
        //ADD Pick Effect or Sound
    }
}
