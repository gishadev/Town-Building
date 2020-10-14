using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class ObjectData : ScriptableObject
{
    [Header("Main")]
    public string Name;
    public GameObject Obj;
    public Sprite Sprite;

    public Vector2Int Dimensions
    {
        get
        {
            int x = Mathf.RoundToInt((MeshFilter.sharedMesh.bounds.max.x - MeshFilter.sharedMesh.bounds.min.x) * Obj.transform.localScale.x);
            int z = Mathf.RoundToInt((MeshFilter.sharedMesh.bounds.max.z - MeshFilter.sharedMesh.bounds.min.z) * Obj.transform.localScale.z);

            return new Vector2Int(x, z);
        }
    }

    public MeshRenderer MeshRenderer
    {
        get => Obj.GetComponent<MeshRenderer>();
    }

    public MeshFilter MeshFilter
    {
        get => Obj.GetComponent<MeshFilter>();
    }
}
