using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueZone : MonoBehaviour
{
    private List<Damageable> objectOut = new List<Damageable>();

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float damageElapsedTime;
    [SerializeField] private float damageMaxTime;

    [Header("Shrink")]
    [SerializeField] private float shrinkTime;
    [SerializeField] private float shrinkElapsedTime;
    private Vector3 initialScale;
    public float shrinkDelay;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Damageable>(out Damageable obj))
        {
            objectOut.Remove(obj);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Damageable>(out Damageable obj) && obj.gameObject.activeSelf)
        {
            objectOut.Add(obj);  
        }
    }
    private void Update()
    {
        Shrink();
        GetDamage();
    }

    private void GetDamage()
    {
        damageElapsedTime += Time.deltaTime;
        if (damageElapsedTime >= damageMaxTime)
        {
            damageElapsedTime = 0f;
            foreach (Damageable obj in objectOut)
            {
                obj.Hit(damage);
            }
        }
    }

    private void Shrink()
    {
        shrinkElapsedTime += Time.deltaTime;
        if (shrinkElapsedTime >= shrinkDelay)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, Mathf.Clamp01((shrinkElapsedTime - shrinkDelay) / shrinkTime));
        }
    }
}
