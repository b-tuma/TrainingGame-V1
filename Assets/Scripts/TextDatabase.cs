using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextDatabase : MonoBehaviour
{
    private static TextDatabase[] _list;
    private static bool _isDirty = true;

    static public TextDatabase[] list
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                //_list = Tools.FindActive<TextDatabase>();
            }
            return _list;
        }
    }

    public int DatabaseID = 0;
    public List<BaseText> Texts = new List<BaseText>();

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
public class BaseText
{
    public int id16;
    public string title;
    public bool isUnlocked;
    public RectTransform textContent;
}