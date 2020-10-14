using UnityEditor;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor
{
    WorldManager worldManager;

    public SerializedProperty xSize;
    public SerializedProperty zSize;

    void OnEnable()
    {
        xSize = serializedObject.FindProperty("xSize");
        zSize = serializedObject.FindProperty("zSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(xSize);
        EditorGUILayout.PropertyField(zSize);

        worldManager = (WorldManager)target;

        if (worldManager.xSize != xSize.intValue || worldManager.zSize != zSize.intValue)
            worldManager.CreateWorld(xSize.intValue, zSize.intValue);

        serializedObject.ApplyModifiedProperties();
    }
}
