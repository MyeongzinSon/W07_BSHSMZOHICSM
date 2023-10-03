using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class VCamManager : MonoBehaviour
{

    private TestVCamBlend expandedVirtualCamera;
    private CinemachineVirtualCamera activeVirtualCamera;
    [SerializeField] private List<CinemachineBrain> brains = new List<CinemachineBrain>();

    // 싱글톤 인스턴스
    private static VCamManager instance;

    // 다른 스크립트에서 싱글톤에 접근할 수 있는 속성
    public static VCamManager Instance
    {
        get
        {
            // 인스턴스가 null인 경우 생성
            if (instance == null)
            {
                instance = FindObjectOfType<VCamManager>();

                // Scene에 존재하지 않는 경우 새로 생성
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SingletonExample");
                    instance = singletonObject.AddComponent<VCamManager>();
                }
            }
            return instance;
        }
    }

    public void Expand(int playerNum)
    {
        activeVirtualCamera = brains[playerNum].ActiveVirtualCamera as CinemachineVirtualCamera;
        expandedVirtualCamera = activeVirtualCamera.transform.parent.GetComponentInChildren<TestVCamBlend>();
        expandedVirtualCamera?.SetActiveCam(true);
    }

    public void Reduce()
    {
        expandedVirtualCamera?.SetActiveCam(false);
    }
}
