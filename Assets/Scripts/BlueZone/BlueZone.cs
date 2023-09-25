using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueZone : MonoBehaviour
{
    private List<Damageable> objectOut = new List<Damageable>();

    private CircleCollider2D circleCollider;

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float damageElapsedTime;
    [SerializeField] private float damageMaxTime;

    [Header("Shrink")]
    [SerializeField] private float shrinkSizePerSecond;
    [SerializeField] private float shrinkElapsedTime;
    public float shrinkDelay;

    private void Awake()
    {
        TryGetComponent(out circleCollider);
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
        if (collision.TryGetComponent<Damageable>(out Damageable obj))
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
        if (shrinkElapsedTime < shrinkDelay)
        {
            shrinkElapsedTime += Time.deltaTime;
        }
        else
        {
            circleCollider.radius -= shrinkSizePerSecond * Time.deltaTime;
        }
    }
}
