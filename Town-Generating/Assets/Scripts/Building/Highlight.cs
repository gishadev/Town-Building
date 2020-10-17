using UnityEngine;

public class Highlight : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public GameObject highlight;
    #endregion

    #region PRIVATE_FIELDS
    MeshRenderer[] hlMeshRenderers;
    MeshFilter[] hlMeshFilters;

    Palette[] defaultPalettes;
    #endregion

    void Awake()
    {
        hlMeshRenderers = highlight.GetComponentsInChildren<MeshRenderer>();
        hlMeshFilters = highlight.GetComponentsInChildren<MeshFilter>();
    }

    void Start()
    {
        ChangeHighlightModel(WorldManager.Instance.ObjectBuilder.NowObjectToBuild.Obj);
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

        for (int i = 0; i < hlMeshRenderers.Length; i++)
            hlMeshRenderers[i].sharedMaterials = defaultPalettes[i].materials;
    }

    public void PlaceHighlight(Vector3 position, Quaternion rotation, Material mat)
    {
        highlight.transform.position = position;
        highlight.transform.rotation = rotation;

        for (int i = 0; i < hlMeshRenderers.Length; i++)
        {
            Material[] newMaterials = new Material[hlMeshRenderers[i].sharedMaterials.Length];
            for (int j = 0; j < hlMeshRenderers[i].sharedMaterials.Length; j++)
                newMaterials[j] = mat;

            hlMeshRenderers[i].sharedMaterials = newMaterials;
        }
    }

    public void ChangeHighlightModel(GameObject obj)
    {
        // Getting new components.
        MeshRenderer[] _meshRenderers = PrefabChildren.GetAllChilds<MeshRenderer>(obj.transform);
        MeshFilter[] _meshFilters = PrefabChildren.GetAllChilds<MeshFilter>(obj.transform);

        // Destroying components.
        for (int i = 0; i < highlight.transform.childCount; i++)
            Destroy(highlight.transform.GetChild(i).gameObject);

        // Reinstantiating components.
        MeshRenderer[] hlmr = new MeshRenderer[_meshFilters.Length];
        MeshFilter[] hlmf = new MeshFilter[_meshFilters.Length];
        hlmf[0] = highlight.GetComponent<MeshFilter>();
        hlmr[0] = highlight.GetComponent<MeshRenderer>();
        for (int i = 1; i < _meshFilters.Length; i++)
        {
            GameObject g = new GameObject("Child");
            g.transform.SetParent(highlight.transform);
            g.transform.localPosition = _meshFilters[i].transform.localPosition;
            hlmf[i] = g.AddComponent<MeshFilter>();
            hlmr[i] = g.AddComponent<MeshRenderer>();
        }
        hlMeshRenderers = hlmr;
        hlMeshFilters = hlmf;

        // Reinstalling materials and models in components.
        defaultPalettes = new Palette[_meshFilters.Length];
        for (int i = 0; i < _meshFilters.Length; i++)
        {
            hlMeshRenderers[i].transform.localScale = _meshRenderers[i].transform.localScale;
            hlMeshFilters[i].sharedMesh = _meshFilters[i].sharedMesh;

            defaultPalettes[i] = new Palette(_meshRenderers[i].sharedMaterials);
            hlMeshRenderers[i].sharedMaterials = defaultPalettes[i].materials;
        }
    }
}
