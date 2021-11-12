using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
	private GameManager gameManager;
	private WorldDatabase worldDatabase;
	public World worldPrefab;
	private World[] mWorlds;
	private Image[] mDots;
	public Image dotPrefab;
	public RectTransform dotsBar;
	public RectTransform worldsBar;
	public float dotsDistance = 25f;
	public Color activeDotColor = Color.white;
	public Color inactiveDotColor = Color.gray;
	public int activeWorld = 0;
	private bool mMoving;
	private int worldDatabaseLength;
    public AudioClip swipeClip;
    private AudioSource mAudioSource;
    public AudioClip lockedClip;
    public AudioClip unlockedClip;
    public Text coin;

	private Vector2 mWorldPivot;
	private float mSecondaryPivot;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;

    private float extraMove;

    public RectTransform layer1;
    public float parallaxFactor1;
    public RectTransform layer2;
    public float parallaxFactor2;
    public RectTransform layer3;
    public float parallaxFactor3;
    public RectTransform layer4;
    public float parallaxFactor4;

    void Awake()
	{
		gameManager = GameManager.gm;
        mAudioSource = GetComponent<AudioSource>();
		worldDatabase = gameManager.GetComponent<WorldDatabase>();
		worldDatabaseLength = worldDatabase.Worlds.Count;
		if (worldDatabaseLength == 0) return;
		mWorlds = new World[worldDatabaseLength];
		float dotWidth = dotPrefab.rectTransform.sizeDelta.x;
		float dotStartPos = -((dotWidth/2f)*(worldDatabaseLength - 1)) + (-dotsDistance*(worldDatabaseLength - 1))/2f;
		mDots = new Image[worldDatabaseLength];
		for (int i = 0; i < worldDatabaseLength; i++)
		{
			float instantiatePos = dotStartPos + (dotWidth * i) - (-dotsDistance * (i));
			mDots[i] = Instantiate(dotPrefab);
			mDots[i].rectTransform.SetParent(dotsBar);
			mDots[i].rectTransform.anchoredPosition = new Vector3(instantiatePos, 0f, 0f);
			mDots[i].rectTransform.localScale = Vector2.one;
			mDots[i].color = i == 0 ? activeDotColor : inactiveDotColor;
		}

		mSecondaryPivot = (worldsBar.rect.width/2) - (((worldsBar.rect.width / 2f) - (worldPrefab.GetComponent<RectTransform>().rect.width / 2f))/2f);
		Debug.Log(mSecondaryPivot);

		for (int a = 0; a < worldDatabaseLength; a++)
		{
			BaseWorld world = worldDatabase.Worlds[a];
			mWorlds[a] = Instantiate(worldPrefab);
			mWorlds[a].ChangeInfo(world.myAnim, world.name, world.levelCount);
            mWorlds[a].SetupAudio(world.isUnlocked ? unlockedClip : lockedClip, mAudioSource);
			mWorlds[a].isLocked = !world.isUnlocked;
			RectTransform worldRect = mWorlds[a].GetComponent<RectTransform>();
			worldRect.SetParent(worldsBar);
			worldRect.anchoredPosition = new Vector2(mSecondaryPivot * (a), 0f);
			worldRect.localScale = a == 0 ? Vector2.one : new Vector2(0.5f, 0.5f);
			worldRect.SetAsFirstSibling();
		}
	}

	public void MoveRight()
	{
        if (mMoving)
        {
            extraMove = -1f;
            return;
        }
        if (activeWorld == worldDatabaseLength -1) return;
		activeWorld++;
        mAudioSource.PlayOneShot(swipeClip);
        mMoving = true;
		for (int i = 0; i < worldDatabaseLength; i++)
		{
			RectTransform worldRect = mWorlds[i].GetComponent<RectTransform>();
			worldRect.DOLocalMoveX(worldRect.anchoredPosition.x - mSecondaryPivot, 0.7f).SetEase(Ease.InOutQuint);
            layer1.DOLocalMoveX(-parallaxFactor1, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer2.DOLocalMoveX(-parallaxFactor2, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer3.DOLocalMoveX(-parallaxFactor3, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer4.DOLocalMoveX(-parallaxFactor4, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            if (activeWorld == i) worldRect.SetAsLastSibling();
			worldRect.DOScale(activeWorld == i ? Vector3.one : new Vector3(0.5f, 0.5f, 0.5f), 0.7f).SetEase(Ease.InOutQuint).OnComplete(stopMoving);
			mDots[i].color = i == activeWorld ? activeDotColor : inactiveDotColor;
		}

	}

	public void ReturnButton()
	{
		gameManager.LoadLevel(Levels.loginScene);
	}

    public void TextButton()
    {
        gameManager.LoadLevel(Levels.textSelectorScene);
    }

    public void VideoButton()
    {
        gameManager.LoadLevel(Levels.videoSelectorScene);
    }

    public void StoreButton()
    {
        gameManager.LoadLevel(Levels.storeScene);
    }

    public void StatusButton()
    {
        gameManager.LoadLevel(Levels.statusScene);
    }

	public void MoveLeft()
	{
	    if (mMoving)
	    {
	        extraMove = 1f;
	        return;
	    }
		if (activeWorld == 0) return;
        mAudioSource.PlayOneShot(swipeClip);
		activeWorld--;
		mMoving = true;
		for (int i = 0; i < worldDatabaseLength; i++)
		{
			RectTransform worldRect = mWorlds[i].GetComponent<RectTransform>();
			worldRect.DOLocalMoveX(worldRect.anchoredPosition.x + mSecondaryPivot, 0.7f).SetEase(Ease.InOutQuint);
		    layer1.DOLocalMoveX(parallaxFactor1, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer2.DOLocalMoveX(parallaxFactor2, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer3.DOLocalMoveX(parallaxFactor3, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            layer4.DOLocalMoveX(parallaxFactor4, 0.7f).SetRelative(true).SetEase(Ease.InOutQuint);
            if (activeWorld == i) worldRect.SetAsLastSibling();
			worldRect.DOScale(activeWorld == i ? Vector3.one : new Vector3(0.5f, 0.5f, 0.5f), 0.7f).SetEase(Ease.InOutQuint).OnComplete(stopMoving);
			mDots[i].color = i == activeWorld ? activeDotColor : inactiveDotColor;
		}
	}

	public void stopMoving()
	{
		mMoving = false;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    coin.text = "" + gameManager.coinNumber;
        SwipeDetection();
	    if (extraMove > 0)
	    {
	        extraMove = 0;
            MoveLeft();
	    }
	    if (extraMove < 0)
	    {
	        extraMove = 0;
            MoveRight();
	    }

	    if (Input.GetKeyDown(KeyCode.RightArrow))
	    {
	        MoveRight();
	    }

	    if (Input.GetKeyDown(KeyCode.LeftArrow))
	    {
	        MoveLeft();
	    }
	}

    void SwipeDetection()
    {
        if (Input.touchCount > 0)
        {

            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        /* this is a new touch */
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        /* The touch is being canceled */
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    MoveLeft();
                                }
                                else
                                {
                                    MoveRight();
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    // MOVE UP
                                }
                                else
                                {
                                    // MOVE DOWN
                                }
                            }

                        }

                        break;
                }
            }
        }
    }
}
