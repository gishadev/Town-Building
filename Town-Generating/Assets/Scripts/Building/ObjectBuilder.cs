using UnityEngine;
using System.Linq;

public class ObjectBuilder : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("General")]
    public Transform objectsParent;
    public Highlight highlight;

    [Header("Objects")]
    //public ObjectData[] objects;
    public ObjectCategory[] objectCategories;

    [Header("Materials")]
    public Material blockedMaterial;

    [HideInInspector] public bool isEnabled = true;
    #endregion

    #region PRIVATE_FIELDS
    float yRotation;
    Vector3 groundPos;

    int groundLayerMask;
    int buildingLayerMask;

    // Destroy Raycast.
    GameObject dr_lastObj;
    #endregion

    #region PROPERTIES
    public ObjectData NowObjectToBuild { private set; get; }
    public ObjectCategory NowObjectCategory { private set; get; }
    public BuildMode NowBuildMode { private set; get; }
    public Node[] SelectedNodes { private set; get; }
    public Node[] OldNodes { private set; get; }
    #endregion

    #region COMPONENTS
    Camera cam;
    #endregion

    ////////////////////////////////////////////////// METHODS //////////////////////////////////////////////////

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        ChangeCategory(0);
        ChangeObject(0);
        ChangeBuildMode(BuildMode.Nothing);

        groundLayerMask = (1 << LayerMask.NameToLayer("Ground"));
        buildingLayerMask = (1 << LayerMask.NameToLayer("Building"));
    }

    void Update()
    {
        if (isEnabled)
        {
            if (NowBuildMode == BuildMode.Build)
            {
                BuildRaycast();

                // Building.
                if (Input.GetMouseButton(0))
                    if (SelectedNodes != null)
                    {
                        BuildObject(SelectedNodes);
                        highlight.Disable();
                    }

                // Rotation.
                if (Input.GetKeyDown(KeyCode.R))
                    RotateObject(90f);
                else if (Input.GetKeyDown(KeyCode.F))
                    RotateObject(-90f);
            }

            if (NowBuildMode == BuildMode.Destroy)
            {
                GameObject raycastedObject = DestroyRaycast();

                DestroyHighlight(raycastedObject);

                if (Input.GetMouseButton(0))
                    if (raycastedObject != null)
                        DestroyObject(dr_lastObj);
            }
        }

        else
        {
            highlight.Disable();
            if (dr_lastObj != null)
                dr_lastObj.GetComponent<Object>().ApplyDefaultMaterials();
        }

    }

    public void ChangeBuildMode(BuildMode newMode)
    {
        NowBuildMode = newMode;
    }

    #region Building
    void BuildRaycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayerMask))
        {
            groundPos = hitInfo.point;

            SelectedNodes = GridTransform.GetNodes(
                groundPos,
                NowObjectToBuild.Dimensions.x,
                NowObjectToBuild.Dimensions.y,
                GetRotation().eulerAngles.y);

            if (OldNodes == null)
                OldNodes = SelectedNodes;

            if (SelectedNodes != null)
            {
                if (SelectedNodes != OldNodes)
                    highlight.Enable();

                Node aNode = SelectedNodes.First();
                Node bNode = SelectedNodes.Last();

                Quaternion rotation = GetRotation();

                if (aNode != null && bNode != null)
                {
                    Vector3 position = GridTransform.CenterVector3FromCoords(aNode.coords, bNode.coords);

                    if (!GridTransform.IsBlocked(SelectedNodes))
                        highlight.PlaceHighlight(position, rotation);
                    else
                        highlight.PlaceHighlight(position, rotation, blockedMaterial);
                }
                else
                    highlight.Disable();

                OldNodes = SelectedNodes;
            }

        }

        else
        {
            SelectedNodes = null;
            highlight.Disable();
        }

    }

    public void BuildObject(Node[] nodes)
    {
        if (!GridTransform.IsBlocked(nodes))
        {
            Node aNode = nodes.First();
            Node bNode = nodes.Last();
            Vector3 position = GridTransform.CenterVector3FromCoords(aNode.coords, bNode.coords);

            Object o = GameObject.Instantiate(NowObjectToBuild.Obj, position, GetRotation(), objectsParent).AddComponent<Object>();
            o.thisObjectData = NowObjectToBuild;
            o.SetRandomPalette();
            foreach (Node n in nodes)
                n.obj = o;
        }
    }

    public void ChangeObject(int index)
    {
        NowObjectToBuild = NowObjectCategory.objectsData[index];
        highlight.ChangeHighlightModel(NowObjectToBuild.MeshRenderer, NowObjectToBuild.MeshFilter);
    }

    public void ChangeCategory(int index)
    {
        NowObjectCategory = objectCategories[index];
    }
    #endregion

    #region Destroying
    public void DestroyObject(GameObject selectedObject)
    {
        GameObject.Destroy(selectedObject);
    }

    GameObject DestroyRaycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, buildingLayerMask))
        {
            return hitInfo.collider.gameObject;
        }

        return null;
    }

    void DestroyHighlight(GameObject raycastedObject)
    {
        if (raycastedObject != null)
        {
            // Getting mesh renderer (really?).
            MeshRenderer mr = raycastedObject.GetComponent<MeshRenderer>();

            // Changing all materials to blockedMaterial.
            Material[] newMaterials = new Material[mr.sharedMaterials.Length];
            for (int i = 0; i < mr.sharedMaterials.Length; i++)
                newMaterials[i] = blockedMaterial;

            mr.sharedMaterials = newMaterials;

            if (dr_lastObj == null)
                dr_lastObj = raycastedObject;

            // Returning default materials.
            if (dr_lastObj != raycastedObject)
            {
                dr_lastObj.GetComponent<Object>().ApplyDefaultMaterials();
                dr_lastObj = raycastedObject;
            }
        }

        else
        {
            // Returning default materials.
            if (dr_lastObj != null)
                if (dr_lastObj != raycastedObject)
                    dr_lastObj.GetComponent<Object>().ApplyDefaultMaterials();
        }
    }
    #endregion

    #region Objects Changing
    public void RotateObject(float yRot)
    {
        yRotation += yRot;
    }
    #endregion

    #region GetValues
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Vector3.up * yRotation);
    }
    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        #region Node
        if (SelectedNodes != null)
        {
            Gizmos.color = new Color(0, 1f, 0, 0.35f);

            for (int i = 0; i < SelectedNodes.Length; i++)
            {
                Node n = SelectedNodes[i];

                if (n != null)
                {
                    Vector3 center = GridTransform.FromCoordsToVector3(n.coords);
                    Vector3 size = Vector3.one;

                    Gizmos.DrawCube(center, size);
                }
            }
        }
        #endregion
    }
    #endregion
}

public enum BuildMode
{
    Nothing,
    Build,
    Destroy
}

[System.Serializable]
public class ObjectCategory
{
    public string Name;
    public ObjectData[] objectsData;
}