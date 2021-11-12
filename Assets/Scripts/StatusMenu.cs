using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusMenu : MonoBehaviour
{
    private GameManager mGameManager;
    public Text money;
    public Scrollbar scrollbar;
    public RectTransform statusHolder;
    private float originalPosition;
    public float maxMovement;
    private float mMovementExtension;

    void Awake()
    {
        mGameManager = GameManager.gm;
        money.text = mGameManager.coinNumber.ToString();
        originalPosition = statusHolder.anchoredPosition.y;
        mMovementExtension = originalPosition - maxMovement;
    }

    void FixedUpdate()
    {
        float holderPosition = mMovementExtension*scrollbar.value;
        statusHolder.anchoredPosition = new Vector2(statusHolder.anchoredPosition.x, originalPosition + holderPosition);
    }

    public void ReturnButton()
    {
        mGameManager.LoadLevel(mGameManager.lastScene);
    }
}
