using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TextDatabase))]
public class TextDatabaseInspector : Editor
{
    private static int _index = 0;
    private bool mConfirmDelete = false;

    public static void SelectIndex(TextDatabase db, BaseText text)
    {
        _index = 0;
        foreach (BaseText t in db.Texts)
        {
            if (t == text) break;
            ++_index;
        }
    }

    public override void OnInspectorGUI()
    {
        TextDatabase db = target as TextDatabase;
        EditorGUILayout.Separator();

        BaseText text = null;

        if (db.Texts == null || db.Texts.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.Texts.Count - 1);
            text = db.Texts[_index];
        }

        if (mConfirmDelete)
        {
            GUILayout.Label("Deseja realmente excluir esse Texto?");
            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Cancelar"))
                {
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Deletar"))
                {
                    db.Texts.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Novo Texto"))
            {
                BaseText bt = new BaseText();
                bt.id16 = (db.Texts.Count > 0) ? db.Texts[db.Texts.Count - 1].id16 + 1 : 0;
                db.Texts.Add(bt);
                _index = db.Texts.Count - 1;

                bt.title = "Novo texto";
                text = bt;
            }
            GUI.backgroundColor = Color.white;

            if (text != null)
            {
                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    if (_index == 0) GUI.color = Color.grey;
                    if (GUILayout.Button("<<"))
                    {
                        mConfirmDelete = false;
                        --_index;
                    }
                    GUI.color = Color.white;
                    _index = EditorGUILayout.IntField(_index + 1, GUILayout.Width(40f)) - 1;
                    GUILayout.Label("/ " + db.Texts.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.Texts.Count) GUI.color = Color.grey;
                    if (GUILayout.Button(">>"))
                    {
                        mConfirmDelete = false;
                        ++_index;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    string textTitle = EditorGUILayout.TextField("Texto (ID " + _index + ")", text.title);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!textTitle.Equals(text.title))
                    {
                        text.title = textTitle;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                bool isUnlocked = EditorGUILayout.Toggle("Destravado", text.isUnlocked);
                if (isUnlocked != text.isUnlocked)
                {
                    text.isUnlocked = isUnlocked;
                }
                var content = EditorGUILayout.ObjectField("Conteudo", text.textContent, typeof (RectTransform), false);
                if (content != text.textContent)
                {
                    text.textContent = content as RectTransform;
                }
            }
        }
    }
}
