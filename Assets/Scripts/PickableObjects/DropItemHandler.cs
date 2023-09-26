using System.Collections;
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
        StartCoroutine(CreateDropItem());
    }
    private IEnumerator FadeInAndOut()
    {
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            // Calculate the alpha value based on the current time
            alpha = Mathf.PingPong(timer / fadeInterval, .5f);

            // Apply the alpha to the SpriteRenderer
            spriteRenderer.color = new Color(1.0f, 1.0f, 0.0f, alpha);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CreateDropItem()
    {
        yield return new WaitForSeconds(2f);
        if (createIdx == 0)
        {
            Instantiate(Resources.Load("Prefabs/DropItems/HealPack"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (createIdx == 1)
        {
            Instantiate(Resources.Load("Prefabs/DropItems/EnforcePack"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}