using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    // 데미지를 입었을 때 깜빡이는 이미지
    public Image image;

    // 페이드아웃 속도 (낮을수록 오래 지속됨)
    public float flashSpeed;

    // 현재 실행 중인 코루틴 (중복 실행 방지용)
    private Coroutine coroutine;

    private void Start()
    {
        // 플레이어가 피해를 입었을 때 Flash() 실행하도록 이벤트 등록
        CharacterManager.Instance.Player.condition.onTakeDamaged += Flash;
    }

    /// <summary>
    /// 데미지 효과를 나타내는 메서드
    /// </summary>
    public void Flash()
    {
        // 이전에 실행 중이던 코루틴이 있다면 중지
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // 이미지 활성화 및 색상 설정 (연한 붉은색)
        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f);

        // 페이드아웃 코루틴 시작
        coroutine = StartCoroutine(FadeAway());
    }

    /// <summary>
    /// 데미지 효과를 서서히 사라지게 하는 페이드아웃 코루틴
    /// </summary>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f; // 초기 알파값 (투명도)
        float a = startAlpha;

        // 알파값이 0이 될 때까지 감소
        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        // 페이드아웃이 완료되면 이미지 비활성화
        image.enabled = false;
    }
}
