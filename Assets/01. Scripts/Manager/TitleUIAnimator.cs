using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Vector3 titleStartOffset = new Vector3(0, 200f, 0);
    public float titleSlideDuration = 1.0f;

    public GameObject[] buttons; // 각 버튼 GameObject 배열
    public float buttonFadeDuration = 0.5f;

    private Vector3 titleTargetPosition;

    private void Start()
    {
        titleTargetPosition = titleText.rectTransform.anchoredPosition;
        titleText.rectTransform.anchoredPosition = titleTargetPosition + titleStartOffset;

        foreach (var btn in buttons)
        {
            SetButtonAlpha(btn, 0f);
            btn.SetActive(true);
            SetButtonInteractable(btn, false);
        }

        int playAnimFlag = PlayerPrefs.GetInt("PlayTitleAnimation", 1); // 기본 1(재생)

        if (playAnimFlag == 1)
        {
            StartCoroutine(AnimateTitleAndButtons());
        }
        else
        {
            // 애니메이션 없이 바로 상태 세팅
            titleText.rectTransform.anchoredPosition = titleTargetPosition;
            foreach (var btn in buttons)
            {
                SetButtonAlpha(btn, 1f);
                SetButtonInteractable(btn, true);
            }
        }

        // 다음번 타이틀 씬 진입시 애니메이션이 다시 나오게 초기화
        PlayerPrefs.SetInt("PlayTitleAnimation", 1);
    }


    IEnumerator AnimateTitleAndButtons()
    {
        // 타이틀 텍스트 슬라이드 다운
        float elapsed = 0;
        while (elapsed < titleSlideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / titleSlideDuration);
            titleText.rectTransform.anchoredPosition = Vector3.Lerp(
                titleTargetPosition + titleStartOffset,
                titleTargetPosition,
                t
            );
            yield return null;
        }

        // 바운스 효과 추가
        yield return StartCoroutine(BounceTitle(0.3f, 30f, 3));

        // 버튼들 동시에 페이드 인
        yield return StartCoroutine(FadeInButtons(buttonFadeDuration));

        // 페이드 완료 후 인터랙션 활성화
        foreach (var btn in buttons)
        {
            SetButtonInteractable(btn, true);
        }
    }

    IEnumerator BounceTitle(float bounceDuration, float bounceHeight, int bounceCount)
    {
        Vector3 basePos = titleTargetPosition;
        for (int i = 0; i < bounceCount; i++)
        {
            // 위로 튀기기
            float elapsed = 0f;
            while (elapsed < bounceDuration / 2)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (bounceDuration / 2);
                // EaseOutQuad 모션
                float yOffset = Mathf.Lerp(0, bounceHeight, 1 - (1 - t) * (1 - t));
                titleText.rectTransform.anchoredPosition = basePos + new Vector3(0, yOffset, 0);
                yield return null;
            }

            // 다시 내려오기
            elapsed = 0f;
            while (elapsed < bounceDuration / 2)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (bounceDuration / 2);
                // EaseInQuad 모션
                float yOffset = Mathf.Lerp(bounceHeight, 0, t * t);
                titleText.rectTransform.anchoredPosition = basePos + new Vector3(0, yOffset, 0);
                yield return null;
            }
        }

        titleText.rectTransform.anchoredPosition = basePos;
    }

    IEnumerator FadeInButtons(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            foreach (var btn in buttons)
            {
                SetButtonAlpha(btn, alpha);
            }
            yield return null;
        }
        foreach (var btn in buttons)
        {
            SetButtonAlpha(btn, 1f);
        }
    }

    void SetButtonAlpha(GameObject btn, float alpha)
    {
        foreach (var img in btn.GetComponentsInChildren<Image>())
        {
            var c = img.color;
            c.a = alpha;
            img.color = c;
        }
        foreach (var tmp in btn.GetComponentsInChildren<TextMeshProUGUI>())
        {
            var c = tmp.color;
            c.a = alpha;
            tmp.color = c;
        }
    }

    void SetButtonInteractable(GameObject btn, bool interactable)
    {
        var btnComp = btn.GetComponent<UnityEngine.UI.Button>();
        if (btnComp != null)
        {
            btnComp.interactable = interactable;
        }
    }
}
