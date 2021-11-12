using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private GameManager mGameManager;
    public RectTransform levelRect;
    public float startPos;
    public float endPos;
    public float walkDistance;
    private float touchStart;
    private float touchPos;

    public Text coinNum;

    private void Awake()
    {
        mGameManager = GameManager.gm;
        levelRect.anchoredPosition = new Vector2(startPos, levelRect.anchoredPosition.y);
        walkDistance = startPos - endPos;
    }

    void Update()
    {
        coinNum.text = mGameManager.coinNumber.ToString();
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                touchStart = Input.touches[0].position.x;
                touchPos = levelRect.anchoredPosition.x;
                
            }
            Debug.Log(touchStart - Input.touches[0].position.x);
            levelRect.anchoredPosition = new Vector2(touchPos - (touchStart - Input.touches[0].position.x), levelRect.anchoredPosition.y);

        }

        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition.x;
            touchPos = levelRect.anchoredPosition.x;
        }
        if (Input.GetMouseButton(0))
        {
            levelRect.anchoredPosition = new Vector2(touchPos - (touchStart - Input.mousePosition.x), levelRect.anchoredPosition.y);
        }
        if (levelRect.anchoredPosition.x < endPos)
        {
            levelRect.anchoredPosition = new Vector2(endPos, levelRect.anchoredPosition.y);
        }
        if (levelRect.anchoredPosition.x > startPos)
        {
            levelRect.anchoredPosition = new Vector2(startPos, levelRect.anchoredPosition.y);
        }
    }

    public void ReturnButton()
    {
        mGameManager.LoadLevel(Levels.worldScene);
    }

    public void VideoButton()
    {
        mGameManager.LoadLevel(Levels.videoSelectorScene);
    }

    public void TextButton()
    {
        mGameManager.LoadLevel(Levels.textSelectorScene);
    }

    public void StoreButton()
    {
        mGameManager.LoadLevel(Levels.storeScene);
    }

}
