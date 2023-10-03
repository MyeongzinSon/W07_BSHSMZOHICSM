using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{
    const float cameraExpandDelay = 0.5f;

    [SerializeField] int playerIndex;

    private PlayerMove move;
    private PlayerRoll roll;
    private ShurikenShooter attack;

    bool isLooking = false;
    float cameraExpandTimer = -1f;

    InputDevice assignedDevice => ControllerSelector.inputDevices[playerIndex];
    bool IsKeyboardInput => assignedDevice is Keyboard;

    void Awake()
    {
        TryGetComponent(out move);
        TryGetComponent(out roll);
        TryGetComponent(out attack);
    }

    void Start()
    {
        //PlayerInput의 Scheme을 이 플레이어가 사용할 기기로 교체합니다.

        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputDevice[] schemeDevices;
        //할당된 기기가 Keyboard일 경우, Mouse를 함께 할당해주어야 합니다.
        if (assignedDevice.device is Keyboard)
        {
            Mouse currentMouse = null;
            foreach (var v in InputSystem.devices)
            {
                if (v is Mouse mouse)
                {
                    currentMouse = mouse;
                }
            }
            schemeDevices = new[] {assignedDevice, currentMouse};
        }
        else
        {
            schemeDevices = new[] {assignedDevice};
        }

        if (!playerInput.SwitchCurrentControlScheme(schemeDevices))
        {
            Debug.LogError($"{gameObject.name}에 적합한 Input Device Scheme을 찾지 못했습니다.");
        }
    }


    void Update()
    {
        SetAttackDirectionWithMouse();
        if (cameraExpandTimer >= 0)
        {
            cameraExpandTimer += Time.deltaTime;
            if (cameraExpandTimer >= cameraExpandDelay)
            {
                // set PlayerNum
                VCamManager.Instance.Expand(0);
            }
        }
    }

    void SetAttackDirectionWithMouse()
    {
        if (!IsKeyboardInput)
        {
            return;
        }
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        attack.SetDirection(dir);
    }

    bool CheckMyDevice(InputAction.CallbackContext context)
    {
        //Debug.Log($"{gameObject.name}: CheckMyDevice : {context.control.device.name} == {assignedDevice.name}");
        if (IsKeyboardInput
            && context.control.device is Mouse)
        {
            return true;
        }

        return context.control.device == assignedDevice;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        string temp = "";
        foreach (var t in GetComponent<PlayerInput>().devices)
        {
            temp += t.name + ",";
        }
        Debug.Log($"{gameObject.name}.Move : " + temp);


        if (!CheckMyDevice(context))
        {
            //Debug.Log($"-After Check : index={playerIndex}");
            return;
        }

        //Debug.Log($"클릭한 디바이스: {context.control.device.name}");

        if (GameManager.Instance.isBattleStart)
        {
            move.OnMove(context);
            //if (!isLooking)
            //         {
            //	attack.SetDirection(context.ReadValue<Vector2>());
            //         }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!CheckMyDevice(context)) return;

        var direction = context.ReadValue<Vector2>();
        if (!IsKeyboardInput && direction != Vector2.zero)
        {
            attack.SetDirection(direction);
        }
        if (context.started)
        {
            isLooking = true;
            Debug.Log($"isLooking = true");
        }
        else if (context.canceled)
        {
            isLooking = false;
            Debug.Log($"isLooking = false");
        }
    }

    public void OnLookOnMouse(InputAction.CallbackContext context)
    {
        if (!CheckMyDevice(context)) return;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!CheckMyDevice(context)) return;

        if (GameManager.Instance.isBattleStart)
        {
            if (context.started)
            {
                if (attack.StartCharge())
                    cameraExpandTimer = 0;
                if (IsKeyboardInput)
                {
                    isLooking = true;
                }
            }
            if (context.canceled)
            {
                SetAttackDirectionWithMouse();
                if (attack.EndCharge())
                {
                    cameraExpandTimer = -1;
                    VCamManager.Instance.Reduce();
                }
                if (IsKeyboardInput)
                {
                    isLooking = false;
                }
            }
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!CheckMyDevice(context)) return;

        if (GameManager.Instance.isBattleStart)
        {
            roll.OnRoll(context);
            attack.Cancel();
            cameraExpandTimer = -1;
            VCamManager.Instance.Reduce();
        }
    }
}
