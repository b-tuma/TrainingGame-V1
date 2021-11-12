using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class World : MonoBehaviour
{
    public GameManager mGameManager;
    public int databaseID;
    private BaseWorld myBaseWorld;
    public Sprite myWorld;
    private Sprite[] mySprites;
    public Text worldNameText;
    public Text levelsText;
    public GameObject lockImage;
    public Image copo;
    public GameObject unlockedStats;
    public Button worldButton;
    public string worldName;
    public int levelCount;
    public int levelsCompleted;
    public float inactiveWorldsScale = 0.5f;
    public Touch finger;
    public bool isLocked;
    public int animTimer;
    private AudioSource mAudioSource;
    private AudioClip mAudioClip;
    public Color lockedColor;


    void Awake()
    {
        mGameManager = GameManager.gm;
        myBaseWorld = mGameManager.worldDatabase.Worlds[databaseID];
        worldButton.GetComponent<Image>().sprite = myWorld;
        worldNameText.text = worldName;
        levelsText.text = levelsCompleted + "/" + levelCount;
        
    }

    public void SetupAudio(AudioClip clip, AudioSource source)
    {
        mAudioSource = source;
        mAudioClip = clip;
    }

    public void ChangeInfo(Sprite[] newSprite, string newName, int newLevelCount)
    {
        mySprites = newSprite;
        worldButton.GetComponent<Image>().sprite = newSprite[0];
        DOTween.To(() => animTimer, x => animTimer = x, 2, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        worldNameText.text = newName;
        levelsText.text = levelsCompleted + "/" + newLevelCount;
    }

	void Start ()
    {
	    if (isLocked)
	    {
	        worldButton.GetComponent<Image>().color = lockedColor;
	        //worldButton.interactable = false;
            unlockedStats.SetActive(false);
            lockImage.SetActive(true);
	    }
	    else
	    {
	        //worldButton.interactable = true;
            unlockedStats.SetActive(true);
            lockImage.SetActive(false);
	    }
	}

    public void OnClick()
    {
        if (!isLocked)
        {
            mGameManager.LoadLevel(myBaseWorld.sceneName);
        }
        
        mAudioSource.PlayOneShot(mAudioClip);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (isLocked) return;
        worldButton.GetComponent<Image>().sprite = mySprites[animTimer];

    }
}
