using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    private bool isMoving = true;
    private float moveSpeed = 1.5f;
    private float disappearTime = 1.0f;
    private float currentTime = 0.0f;
    public string _text;

    private void Start()
    {
        GetComponent<TextMesh>().text = _text;
    }
    private void Update()
    {
        if (isMoving)
        {
            // Y 값을 1초 동안 moveSpeed만큼 올립니다.
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;

            // 1초가 지나면 이동을 멈춥니다.
            if (currentTime >= 1.0f)
            {
                isMoving = false;
                currentTime = 0.0f;
            }
        }
        else
        {
            // 게임 오브젝트를 투명하게 만들고 사라지도록 합니다.
            Color currentColor = GetComponent<Renderer>().material.color;
            currentColor.a -= Time.deltaTime / disappearTime;
            GetComponent<Renderer>().material.color = currentColor;

            // 투명해진 후에 파괴합니다.
            if (currentColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}