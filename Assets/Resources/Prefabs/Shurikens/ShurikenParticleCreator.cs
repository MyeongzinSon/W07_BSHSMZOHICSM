using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenParticleCreator : MonoBehaviour
{
    private GameObject shurikenParticlePrefab;
    public ShurikenShooter shurikenShooter;
    public Color[] colors = new Color[]
    {
        new Color(0.0f, 0.0f, 1.0f),      // 파란색
        new Color(0.647f, 0.165f, 0.165f), // brown색
        new Color(1.0f, 0.964f, 0.415f),  // 밝은 노랑
        new Color(1.0f, 0.647f, 0.0f),    // 주황색
        new Color(0.886f, 0.447f, 0.337f), // 황토색
        new Color(0.0f, 0.0f, 0.0f),      // 검은색
        new Color(0.4f, 1.0f, 0.0f),      // 초록색
        new Color(0.5f, 0.0f, 0.5f),      // 보라색
        new Color(0.933f, 0.702f, 0.537f), // 연한 황토색
        new Color(1.0f, 1.0f, 1.0f),      // 흰색
        new Color(0.647f, 0.0f, 0.0f),    // 진한 빨간색
        new Color(1.0f, 0.270f, 0.0f),    // 주황 빨간색
        new Color(1.0f, 0.0f, 0.0f)       // 빨간색
    };
    void Start()
    {
        shurikenParticlePrefab = (GameObject)Resources.Load("Prefabs/Particles/ShurikenParticle");
    }

    private void Update()
    {
        if (GetComponent<Shuriken>().state == Shuriken.ShurikenState.ATTACK)
        {
            InvokeRepeating("SpawnShurikenParticle", 0f, 0.3f);
        }
        else
        {
            CancelInvoke("SpawnShurikenParticle");
        }
    }

    void SpawnShurikenParticle()
    {
        if (shurikenParticlePrefab != null)
        {
            if (GetComponent<Shuriken>().owner.tag == "Player")
            {
                for (int i = 0; i < GameManager.Instance.upgradedListInt.Count; i++)
                {
                    GameObject particle = Instantiate(shurikenParticlePrefab, transform.position, Quaternion.identity);
                    var idx = GameManager.Instance.upgradedListInt[i];
                    particle.GetComponent<ShurikenParticleManager>().col = colors[idx];
                }
            }
            else
            {
                
            }
        }
    }
}
