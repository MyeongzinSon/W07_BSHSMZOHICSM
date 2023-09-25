using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRoll : MonoBehaviour, NewInputActions.IPlayerActions
{
    private NewInputActions inputs;
    private PlayerMove playerMove;
    private Rigidbody2D playerRigidbody;

    [Header("������ ����")]
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollCoolTime;
    [SerializeField] private float rollingTime;
    [SerializeField] private float elapsedTime;
    [SerializeField] private int rollCurrentFrequency;
    [SerializeField] private int rollMaxFrequency;
    [SerializeField] private bool isRollingCoroutinePlaying = false;

    private void Awake()
    {
        inputs = new();
        inputs.Player.SetCallbacks(this);
        inputs.Enable();

        TryGetComponent(out playerMove);
        TryGetComponent(out playerRigidbody);
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
    public void OnFire(InputAction.CallbackContext _context)
    {
    }

    public void OnLook(InputAction.CallbackContext _context)
    {
    }

    public void OnLookOnMouse(InputAction.CallbackContext _context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
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
                        Debug.Log("��Ÿ�� ��ٸ��� ��");
                    }
                }
                else
                {
                    Debug.Log("������ ��");
                }
            }
            else
            {
                Debug.Log("���� �Է� ����");
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
