using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextSelector : MonoBehaviour
{
    public Button textPrefab;
    public RectTransform startPoint;
    public float textSeparation = 25f;
    private GameManager gameManager;
    private TextDatabase textDatabase;
    private int textDatabaseLength;
    private Button[] mTexts;
    public RectTransform textHolder;
    private float textHolderMovement;
    public Scrollbar scrollbar;
    private float textHolderOriginal;

    void Awake()
    {
        gameManager = GameManager.gm;
        textDatabase = gameManager.GetComponent<TextDatabase>();
        textDatabaseLength = textDatabase.Texts.Count;
        if (textDatabaseLength == 0) return;
        mTexts = new Button[textDatabaseLength];
        float topY = 0;
        float bottomY = 0;
        for (int i = 0; i < textDatabaseLength; i++)
        {
            BaseText text = textDatabase.Texts[i];
            mTexts[i] = Instantiate(textPrefab);
            if (text.isUnlocked)
            {
                mTexts[i].interactable = true;
                mTexts[i].transform.FindChild("Lock").gameObject.SetActive(false);
            }
            mTexts[i].transform.FindChild("Text").GetComponent<Text>().text = text.title;
            RectTransform textRect = mTexts[i].GetComponent<RectTransform>();
            textRect.SetParent(textHolder.parent);
            float instantiatePos = startPoint.anchoredPosition.y - ((textRect.sizeDelta.y + textSeparation)*i);
            textRect.anchoredPosition = new Vector2(startPoint.anchoredPosition.x, instantiatePos);
            textRect.localScale = Vector3.one;
            if (i == 0)
            {
                topY = textRect.anchoredPosition.y + (textRect.sizeDelta.y/2);
            }
            if (i == textDatabaseLength - 1)
            {
                bottomY = textRect.anchoredPosition.y - (textRect.sizeDelta.y/2);
            }
            mTexts[i].onClick.AddListener(() => {LoadText(text); });
        }
        textHolder.sizeDelta = new Vector2(textHolder.sizeDelta.x, topY - bottomY + 40f);
        textHolderMovement = textHolder.sizeDelta.y - textHolder.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        foreach (var textB in mTexts)
        {
            textB.transform.SetParent(textHolder.transform);
        }
        textHolderOriginal = textHolder.anchoredPosition.y;
    }

    public void LoadText(BaseText targetText)
    {
        gameManager.textToLoad = targetText;
        gameManager.LoadLevel(Levels.textViewerScene);
    }

    void FixedUpdate()
    {
        float holderPosition = textHolderMovement*scrollbar.value;
        textHolder.anchoredPosition = new Vector2(textHolder.anchoredPosition.x, textHolderOriginal + holderPosition);
    }

    public void ReturnButton()
    {
        gameManager.LoadLevel(Levels.worldScene);
    }
}
