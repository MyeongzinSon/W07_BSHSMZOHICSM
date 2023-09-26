using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenParticleManager : MonoBehaviour
{
    public Gradient colorGradient; // 원하는 색상 변화를 정의한 그래디언트
    public Color col;
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        
        // Particle System의 Main 모듈을 가져옵니다.
        var mainModule = particleSystem.main;

        // Color Over Lifetime 모듈을 가져옵니다.
        var colorOverLifetimeModule = particleSystem.colorOverLifetime;

        // 그래디언트를 설정합니다.
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(colorGradient);

        // Color Over Lifetime 모듈을 적용합니다.
        mainModule.startColor = col;
    }
}
