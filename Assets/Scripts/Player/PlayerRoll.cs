using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerRoll : MonoBehaviour
{
    private Mover playerMove;
    private Rigidbody2D playerRigidbody;

    [Header("구르기 스펙")]
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollCoolTime;
    [SerializeField] private float rollingTime;
    [SerializeField] private float elapsedTime;
    [SerializeField] private int rollCurrentFrequency;
    [SerializeField] private int rollMaxFrequency;
    [SerializeField] private bool isRollingCoroutinePlaying = false;

    private bool isOnSpiderWeb = false;

    public bool CanRoll { get {
            bool direction = playerMove.direction != Vector2.zero;
            bool rolling = !isRollingCoroutinePlaying;
            bool cooltime = rollCurrentFrequency < rollMaxFrequency;
            //Debug.Log($"CanRoll : direction = {direction}, rolling = {rolling}, cooltime = {cooltime}");
            return direction && rolling && cooltime && (isOnSpiderWeb == false);
                } }

    private void Awake()
    {
        TryGetComponent(out playerMove);
        TryGetComponent(out playerRigidbody);
    }
    private void Start()
    {
        if (TryGetComponent<CharacterStats>(out var stats))
        {
            rollDistance = stats.rollDistance;
            rollCoolTime = stats.rollCooldown;
            rollMaxFrequency = stats.maxRollNum;
        }
    }
    public void OnRoll(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            TryRoll();
        }
    }
    public bool TryRoll()
    {
        if (isOnSpiderWeb)
        {
            Debug.Log("거미줄 위에 있어서 구르기 불가능");
            GameManager.Instance.CreateImageText("고정됨!", new Color(.9f, .9f, .9f), transform.position);
            return false;
        }
        if (playerMove.direction != Vector2.zero)
        {
            if (!isRollingCoroutinePlaying)
            {
                if (rollCurrentFrequency < rollMaxFrequency)
                {
                    StartCoroutine(nameof(RollCoroutine));
                    return true;
                }
                else
                {
                    Debug.Log("쿨타임 기다리는 중");
                }
            }
            else
            {
                Debug.Log("구르는 중");
            }
        }
        else
        {
            Debug.Log("방향 입력 없음");
        }
        return false;
    }

    private IEnumerator RollCoroutine()
    {
        TryGetComponent<NavMeshAgent>(out var agent);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Stretch");
        transform.GetChild(0).GetComponent<PlayerAfterImageHandler>().RepeatSpawnTrail();
        Vector2 direction = playerMove.direction;
        direction = direction.normalized;
        rollCurrentFrequency++;
        elapsedTime = 0f;
        playerMove.CanMove = false;
        isRollingCoroutinePlaying = true;

        while (elapsedTime < rollingTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (agent != null)
            {
                agent.Move(rollDistance * Time.fixedDeltaTime * direction);
            }
            else
            {
                playerRigidbody.MovePosition(playerRigidbody.position + rollDistance * Time.fixedDeltaTime * direction);
            }
            yield return new WaitForFixedUpdate();
        }

        playerMove.CanMove = true;
        isRollingCoroutinePlaying = false;

        yield return new WaitForSeconds(rollCoolTime);

        rollCurrentFrequency--;
    }
    
    public void SetVariables(float _speed, float _cooltime)
    {
        rollDistance = _speed;
        rollCoolTime = _cooltime;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpiderWeb"))
        {
            isOnSpiderWeb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SpiderWeb"))
        {
            isOnSpiderWeb = false;
        }
    }
}
