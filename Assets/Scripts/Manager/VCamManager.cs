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

    // �̱��� �ν��Ͻ�
    private static VCamManager instance;

    // �ٸ� ��ũ��Ʈ���� �̱��濡 ������ �� �ִ� �Ӽ�
    public static VCamManager Instance
    {
        get
        {
            // �ν��Ͻ��� null�� ��� ����
            if (instance == null)
            {
                instance = FindObjectOfType<VCamManager>();

                // Scene�� �������� �ʴ� ��� ���� ����
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
