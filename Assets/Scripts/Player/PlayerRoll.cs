using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRoll : MonoBehaviour
{
    private PlayerMove playerMove;
    private Rigidbody2D playerRigidbody;

    [Header("구르기 스펙")]
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollCoolTime;
    [SerializeField] private float rollingTime;
    [SerializeField] private float elapsedTime;
    [SerializeField] private int rollCurrentFrequency;
    [SerializeField] private int rollMaxFrequency;
    [SerializeField] private bool isRollingCoroutinePlaying = false;

    private void Awake()
    {
        TryGetComponent(out playerMove);
        TryGetComponent(out playerRigidbody);
    }

    public void OnRoll(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            if (playerMove.direction != Vector2.zero)
            {
                if (!isRollingCoroutinePlaying)
                {
                    if (rollCurrentFrequency < rollMaxFrequency)
                    {
                        StartCoroutine(nameof(RollCoroutine));
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
        }
    }

    private IEnumerator RollCoroutine()
    {
        Vector2 direction = playerMove.direction;
        direction = direction.normalized;
        rollCurrentFrequency++;
        elapsedTime = 0f;
        playerMove.CanMove = false;
        isRollingCoroutinePlaying = true;

        while (elapsedTime < rollingTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + rollDistance * Time.fixedDeltaTime * direction);
            yield return new WaitForFixedUpdate();
        }

        playerMove.CanMove = true;
        isRollingCoroutinePlaying = false;

        yield return new WaitForSeconds(rollCoolTime);

        rollCurrentFrequency--;
    }

}
