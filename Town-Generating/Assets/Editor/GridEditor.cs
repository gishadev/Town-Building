using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    WorldBuilder worldBuilder;
    Grid grid;

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

        worldBuilder = WorldBuilder.Instance;
        grid = (Grid)target;

        if (grid.xSize != xSize.intValue || grid.zSize != zSize.intValue)
            worldBuilder.CreateWorld(xSize.intValue, zSize.intValue);

        serializedObject.ApplyModifiedProperties();
    }
}
