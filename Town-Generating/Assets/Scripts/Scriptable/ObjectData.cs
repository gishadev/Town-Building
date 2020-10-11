using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class ObjectData : ScriptableObject
{
    public string Name;
    public GameObject Obj;
    public Sprite Sprite;

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
