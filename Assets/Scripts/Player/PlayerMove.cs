using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Mover
{
    [Header("넉백 관련")]
    public float knockbackTime = 0.25f;
    [Header("슬로우 관련")]
    public float slowRatio = 0.2f;
    private int slowCount = 0;
    //public Vector2 lastInput;
    
    

    public void OnMove(InputAction.CallbackContext _callback)
    {
        Vector2 input = _callback.ReadValue<Vector2>();
        direction = input;
        if (input != Vector2.zero)
        {
            lastInput = direction;
        }
    }

    public override void Move()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        
        //슬로우라면 슬로우 비율 지정해서 슬로우
        if(slowCount>0)
            body.MovePosition(body.position + (1f-slowRatio)*speed * Time.fixedDeltaTime * direction.normalized);
        else
            body.MovePosition(body.position + speed * Time.fixedDeltaTime * direction.normalized);
        
        
        if (isRotate)
        {
            //Direction방향으로 Rotation값 수정
            SetRotationByDirection();
        }
        //body.velocity = direction.normalized * speed;
    }


    public void KnockBack(Vector2 direction, float power)
    {
        if (CanMove
            &&direction!=Vector2.zero)
        {
            body.velocity = Vector2.zero;
            body.AddForce(direction.normalized * power);
            StartCoroutine(SetKnockBackTimer());
        }
    }

    private IEnumerator SetKnockBackTimer()
    {
        CanMove = false;
        yield return new WaitForSeconds(knockbackTime);
        CanMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpiderWeb"))
        {
            //스파이더웹이라면 슬로우 On
            slowCount += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpiderWeb"))
        {
            //스파이더웹이라면 슬로우 On
            slowCount -= 1;
        }
    }
}
