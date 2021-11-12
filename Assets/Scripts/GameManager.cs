using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public const bool ShowMode = true;
    public LoadingScreen loadingScreen;
    public static GameManager gm;
    public string databaseUrl = "http://localhost:8080/";
    public QuestionDatabase questionDatabase;
    public WorldDatabase worldDatabase;
    public TextDatabase textDatabase;
    public VideoDatabase videoDatabase;

    public BaseQuestion questionToLoad;
    public BaseWorld worldToLoad;
    public BaseText textToLoad;
    private bool isLoading;
    public string lastScene;
    public bool hasShowMessage;

    //Migue
    public int coinNumber;

    void Awake()
    {
        if (gm != null)
        {
            GameObject.Destroy(gm);
        }
        else
        {
            gm = this;
        }
        questionDatabase = GetComponent<QuestionDatabase>();
        worldDatabase = GetComponent<WorldDatabase>();
        textDatabase = GetComponent<TextDatabase>();
        videoDatabase = GetComponent<VideoDatabase>();
        GameObject[] allObjects = FindObjectsOfType(typeof (GameObject)) as GameObject[];
        if (allObjects != null)
        {
            foreach (GameObject myObject in allObjects)
            {
                if (myObject.layer == Layers.BaseLayer)
                {
                    DontDestroyOnLoad(myObject);
                }
            }
        }
        
    }

    public void LoadLevel(string levelToLoad, bool skipLoadingScreen = false)
    {
        if (isLoading) return;
        if (string.IsNullOrEmpty(levelToLoad)) return;
        isLoading = true;
        lastScene = Application.loadedLevelName;
        StartCoroutine(LoadLevelRoutine(levelToLoad, skipLoadingScreen));
    }

    IEnumerator LoadLevelRoutine(string level, bool skip)
    {
        

        Debug.Log("GameManager: Loading Level[" + level + "]...");

        if (!skip)
        {
            loadingScreen.OpenLoadingScreen();
            yield return new WaitForSeconds(loadingScreen.fadeSpeed);
        }
        AsyncOperation async = Application.LoadLevelAsync(level);
        yield return async;
        if (!skip)
        {
            loadingScreen.CloseLoadingScreen();
        }
        isLoading = false;

        Debug.Log("GameManager: Level[" + level + "] loaded.");
    }

    void Start()
    {
        LoadLevel(Levels.loginScene);
    }
}
