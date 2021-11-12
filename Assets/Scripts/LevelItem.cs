using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class LevelItem : MonoBehaviour
{
    public enum LevelType
    {
        Text,
        Video,
        Quiz
    }

    private GameManager mGameManager;
    private Button myButton;
    private GameObject myLock;
    public string levelNumber;
    public LevelType myLevelType = LevelType.Text;
    public int databaseID;
    private bool isUnlocked;
    private bool isCompleted;
    private BaseText myText;
    private BaseQuestion myQuestion;
    private BaseVideo myVideo;
    public Sprite incompleteSprite;
    public GameObject message;
    public AudioClip unlockedClip;
    public AudioClip lockedClip;
    public AudioSource audioSource;

    private bool isValid = false;

    void Awake()
    {
        mGameManager = GameManager.gm;
        myButton = GetComponentInChildren<Button>();
        Text textNum = GetComponentInChildren<Text>();
        if (textNum != null)
        {
            textNum.text = levelNumber;
        }
        myLock = transform.FindChild("Lock").gameObject;
        switch (myLevelType)
        {
                case LevelType.Quiz:
                if (mGameManager.questionDatabase.Questions.Count >= databaseID - 1)
                {
                    isValid = true;
                    myQuestion = mGameManager.questionDatabase.Questions[databaseID];
                    isUnlocked = myQuestion.isUnlocked;
                    isCompleted = myQuestion.completed;
                }
                break;
                case LevelType.Text:
                if (mGameManager.textDatabase.Texts.Count >= databaseID - 1)
                {
                    isValid = true;
                    myText = mGameManager.textDatabase.Texts[databaseID];
                    isUnlocked = myText.isUnlocked;
                }
                break;
                case LevelType.Video:
                if (mGameManager.videoDatabase.Videos.Count >= databaseID - 1)
                {
                    isValid = true;
                    myVideo = mGameManager.videoDatabase.Videos[databaseID];
                    isUnlocked = myVideo.isUnlocked;
                }
                break;
        }

        if (isUnlocked)
        {
            myLock.SetActive(false);
            myButton.interactable = true;

        }
        if (myLevelType == LevelType.Quiz)
        {
            if (isUnlocked)
            {
                myLock.SetActive(false);
                if (isCompleted)
                {
                    //myButton.interactable = false;
                }
                else
                {
                    myButton.interactable = true;
                    myButton.GetComponent<Image>().sprite = incompleteSprite;
                    GetComponent<Image>().enabled = false;
                }
            }
            else
            {
                myLock.SetActive(true);
                if (isCompleted)
                {
                    //myButton.interactable = false;
                }
                else
                {
                    myButton.interactable = false;
                    myButton.GetComponent<Image>().sprite = incompleteSprite;
                    GetComponent<Image>().enabled = false;
                }
            }
        }
        myButton.interactable = true;
    }

    public void OnClick()
    {
        if (isUnlocked)
        {
            audioSource.PlayOneShot(unlockedClip);
            switch (myLevelType)
            {
                case LevelType.Quiz:
                    if (myQuestion.completed)
                    {
                        return;
                    }
                    mGameManager.questionToLoad = myQuestion;
                    mGameManager.LoadLevel(Levels.questionScene);
                    break;
                case LevelType.Text:
                    mGameManager.textToLoad = myText;
                    mGameManager.LoadLevel(Levels.textViewerScene);
                    break;
                case LevelType.Video:
                    //Application.OpenURL(myVideo.url);
                    Handheld.PlayFullScreenMovie("Nova.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
                    if (!mGameManager.hasShowMessage)
                    {
                        message.SetActive(true);
                        mGameManager.hasShowMessage = true;
                    }
                    break;
            }
        }
        
        if (!isUnlocked)
        {
            audioSource.PlayOneShot(lockedClip);
        }
    }
}
