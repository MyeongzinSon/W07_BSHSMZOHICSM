using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Serialization;

public class ControllerSelectionUI : MonoBehaviour
{
    //할당된 번호
    public ControllerSelector selector;
    public int playerId = 0;
    
    //UI Text
    public TMP_Text keyText;
    public TMP_Text confirmText;
    
    //선택된 장치 정보
    private string selectedDevice = "";
    private bool CanSelect => !selectedDevice.Equals("");

    //확인키 두번 눌러야 확정
    private int confirmCount = 0;

    bool CheckValid()
    {
        //이전에 선택된 디바이스가 내가 선택한 디바이스와 같은지 확인
        for (int i = 0; i < playerId; i++)
        {
            if (ControllerSelector.inputDeviceId[i] == ControllerSelector.inputDeviceId[playerId])
            {
                return false;
            }
        }
        return true;
    }

    void SetSelectedDevice(InputDevice _device)
    {
        
        if (_device is XInputController)selectedDevice = "Xbox 컨트롤러";
        else if (_device is Gamepad) selectedDevice = "게임패드";
        else if (_device is Keyboard) selectedDevice = "키보드 & 마우스";
        else if (_device is Joystick) selectedDevice = "조이스틱";
        
        keyText.text = "선택된 기기: " + selectedDevice;
        ControllerSelector.inputDeviceId[playerId] = _device.deviceId;
        
        
        if (CheckValid())
        {
            confirmText.text = "준비가 완료되면 장치의 확인 키를 눌러주세요.";
        }
        else
        {
            confirmText.text = "해당 장치는 이미 다른 플레이어가 선택한 장치입니다.";
        }
        confirmCount = 0;
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        Debug.Log("아!!!!");
        if (ControllerSelector.inputDeviceId[playerId] != _context.control.device.deviceId)
        {
            //만약 입력 장치가 바뀌었다면, 모든 것을 초기화하고 기기를 다시 할당
            SetSelectedDevice(_context.control.device);
        }
    }

    public void OnSelection(InputAction.CallbackContext _context)
    {
        if(!_context.started)
            return;

        if (!CheckValid())
        {
            //다른 플레이어가 사용하는 장치라면, 그냥 텍스트만 출력하고 아무일도 일어나지 않음.
            SetSelectedDevice(_context.control.device);
        }
        else if (ControllerSelector.inputDeviceId[playerId] != _context.control.device.deviceId)
        {
            //만약 입력 장치가 바뀌었다면, 모든 것을 초기화하고 기기를 다시 할당
            SetSelectedDevice(_context.control.device);
        }
        else
        {
            //입력 장치가 그대로라면, 같은 장치로 두번 연속 확인을 눌러야 확정한다.
            if (confirmCount == 0)
            {
                confirmText.text = "이 컨트롤러로 확정하려면 한번 더 눌러주세요!";
                confirmCount = 1;
            }   
            else if (confirmCount == 1)
            {
                confirmCount = 2;
                confirmText.text = $"Player{playerId+1}: {selectedDevice}으로 확정!!";
                selector.ChangeFocus();
            }
        }
        
    }
}
