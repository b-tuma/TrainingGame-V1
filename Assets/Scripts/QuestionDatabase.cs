using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class QuestionDatabase : MonoBehaviour
{
    private static QuestionDatabase[] _list;
    private static bool _isDirty = true;


    static public QuestionDatabase[] list
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                //_list = Tools.FindActive<QuestionDatabase>();
            }
            return _list;
        }
    }

    public int DatabaseID = 0; 
    public List<BaseQuestion> Questions = new List<BaseQuestion>();

    void OnEnable()
    {
        _isDirty = true;
    }

    void OnDisable()
    {
        _isDirty = false;
    }
}

[System.Serializable]
public class BaseQuestion
{
    public int id16;
    public string question;
    public bool isUnlocked;
    public string[] alternative = new string[5];
    public int correctAlternative;
    public int maxScore;
    public int minScore;
    public float time;
    public bool completed;
}