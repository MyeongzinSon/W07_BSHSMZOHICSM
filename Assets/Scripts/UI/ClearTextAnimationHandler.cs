using System.Collections;
using UnityEngine;

public class ClearTextAnimationHandler : MonoBehaviour
{
    public float animationDuration = 1.0f; // 애니메이션 지속 시간
    public Vector3 targetScale = new Vector3(2.0f, 2.0f, 2.0f); // 목표 스케일

    private Transform myTransform;
    private Coroutine scaleCoroutine;

    private void OnEnable()
    {
        myTransform = transform;

        // 이전에 실행 중인 애니메이션을 중지하고 새로운 애니메이션을 시작
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsedTime = 0;
        Vector3 initialScale = myTransform.localScale;

        while (elapsedTime < animationDuration)
        {
            myTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 스케일을 목표 스케일로 설정
        myTransform.localScale = targetScale;

        // 다시 원래 스케일로 돌아가는 애니메이션 시작
        yield return new WaitForSeconds(1.0f); // 1초 딜레이 추가
        elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            myTransform.localScale = Vector3.Lerp(targetScale, Vector3.one, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 스케일을 Vector3(1f, 1f, 1f)로 설정
        myTransform.localScale = Vector3.one;
    }
}