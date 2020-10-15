using UnityEngine;

public class Object : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public ObjectData thisObjectData;
    public Palette defaultPalette;
    #endregion

    #region COMPONENTS
    MeshRenderer meshRenderer;
    #endregion

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetRandomPalette()
    {
        defaultPalette = thisObjectData.palettes[Random.Range(0, thisObjectData.palettes.Length)];

        ApplyDefaultMaterials();
    }

    public void ApplyDefaultMaterials()
    {
        meshRenderer.sharedMaterials = defaultPalette.materials;
    }
}
