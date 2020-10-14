using UnityEngine;

public class Highlight : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public GameObject highlight;
    #endregion

    #region PRIVATE_FIELDS
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Material[] defaultMaterials;
    #endregion

    void Awake()
    {
        meshRenderer = highlight.GetComponent<MeshRenderer>();
        meshFilter = highlight.GetComponent<MeshFilter>();
    }

    void Start()
    {
        ChangeHighlightModel(
            WorldManager.Instance.ObjectBuilder.NowObjectToBuild.MeshRenderer,
            WorldManager.Instance.ObjectBuilder.NowObjectToBuild.MeshFilter);
    }

    public void Enable()
    {
        highlight.SetActive(true);
    }

    public void Disable()
    {
        highlight.SetActive(false);
    }

    public void PlaceHighlight(Vector3 position, Quaternion rotation)
    {
        highlight.transform.position = position;
        highlight.transform.rotation = rotation;

        meshRenderer.sharedMaterials = defaultMaterials;
    }

    public void PlaceHighlight(Vector3 position, Quaternion rotation, Material mat)
    {
        highlight.transform.position = position;
        highlight.transform.rotation = rotation;

        Material[] newMaterials = new Material[meshRenderer.sharedMaterials.Length];
        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
            newMaterials[i] = mat;

        meshRenderer.sharedMaterials = newMaterials;
    }

    public void ChangeHighlightModel(MeshRenderer _meshRenderer, MeshFilter _meshFilter)
    {
        meshRenderer.transform.localScale = _meshRenderer.transform.localScale;
        meshFilter.sharedMesh = _meshFilter.sharedMesh;

        defaultMaterials = _meshRenderer.sharedMaterials;
        meshRenderer.sharedMaterials = defaultMaterials;
    }
}
