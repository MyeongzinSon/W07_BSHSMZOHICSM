using UnityEngine;
using System.Collections;
 
public class FlashingObject : MonoBehaviour {
 
    /// <summary>
    /// Singleton
    /// </summary>
    public static FlashingObject Instance;
   
    private Color[] colors = {Color.white, Color.red};
   
    public void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of FlashingObject!");
        }
        Instance = this;
    }
   
    public void MakeObjectFlash(Material material,
                                float durationTime, float intervalTime)
    {
        StartCoroutine(Flash(material, durationTime, intervalTime));
    }
   
    IEnumerator Flash(Material material, float time, float intervalTime)
    {
       
        Color originalColor = material.color;
       
        float elapsedTime = 0f;
        int index = 0;
        while(elapsedTime < time )
        {
            material.color = colors[index % 2];
           
            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
       
        material.color = originalColor;
       
    }
 
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
        while(elapsedTime < time )
        {
            sprite.color = colors[index % 2];
           
            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        Debug.Log("originalColor: " + originalColor);
        sprite.color = originalColor;
       
    }
 
    public void MakeObjectFlashSprites(SpriteRenderer[] sprites,
                                       int numTimes, float delay)
    {
        StartCoroutine(FlashSprites(sprites, numTimes, delay));
    }
 
    /**
     * Coroutine to create a flash effect on all sprite renderers passed in to the function.
     *
     * @param sprites   a sprite renderer array
     * @param numTimes  how many times to flash
     * @param delay     how long in between each flash
     * @param disable   if you want to disable the renderer instead of change alpha
     */
    IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float delay, bool disable = false) {
        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++) {          
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {              
                if (disable) {
                    // for disabling
                    sprites[i].enabled = false;
                } else {
                    // for changing the alpha
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 0.5f);
                    // Debug.Log(sprites[i].color.r + " " + sprites[i].color.g + " " + sprites[i].color.b);
                    // sprites[i].color = new Color(0, 100, 0, 0.5f);
                }
            }
           
            // delay specified amount
            yield return new WaitForSeconds(delay);
           
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {
                if (disable) {
                    // for disabling
                    sprites[i].enabled = true;
                } else {
                    // for changing the alpha
                    // Debug.Log(sprites[i].color.r + " " + sprites[i].color.g + " " + sprites[i].color.b);
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 1);
                    //sprites[i].color = new Color(0, 100, 0, 1);
                }
            }
           
            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }
 
   
}