using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class ControllerSelector : MonoBehaviour
{
    //Static Variables
    public const int maxPlayerNum = 2;
    public static int[] inputDeviceId = new int[maxPlayerNum];
    
    //할당된 번호
    public int selectionNumber = 0;
    
    //UI Text
    public TMP_Text keyText;
    public TMP_Text confirmText;
    
    //현재 선택중인 플레이어 번호
    private int playerId = 0;
    
    //선택된 장치 정보
    private string selectedDevice = "";
    private bool CanSelect => !selectedDevice.Equals("");

    //확인키 두번 눌러야 확정
    private int confirmCount = 0;

    void SetSelectedDevice(InputDevice _device)
    {
        Debug.Log(_device.deviceId);
        if (_device is XInputController)selectedDevice = "Xbox 컨트롤러";
        else if (_device is Gamepad) selectedDevice = "게임패드";
        else if (_device is Keyboard) selectedDevice = "키보드 & 마우스";
        else if (_device is Joystick) selectedDevice = "조이스틱";
        
        keyText.text = "선택된 기기: " + selectedDevice;
        inputDeviceId[selectionNumber] = _device.deviceId;
        
        confirmText.text = "준비가 완료되면 장치의 확인 키를 눌러주세요.";
        confirmCount = 0;
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        if (inputDeviceId[selectionNumber] != _context.control.device.deviceId)
        {
            //만약 입력 장치가 바뀌었다면, 모든 것을 초기화하고 기기를 다시 할당
            SetSelectedDevice(_context.control.device);
        }
    }

    public void OnSelection(InputAction.CallbackContext _context)
    {
        if(!_context.started)
            return;
        
        if (inputDeviceId[selectionNumber] != _context.control.device.deviceId)
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
                confirmText.text = $"Player{playerId}: {selectedDevice}으로 확정!!";
            }
        }
        
    }
    
}
