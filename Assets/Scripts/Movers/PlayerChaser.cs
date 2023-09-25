using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    [Header("Trigger에 대상이 들어온다면 추적합니다.")]
    public GameObject target;
    public float maxDistanceFromPlayer = 0f;
    public float attackDelay = 1.0f;

    public float minimumIdleTime = 1.0f;
    public float minimumMoveTime = 0.5f;

    private SpellCaster caster;

    private Mover mover;


    //about Statement
    public enum State
    {
        IDLE, MOVE, ATTACK
    }
    private bool canStateChanged = true;
    public State nextState;

    private void Start()
    {
        canStateChanged = true;
        mover = transform.parent.GetComponent<Mover>();
        caster = transform.GetComponent<SpellCaster>();
    }

    private void Update()
    {
        if (canStateChanged)
        {
            StartNextState();
        }
    }

    private void StartNextState()
    {
        canStateChanged = false;
        if (nextState == State.IDLE)
        {
            StartCoroutine(IdleCoroutine(minimumIdleTime));
        }
        else if (nextState == State.MOVE)
        {
            StartCoroutine(MoveCoroutine(minimumMoveTime));
        }
        else if (nextState == State.ATTACK)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void PostStateEnd(State nextState)
    {
        this.nextState = nextState;
        canStateChanged = true;
    }

    IEnumerator IdleCoroutine(float time = 0.0f)
    {
        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (target != null && timer >= time)
            {
                PostStateEnd(State.MOVE);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator MoveCoroutine(float time = 0.0f)
    {
        float timer = 0.0f;
        //플레이어와의 거리가 n 이하라면
        mover.CanMove = true;
        while (target != null)
        {
            mover.direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

            timer += Time.deltaTime;
            if (timer>=time
                &&((Vector2)target.transform.position - (Vector2)transform.position).magnitude <= maxDistanceFromPlayer)
            {
                //공격 시전
                mover.CanMove = false;
                PostStateEnd(State.ATTACK);
                yield break;
            }
            yield return null;
        }
        //타겟이 사라졌으므로 
        mover.CanMove = false;
        PostStateEnd(State.IDLE);
        yield break;
    }

    IEnumerator AttackCoroutine()
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;
        //
        //Debug.Log(dir);
        //테스트용
        yield return new WaitForSeconds(attackDelay);

        if (caster != null)
        {
            caster.Cast(dir, transform.parent.gameObject, false);
        }
        PostStateEnd(State.IDLE);
        yield break;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //대상 레이어가 들어왔다면 Chase
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Player"))
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Player"))
        {
            target = null;
        }
    }


}
