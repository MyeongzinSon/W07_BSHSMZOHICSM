using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Pickable
{
    public override void Pick()
    {
        base.Pick();
        Debug.Log("Pick");
    }
}
