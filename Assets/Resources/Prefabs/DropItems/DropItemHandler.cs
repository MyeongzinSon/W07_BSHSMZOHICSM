using UnityEngine;

public class DropItemHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isFadingIn = true;
    private float alpha = 0.0f;
    private float fadeInterval = 0.5f;
    private float fadeDuration = 2.0f;

    public int createIdx; // 0 for HealPack, 1 for EnforcePack

    private void Start()
    {
        spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInAndOut());
    }
    private System.Collections.IEnumerator FadeInAndOut()
    {
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            // Calculate the alpha value based on the current time
            alpha = Mathf.PingPong(timer / fadeInterval, 1.0f);

            // Apply the alpha to the SpriteRenderer
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        
        if (createIdx == 0 && alpha >= 1.0f)
        {
            Instantiate(Resources.Load("Prefabs/DropItems/HealPack"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (createIdx == 1 && alpha >= 1.0f)
        {
            Instantiate(Resources.Load("Prefabs/DropItems/EnforcePack"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}