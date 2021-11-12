using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class StoreDatabase : MonoBehaviour
{
    private static StoreDatabase[] _list;
    private static bool _isDirty = true;

    static public StoreDatabase[] list
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                //_list = Tools.FindActive<StoreDatabase>();
            }
            return _list;
        }
    }

    public int DatabaseID = 0;
    public List<BaseProduct> products = new List<BaseProduct>();

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
public class BaseProduct
{
    public int id16;
    public string name;
    public int price;
    public string description;
    public Sprite image;
}
