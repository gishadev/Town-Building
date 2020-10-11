using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class ObjectData : ScriptableObject
{
    [Header("Main")]
    public string Name;
    public GameObject Obj;
    public Sprite Sprite;
    [Header("Sizes")]
    public int xSize, zSize;


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
