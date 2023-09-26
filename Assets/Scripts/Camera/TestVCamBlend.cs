using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestVCamBlend : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private CinemachineVirtualCamera activeVirtualCamera;
    #endregion

    #region PublicMethod
    public void SetActiveCam(bool isActive)
    {
        activeVirtualCamera.Priority = isActive ? 10 : 0;
    }
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        TryGetComponent(out activeVirtualCamera);
    }
    #endregion
}
