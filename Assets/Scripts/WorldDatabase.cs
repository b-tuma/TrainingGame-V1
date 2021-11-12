using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WorldDatabase : MonoBehaviour
{
    private static WorldDatabase[] _list;
    private static bool _isDirty = true;

    static public WorldDatabase[] list
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
               // _list = Tools.FindActive<WorldDatabase>();
            }
            return _list;
        }
    }

    public int DatabaseID = 0;
    public List<BaseWorld> Worlds = new List<BaseWorld>();

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
public class BaseWorld
{
    public int id16;
    public string name;
    public Sprite myImage;
    public Sprite[] myAnim;
    public int levelCount;
    public string sceneName;
    public bool isUnlocked;
}