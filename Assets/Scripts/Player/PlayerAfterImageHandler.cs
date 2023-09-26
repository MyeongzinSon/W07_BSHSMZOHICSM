using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageHandler : MonoBehaviour
{
    public void RepeatSpawnTrail()
    {
        InvokeRepeating("SpawnTrail", 0.0f, 0.05f);
        StartCoroutine(CancelRepeating());
    }
    
    IEnumerator CancelRepeating()
    {
        yield return new WaitForSeconds(0.3f);
        CancelInvoke("SpawnTrail");
    }
    public void SpawnTrail()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        if (trailPartRenderer)
        {
            trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
            trailPart.transform.position = transform.position;
            Destroy(trailPart, 0.2f);
            StartCoroutine("FadeTrailPart", trailPartRenderer);
        }
        
    }
 
    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.7f;
        trailPartRenderer.color = color;
 
        yield return new WaitForEndOfFrame();
    }
}
