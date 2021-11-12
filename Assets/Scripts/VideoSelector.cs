using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VideoSelector : MonoBehaviour
{
    public Button videoPrefab;
    public RectTransform startPoint;
    public float videoSeparation = 25f;
    private GameManager mGameManager;
    private VideoDatabase videoDatabase;
    private int videoDatabaseLength;
    private Button[] mVideos;
    public RectTransform videoHolder;
    private float videoHolderMovement;
    public Scrollbar scrollbar;
    private float videoHolderOriginal;

    void Awake()
    {
        mGameManager = GameManager.gm;
        videoDatabase = mGameManager.GetComponent<VideoDatabase>();
        videoDatabaseLength = videoDatabase.Videos.Count;
        if (videoDatabaseLength == 0) return;
        mVideos = new Button[videoDatabaseLength];
        float topY = 0;
        float bottomY = 0;
        for (int i = 0; i < videoDatabaseLength; i++)
        {
            BaseVideo video = videoDatabase.Videos[i];
            mVideos[i] = Instantiate(videoPrefab);
            if (video.isUnlocked)
            {
                mVideos[i].interactable = true;
                mVideos[i].transform.FindChild("Lock").gameObject.SetActive(false);
            }
            mVideos[i].transform.FindChild("Text").GetComponent<Text>().text = video.title;
            RectTransform videoRect = mVideos[i].GetComponent<RectTransform>();
            videoRect.SetParent(videoHolder.parent);
            float instantiatePos = startPoint.anchoredPosition.y - ((videoRect.sizeDelta.y + videoSeparation)*i);
            videoRect.anchoredPosition = new Vector2(startPoint.anchoredPosition.x, instantiatePos);
            videoRect.localScale = Vector3.one;
            if (i == 0)
            {
                topY = videoRect.anchoredPosition.y + (videoRect.sizeDelta.y/2);
            }
            if (i == videoDatabaseLength - 1)
            {
                bottomY = videoRect.anchoredPosition.y - (videoRect.sizeDelta.y/2);
            }
            mVideos[i].onClick.AddListener(() => {LoadVideo(video);});
        }
        videoHolder.sizeDelta = new Vector2(videoHolder.sizeDelta.x, topY - bottomY + 40f);
        videoHolderMovement = videoHolder.sizeDelta.y - videoHolder.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        foreach (var videoB in mVideos)
        {
            videoB.transform.SetParent(videoHolder.transform);
        }
        videoHolderOriginal = videoHolder.anchoredPosition.y;
    }

    void FixedUpdate()
    {
        float holderPosition = videoHolderMovement*scrollbar.value;
        videoHolder.anchoredPosition = new Vector2(videoHolder.anchoredPosition.x, videoHolderOriginal + holderPosition);
    }

    public void LoadVideo(BaseVideo targetVideo)
    {
        Application.OpenURL(targetVideo.url);
        //Handheld.PlayFullScreenMovie("Nova.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }

    public void ReturnButton()
    {
        mGameManager.LoadLevel(Levels.worldScene);
    }
}
