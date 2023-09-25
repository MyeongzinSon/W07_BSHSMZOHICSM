using UnityEngine;

public class ShurikenMovementTest : MonoBehaviour
{
    public float moveSpeed = 5f; // Shuriken의 이동 속도
    public float particleSpawnInterval = 0.1f; // 파티클 생성 간격
    public float reverseDelay = 0.5f; // 반전 딜레이 시간
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float lastParticleSpawnTime;
    private float lastReverseTime;

    public GameObject shurikenParticlePrefab; // 파티클 프리팹

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 초기 이동 방향 설정 (예: 오른쪽으로 이동)
        moveDirection = Vector2.right;
        lastParticleSpawnTime = Time.time; // 초기 시간 설정
        lastReverseTime = Time.time; // 초기 시간 설정
    }

    void Update()
    {
        // Shuriken을 이동 방향과 속도로 이동시킵니다.
        rb.velocity = moveDirection * moveSpeed;

        // Shuriken이 화면을 벗어나면 방향을 반대로 변경합니다.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            // 반전 딜레이 이후에 반전합니다.
            if (Time.time - lastReverseTime >= reverseDelay)
            {
                // X와 Y 축 방향을 반전합니다.
                moveDirection.x *= -1;
                moveDirection.y *= -1;
                lastReverseTime = Time.time; // 반전한 시간 업데이트
            }
        }

        // Shuriken을 회전시킵니다.
        transform.Rotate(Vector3.forward * 360f * Time.deltaTime);

        // 일정 시간 간격마다 파티클 생성
        if (Time.time - lastParticleSpawnTime >= particleSpawnInterval)
        {
            lastParticleSpawnTime = Time.time; // 파티클 생성 시간 업데이트
            SpawnShurikenParticle();
        }
    }

    // 파티클을 생성하는 함수
    void SpawnShurikenParticle()
    {
        if (shurikenParticlePrefab != null)
        {
            Instantiate(shurikenParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}