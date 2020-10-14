using UnityEngine;

public class Object : MonoBehaviour
{
    #region PUBLIC_FIELDS
     public Material[] defaultMaterials;
    #endregion

    #region COMPONENTS
    MeshRenderer meshRenderer;
    #endregion

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ReturnDefaultMaterials()
    {
        meshRenderer.sharedMaterials = defaultMaterials;
    }
}
