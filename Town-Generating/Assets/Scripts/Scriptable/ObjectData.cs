using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class ObjectData : ScriptableObject
{
    #region PUBLIC_FIELDS
    [Header("Main")]
    public string Name;
    public GameObject Obj;
    public Sprite Sprite;

    [Header("Palettes")]
    public List<Palette> palettes = new List<Palette>();
    #endregion

    #region PRIVATE_FIELDS
    bool isStaticPalette = false;
    #endregion

    #region PROPERTIES
    public bool IsStaticPalette
    {
        get => isStaticPalette;

        set
        {
            if (value)
            {
                palettes.Clear();
                RestoreDefaultPalette();
            }



            isStaticPalette = value;
        }
    }

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
    #endregion

    #region METHODS
    public void RestoreDefaultPalette()
    {
        Palette firstPalette = new Palette();

        if (palettes != null && palettes.Count > 0)
            firstPalette = palettes[0];
        else
            palettes.Add(firstPalette);
        
        firstPalette.materials = MeshRenderer.sharedMaterials;
    }
    #endregion
}

[System.Serializable]
public class Palette
{
    public Material[] materials;
}
