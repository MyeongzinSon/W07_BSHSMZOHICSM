using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pick();
        }
    }

    public virtual void Pick()
    {
        //ADD Pick Effect or Sound
    }
}
