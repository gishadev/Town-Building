using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectData))]
public class ObjectDataEditor : Editor
{
    ObjectData data;

    GUIStyle header;
    GUIStyle field;
    GUIStyle enterField;

    void OnEnable()
    {
        #region Styles
        header = new GUIStyle();
        header.fixedHeight = 25f;
        header.alignment = TextAnchor.MiddleCenter;
        header.normal.textColor = Color.black;
        header.fontStyle = FontStyle.Bold;
        header.fontSize = 14;

        field = new GUIStyle();
        field.fixedHeight = 23f;
        #endregion

    }

    public override void OnInspectorGUI()
    {
        data = (ObjectData)target;

        #region Main
        GUILayout.BeginVertical("Box");

        GUILayout.Label("Main", header);

        GUILayout.BeginHorizontal(field);
        GUILayout.Label("Name", GUILayout.Width(EditorGUIUtility.labelWidth));
        data.Name = EditorGUILayout.TextField(data.Name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(field);
        GUILayout.Label("Object Type", GUILayout.Width(EditorGUIUtility.labelWidth));
        data.ObjType = (ObjectType)EditorGUILayout.EnumPopup(data.ObjType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(field);
        GUILayout.Label("Object Prefab", GUILayout.Width(EditorGUIUtility.labelWidth));
        data.Obj = (GameObject)EditorGUILayout.ObjectField(data.Obj, typeof(GameObject), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(field);
        GUILayout.Label("Object Sprite", GUILayout.Width(EditorGUIUtility.labelWidth));
        data.Sprite = (Sprite)EditorGUILayout.ObjectField(data.Sprite, typeof(Sprite), false);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        #endregion

        #region Palettes
        GUILayout.BeginVertical("Box");

        GUILayout.Label("Palettes", header);

        GUILayout.BeginHorizontal(field);
        GUILayout.Label("Is Static Palette", GUILayout.Width(EditorGUIUtility.labelWidth));
        data.IsStaticPalette = EditorGUILayout.Toggle(data.IsStaticPalette);
        GUILayout.EndHorizontal();

        if (!data.IsStaticPalette)
        {
            if (GUILayout.Button("Restore default palette"))
                data.RestoreDefaultPalette();

            serializedObject.Update();
            SerializedProperty p = serializedObject.FindProperty("palettes");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(p, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        GUILayout.EndVertical();


        #endregion
    }

}
