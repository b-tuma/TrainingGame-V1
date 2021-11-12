using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VideoDatabase : MonoBehaviour
{
    private static VideoDatabase[] _list;
    private static bool _isDirty = true;

    static public VideoDatabase[] list
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                //_list = Tools.FindActive<VideoDatabase>();
            }
            return _list;
        }
    }

    public int DatabaseID = 0;
    public List<BaseVideo> Videos = new List<BaseVideo>(); 

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
public class BaseVideo
{
    public int id16;
    public string title;
    public bool isUnlocked;
    public string url;
}