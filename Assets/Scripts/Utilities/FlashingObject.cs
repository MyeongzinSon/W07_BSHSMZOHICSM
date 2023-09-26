using UnityEngine;
using System.Collections;
 
public class FlashingObject : MonoBehaviour {
    
    private Color[] colors = {Color.white, Color.red};
 
    public void MakeObjectFlashSprite(SpriteRenderer sprite,
                                float durationTime, float intervalTime)
    {
        StartCoroutine(FlashSprite(sprite, durationTime, intervalTime));
    }
   
    IEnumerator FlashSprite(SpriteRenderer sprite, float time, float intervalTime)
    {
        Color originalColor = sprite.color;
        float elapsedTime = 0f;
        int index = 0;
        while(elapsedTime < time)
        {
            sprite.color = colors[index % 2];
           
            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        sprite.color = originalColor;
       
    }
}