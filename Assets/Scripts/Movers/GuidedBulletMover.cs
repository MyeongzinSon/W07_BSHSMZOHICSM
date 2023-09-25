using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuidedBulletMover : Mover
{
    [Header("유도탄 관련 속성")]
    private Vector2 startPos;
    private Vector2 delta1;
    private Vector2 delta2;

    public Transform target;

    public float eta = 1.0f;

    private float timer = 0f;
    private float reta;

    private bool initialized = false;
    public float bezierDelta = 10.0f;
    public float bezierDelta2 = 10.0f;

    public float refindRadius = 3f;

    private Attacker attacker;

    public void Init()
    {
        attacker = GetComponent<Attacker>();
        reta = 1 / eta;
        startPos = transform.position;
        delta1 = SetDelta(startPos);
        if (target == null)
        {
            FindNewTarget();
        }
        if (target != null)
        {
            delta2 = SetDelta(target.transform.position);
        }

        initialized = true;
    }

    private Vector2 SetDelta(Vector2 org)
    {
        float x, y;
        x = Mathf.Cos(Random.Range(0f, 360f) * Mathf.Deg2Rad) * bezierDelta + org.x;
        y = Mathf.Sin(Random.Range(0f, 360f) * Mathf.Deg2Rad) * bezierDelta + org.y;
        return new Vector2(x, y);
    }

    private void FindNewTarget()
    {
        LayerMask mask = attacker.damageLayer;
        Collider2D[] t = Physics2D.OverlapCircleAll(transform.position, refindRadius, mask);
        if (t.Length > 0)
        {
            Collider2D c = t[Random.Range(0, t.Length)];
            target = c.gameObject.transform;
        }
        else
        {
            Destroy(gameObject);
        }
        //Collider2d[] Physics2D.OverlapCircle(transform.position, 7f);
    }

    public override void AfterMove()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindNewTarget();
        }
        if (!initialized) return;
        body.position = new Vector2(
            Bezier(timer, startPos.x, delta1.x, delta2.x, target.transform.position.x),
            Bezier(timer, startPos.y, delta1.y, delta2.y, target.transform.position.y));
        timer += Time.fixedDeltaTime * reta;

        //Debug.Log(timer + ", (" + direction.x + ", " + direction.y + ")");
        //direction = Vector3.Slerp(direction.normalized, (target.position - transform.position).normalized, slerpCorrection);
    }

    private float Bezier(float t, float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * t * b * 3
            + Mathf.Pow(t, 2) * (1 - t) * c * 3
            + Mathf.Pow(t, 3) * d;
    }


}
