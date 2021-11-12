using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Canvas loadingCanvas;
    public Image copoPlacement;
    public Image background;
    public Sprite[] copoArray;
    private int mCopoIndex;
    private int mCurrentIndex;
    private bool isLoading;
    public float fadeSpeed = 0.5f;
    public float animSpeed = 2f;

    void Awake()
    {
        DOTween.To(() => mCopoIndex, x => mCopoIndex = x, 8, animSpeed).SetEase(Ease.Linear).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        if (mCopoIndex == mCurrentIndex) return;

        mCurrentIndex = mCopoIndex;
        copoPlacement.sprite = copoArray[mCopoIndex];
    }

    public void OpenLoadingScreen()
    {
        if (!isLoading) StartCoroutine(OpenLoadScreen());
    }

    public void CloseLoadingScreen()
    {
        if (isLoading) StartCoroutine(CloseLoadScreen());
    }

    IEnumerator OpenLoadScreen()
    {
        loadingCanvas.gameObject.SetActive(true);
        isLoading = true;
        copoPlacement.DOFade(1f, fadeSpeed).SetEase(Ease.InOutCubic).SetUpdate(true);
        background.DOFade(1f, fadeSpeed).SetEase(Ease.InOutCubic).SetUpdate(true);
        yield return new WaitForSeconds(fadeSpeed);
    }

    IEnumerator CloseLoadScreen()
    {
        copoPlacement.DOFade(0, fadeSpeed).SetEase(Ease.InOutCubic).SetUpdate(true);
        background.DOFade(0, fadeSpeed).SetEase(Ease.InOutCubic).SetUpdate(true);
        yield return new WaitForSeconds(fadeSpeed);
        loadingCanvas.gameObject.SetActive(false);
        isLoading = false;
    }
}
