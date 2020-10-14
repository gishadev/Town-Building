using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class ObjectData : ScriptableObject
{
    [Header("Main")]
    public string Name;
    public GameObject Obj;
    public Sprite Sprite;
    [Header("Dimensions")]
    public Vector2Int Dimensions; 
    //public int xDimension;
    //public int zDimension;

    public MeshRenderer MeshRenderer
    {
        get
        {
            return Obj.GetComponent<MeshRenderer>();
        }
    }

    public MeshFilter MeshFilter
    {
        get
        {
            return Obj.GetComponent<MeshFilter>();
        }
    }
}
