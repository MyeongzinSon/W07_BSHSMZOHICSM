using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIClickFeedback : MonoBehaviour
{
    [SerializeField]
    private RectTransform uiElement; // 클릭 대상 UI 요소 RectTransform
    private float shakeDuration = .4f; // 흔들림 지속 시간
    private float shakeMagnitude = 20f; // 흔들림 범위

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = uiElement.anchoredPosition;
    }

    public void OnClick()
    {
        // 클릭시 흔들림 효과 적용
        StartCoroutine(ShakeUI());
    }

    private IEnumerator ShakeUI()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // 흔들림 효과를 위한 랜덤한 오프셋 생성
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                Random.Range(-1f, 1f) * shakeMagnitude,
                0f);

            uiElement.anchoredPosition = originalPosition + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 흔들림 효과가 끝난 후 UI 요소를 원래 위치로 되돌림
        uiElement.anchoredPosition = originalPosition;
    }
}