using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextViewer : MonoBehaviour
{
    private GameManager mGameManager;
    public Text title;
    public RectTransform contentPrefab;
    private RectTransform content;
    public RectTransform holder;
    public Scrollbar scrollbar;
    private float contentMovement;
    private float contentOriginalPos;

    void Awake()
    {
        mGameManager = GameManager.gm;
        if (mGameManager.textToLoad == null)
        {
            mGameManager.LoadLevel(Levels.worldScene);
            return;
        }

        BaseText newText = mGameManager.textToLoad;
        mGameManager.textToLoad = null;
        title.text = newText.title;
        contentPrefab = newText.textContent;

        content = Instantiate(contentPrefab);
        content.SetParent(holder);
        content.localScale = Vector3.one;
        content.anchoredPosition = holder.anchoredPosition;
        contentMovement = content.sizeDelta.y - holder.sizeDelta.y;
        contentOriginalPos = content.anchoredPosition.y;
    }

    void FixedUpdate()
    {
        float contentPosition = contentMovement*scrollbar.value;
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, contentOriginalPos + contentPosition);
    }

    public void ReturnButton()
    {
        mGameManager.LoadLevel(mGameManager.lastScene);
    }
}
