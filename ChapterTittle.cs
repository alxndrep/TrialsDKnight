using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChapterTittle : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup canvasGroup;
    [SerializeField] private bool isNewChapter;
    [SerializeField] private float Tiempo;
    [SerializeField] private TextMeshProUGUI Chapter;
    [SerializeField] private string ChapterTitle;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField][TextArea] private string DescriptionText;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(isNewChapter) Invoke("ShowChapterTitle", Tiempo);
    }

    private void ShowChapterTitle()
    {
        Chapter.text = ChapterTitle;
        Description.text = DescriptionText;
        LeanTween.alphaCanvas(canvasGroup, 1f, 2f).setOnComplete(FadeOut).setIgnoreTimeScale(true);
    }
    private void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 2f).setDelay(3f).setIgnoreTimeScale(true);
    }

}
