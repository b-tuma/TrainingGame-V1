using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VideoDatabase))]
public class VideoDatabaseInspector : Editor
{
    private static int _index = 0;
    private bool mConfirmDelete = false;

    public static void SelectIndex(VideoDatabase db, BaseVideo video)
    {
        _index = 0;
        foreach (BaseVideo v in db.Videos)
        {
            if (v == video) break;
            ++_index;
        }
    }

    public override void OnInspectorGUI()
    {
        VideoDatabase db = target as VideoDatabase;
        EditorGUILayout.Separator();

        BaseVideo video = null;

        if (db.Videos == null || db.Videos.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.Videos.Count - 1);
            video = db.Videos[_index];
        }

        if (mConfirmDelete)
        {
            GUILayout.Label("Deseja realmente excluir esse Video?");
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
                    db.Videos.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Novo Video"))
            {
                BaseVideo bv = new BaseVideo();
                bv.id16 = (db.Videos.Count > 0) ? db.Videos[db.Videos.Count - 1].id16 + 1 : 0;
                db.Videos.Add(bv);
                _index = db.Videos.Count - 1;

                bv.title = "Novo video";
                video = bv;
            }
            GUI.backgroundColor = Color.white;

            if (video != null)
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
                    GUILayout.Label("/ " + db.Videos.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.Videos.Count) GUI.color = Color.grey;
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
                    string videoTitle = EditorGUILayout.TextField("Video (ID " + _index + ")", video.title);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!videoTitle.Equals(video.title))
                    {
                        video.title = videoTitle;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                bool isUnlocked = EditorGUILayout.Toggle("Destravado", video.isUnlocked);
                if (isUnlocked != video.isUnlocked)
                {
                    video.isUnlocked = isUnlocked;
                }
                string url = EditorGUILayout.TextField("URL", video.url);
                if (url != video.url)
                {
                    video.url = url;
                }
            }
        }
    }
}
