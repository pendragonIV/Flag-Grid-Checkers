using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform[] flashObjs;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);
        SetupGameLogo();
        foreach (Transform flash in flashObjs)
        {
            StartCoroutine(FlashAnim(flash));
        }
    }

    private void SetupGameLogo()
    {
        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 1.5f).SetUpdate(true);

        gameLogo.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        gameLogo.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 1.5f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
        {
            gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-20, 100), 5f, false).SetUpdate(true).OnComplete(() =>
            {
                gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(20, 100), 5f, false).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
            });
        });
    }

    private IEnumerator FlashAnim(Transform flash)
    {
        while (true)
        {
            flash.GetComponent<RectTransform>().anchoredPosition = new Vector3(-170, 0, 0);
            flash.GetComponent<RectTransform>().DOAnchorPos(new Vector2(170, 0), 1.5f, false).SetEase(Ease.OutQuint).SetUpdate(true);
            yield return new WaitForSecondsRealtime(3f);
        }
    }

    public void ShowTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());

    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));

    }   

    private void FadeIn(CanvasGroup canvasGroup ,RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

}
