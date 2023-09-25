using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class VCamManager : MonoBehaviour
{

    private TestVCamBlend expandedVirtualCamera;
    private CinemachineVirtualCamera activeVirtualCamera;
    private CinemachineBrain brain;

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

    private void Awake()
    {
        brain = FindObjectOfType<CinemachineBrain>();
    }

    public void Expand()
    {
        activeVirtualCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        expandedVirtualCamera = activeVirtualCamera.transform.parent.GetComponentInChildren<TestVCamBlend>();
        expandedVirtualCamera?.SetActive(true);
    }

    public void Reduce()
    {
        expandedVirtualCamera?.SetActive(false);
    }
}
