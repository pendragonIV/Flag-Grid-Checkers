using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform drawPanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Transform[] flashObjs;
    [SerializeField]
    private Text winPlayer;
    [SerializeField]
    private Text losePlayer;
    [SerializeField]
    private Transform[] slideIn;
    [SerializeField]
    private Transform playerTurn;


    private void Start()
    {
        overlayPanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        drawPanel.gameObject.SetActive(false);

        foreach (Transform flash in flashObjs)
        {
            StartCoroutine(FlashAnim(flash));
        }
    }

    private IEnumerator FlashAnim(Transform flash)
    {
        while (true)
        {
            flash.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, 0, 0);
            flash.GetComponent<RectTransform>().DOAnchorPos(new Vector2(100, 0), 1.5f, false).SetEase(Ease.OutQuint).SetUpdate(true);
            yield return new WaitForSecondsRealtime(3f);
        }
    }

    private void SlideIn()
    {
        foreach (Transform slide in slideIn)
        {
            Vector3 pos = slide.GetComponent<RectTransform>().anchoredPosition;
            slide.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x - 500, pos.y, pos.z);
            slide.GetComponent<RectTransform>().DOAnchorPos(pos, .5f, false).SetEase(Ease.OutQuint).SetUpdate(true);
            slide.GetComponent<CanvasGroup>().alpha = 0f;
            slide.GetComponent<CanvasGroup>().DOFade(1, .5f).SetUpdate(true);
        }
    }

    public void ShowWinPanel(int win)
    {
        if (win == 1)
        {
            winPlayer.text = "P1";
            losePlayer.text = "P2";
        }
        else
        {
            winPlayer.text = "P2";
            losePlayer.text = "P1";
        }
        winPanel.gameObject.SetActive(true);
        overlayPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        SlideIn();
    }

    public void ShowDrawPanel()
    {
        drawPanel.gameObject.SetActive(true);
        overlayPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), drawPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        SlideIn();
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void ChangeTurn(int player)
    {
        if(player == 1)
        {
            playerTurn.GetChild(0).gameObject.SetActive(true);
            playerTurn.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            playerTurn.GetChild(0).gameObject.SetActive(false);
            playerTurn.GetChild(1).gameObject.SetActive(true);
        }

    }
}
