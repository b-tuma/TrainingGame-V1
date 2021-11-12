using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class QuestionScreen : MonoBehaviour
{
    private GameManager mGameManager;
    public Sprite[] wrongSprites;
    public Sprite[] correctSprites;
    public Sprite wrong;
    public Sprite correct;
    public Toggle questionPrefab;
    public Text quizTitle;
    public Button[] alternatives;
    public RectTransform quizBackground;
    public float minHeight;
    public float maxHeight;
    public RectTransform bar;
    public BaseQuestion myQuestion;
    public int scoreLeft = 9999;
    private float mScoreDif;
    private float mTimer;
    private bool countdownStarted;
    public Text coins;
    private float mHeightDif;
    private bool finished;
    public Image feedback;
    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;
    public float timeBeforeStart = 10f;
    public AudioClip countdownClip;

    void Awake()
    {
        mGameManager = GameManager.gm;
        myQuestion = mGameManager.questionToLoad;
        mGameManager.questionToLoad = null;

        quizTitle.text = myQuestion.question;
        for (int i = 0; i < alternatives.Length; i++)
        {
            alternatives[i].GetComponentInChildren<Text>().text = myQuestion.alternative[i];
            if (myQuestion.correctAlternative == i)
            {
                alternatives[i].onClick.AddListener(CorrectAnswer);
            }
            else
            {
                alternatives[i].onClick.AddListener(WrongAnswer);
            }
        }
        mScoreDif = myQuestion.maxScore - myQuestion.minScore;
        mHeightDif = maxHeight - minHeight;
        StartCoroutine(CoolTimer());
    }

    IEnumerator CoolTimer()
    {
        bar.DOPunchPosition(new Vector3(3, 0, 0), timeBeforeStart, 15, 2f);
        yield return new WaitForSeconds(timeBeforeStart);
        yield return null;
        mTimer = 0;
        countdownStarted = true;
        audioSource.PlayOneShot(countdownClip);
        
    }

    void FixedUpdate()
    {
        if (finished) return;
        
        if (myQuestion.maxScore == myQuestion.minScore)
        {
            bar.sizeDelta = new Vector2(bar.sizeDelta.x, minHeight);
            coins.text = "" + myQuestion.minScore;
            return;
        }
        if (!countdownStarted) return;
        mTimer += Time.fixedDeltaTime;
        scoreLeft = Mathf.CeilToInt(myQuestion.maxScore - ((mScoreDif / myQuestion.time) * mTimer));
        bar.sizeDelta = new Vector2(bar.sizeDelta.x, maxHeight -((mHeightDif / myQuestion.time * mTimer)));
        if (scoreLeft < myQuestion.minScore)
        {
            countdownStarted = false;
            audioSource.Stop();
            scoreLeft = myQuestion.minScore;
        }
        if (bar.sizeDelta.y < minHeight)
        {
            bar.sizeDelta = new Vector2(bar.sizeDelta.x, minHeight);
        }
        coins.text = "" + scoreLeft;
    }

    public void CorrectAnswer()
    {
        if (!finished)
        {
            audioSource.Stop();
            StartCoroutine(feedbackImage(correctSprites));
            if (myQuestion.maxScore == myQuestion.minScore)
            {
                mGameManager.coinNumber += myQuestion.minScore;
            }
            else
            {
                mGameManager.coinNumber += scoreLeft;
            }
            
            myQuestion.completed = true;
            mGameManager.questionDatabase.Questions[myQuestion.id16 + 1].isUnlocked = true;
            audioSource.PlayOneShot(winClip);
        }
        finished = true;
    }

    public void WrongAnswer()
    {
        if (!finished)
        {
            audioSource.Stop();
            StartCoroutine(feedbackImage(wrongSprites));
            myQuestion.maxScore = myQuestion.minScore;
            audioSource.PlayOneShot(loseClip);
        }
        finished = true;
    }

    private int animTimer;

    IEnumerator feedbackImage(Sprite[] mySprite)
    {
        feedback.gameObject.SetActive(true);
        for (int i = 0; i < mySprite.Length; i++)
        {
            feedback.sprite = mySprite[i];
            yield return new WaitForSeconds(0.4f);
        }
        for (int i = 0; i < mySprite.Length; i++)
        {
            feedback.sprite = mySprite[i];
            yield return new WaitForSeconds(0.4f);
        }
        mGameManager.LoadLevel(mGameManager.lastScene);
    }

    public void ReturnButton()
    {
        if (finished) return;
        mGameManager.LoadLevel(mGameManager.lastScene);
    }
}
