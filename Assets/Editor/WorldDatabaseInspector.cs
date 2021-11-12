using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WorldDatabase))]
public class WorldDatabaseInspector : Editor
{
    private static int _index = 0;
    private bool mConfirmDelete = false;

    public static void SelectIndex(WorldDatabase db, BaseWorld world)
    {
        _index = 0;
        foreach (BaseWorld w in db.Worlds)
        {
            if (w == world) break;
            ++_index;
        }
    }

    public override void OnInspectorGUI()
    {
        WorldDatabase db = target as WorldDatabase;
        EditorGUILayout.Separator();

        BaseWorld world = null;

        if (db.Worlds == null || db.Worlds.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.Worlds.Count - 1);
            world = db.Worlds[_index];
        }

        if (mConfirmDelete)
        {
            GUILayout.Label("Deseja realmente excluir esse mundo?");
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
                    db.Worlds.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Novo Mundo"))
            {
                BaseWorld bw = new BaseWorld();
                bw.id16 = (db.Worlds.Count > 0) ? db.Worlds[db.Worlds.Count - 1].id16 + 1 : 0;
                db.Worlds.Add(bw);
                _index = db.Worlds.Count - 1;

                bw.name = "Novo mundo";
                bw.myAnim = new Sprite[3];
                world = bw;
            }
            GUI.backgroundColor = Color.white;

            if (world != null)
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
                    GUILayout.Label("/ " + db.Worlds.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.Worlds.Count) GUI.color = Color.grey;
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
                    string worldName = EditorGUILayout.TextField("Mundo (ID " + _index + ")", world.name);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!worldName.Equals(world.name))
                    {
                        world.name = worldName;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                Sprite[] animArray = new Sprite[3];
                for (int a = 0; a < animArray.Length; a++)
                {
                    animArray[a] = EditorGUILayout.ObjectField("Sprite " + a, world.myAnim[a], typeof (Sprite), false) as Sprite;
                    if (animArray[a] != world.myAnim[a])
                    {
                        world.myAnim[a] = animArray[a];
                    }
                }
                var image = EditorGUILayout.ObjectField("Imagem", world.myImage, typeof(Sprite), false);
                if (image != world.myImage)
                {
                    world.myImage = image as Sprite;
                }
                int levelCount = EditorGUILayout.IntField("Número de niveis", world.levelCount);
                if (levelCount != world.levelCount)
                {
                    world.levelCount = levelCount;
                }
                string sceneName = EditorGUILayout.TextField("Scene", world.sceneName);
                if (sceneName != world.sceneName)
                {
                    world.sceneName = sceneName;
                }

                bool isUnlocked = EditorGUILayout.Toggle("Destravado", world.isUnlocked);
                if (isUnlocked != world.isUnlocked)
                {
                    world.isUnlocked = isUnlocked;
                }
            }
        }
    }
}
