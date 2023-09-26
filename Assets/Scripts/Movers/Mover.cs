using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class Mover : MonoBehaviour
{
	public bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }


    [Header("속도 관련")]
    public float speed;
    public float accel;
    public Vector2 direction;
    public Vector2 angularAccel;

    protected Rigidbody2D body;

    public Vector2 lastInput = new Vector2(0,-1);

    [Header("이동 방향을 바라볼지 결정합니다.")]
    public bool isRotate = false;

    private void Start()
    {
        if (TryGetComponent<CharacterStats>(out var stats))
        {
            speed = stats.moveSpeed;
        }
    }
    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            lastInput = direction;
        }
        BeforeMove();
        if (CanMove)
        {
            Move();
            OnMove();
            CalculateDelta();
        }
        AfterMove();
    }

    public virtual void Move()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        body.MovePosition(body.position + speed * Time.fixedDeltaTime * direction.normalized);
        
        if (isRotate)
        {
            //Direction방향으로 Rotation값 수정
            SetRotationByDirection();
        }
        //body.velocity = direction.normalized * speed;
    }

    public void CalculateDelta()
    {
        //속도, 가속도, 각속도 계산
        speed += accel * Time.fixedDeltaTime;
        direction += angularAccel * Time.fixedDeltaTime;
    }

    public float SetRotationByDirection()
    {
        Vector2 a = Vector2.right;
        Vector2 b = direction.normalized;
        float angle = Vector2.Angle(a,b);
        float t = ((a.x*b.y - b.x*a.y)>=0)?1f:-1f;
        transform.rotation = Quaternion.Euler(0f,0f,angle*t);
        return angle*t;
    }

    public virtual void BeforeMove() { }
    public virtual void OnMove() { }
    public virtual void AfterMove() { }
}
