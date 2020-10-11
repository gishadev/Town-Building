using UnityEngine;
using System;
using System.Linq;

public class WorldBuilder : MonoBehaviour
{
    #region Singleton
    public static WorldBuilder Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public Highlight highlight;
    [Space]
    public bool showGrid = true;
    public Transform worldPlane;
    public LayerMask groundLayer;
    [Space]
    public ObjectBuilder objectBuilder;
    #endregion

    #region PRIVATE_FIELDS
    Node[] selectedNodes;
    Node[] oldNodes;
    Vector3 groundPos;
    #endregion

    #region COMPONENETS
    public Grid Grid { private set; get; }
    Camera cam;
    #endregion

    void Awake()
    {
        Instance = this;
        Grid = GetComponent<Grid>();
        cam = Camera.main;

        objectBuilder.nowObject = objectBuilder.objects[0];
    }

    void Start()
    {
        CreateWorld(Grid.xSize, Grid.zSize);
    }

    void Update()
    {
        Raycast();

        // Building.
        if (Input.GetMouseButton(0))
            if (selectedNodes != null)
            {
                objectBuilder.BuildObject(selectedNodes);
                highlight.Disable();
            }

        // Rotation.
        if (Input.mouseScrollDelta.y > 0)
            objectBuilder.RotateObject(90f);
        else if (Input.mouseScrollDelta.y < 0)
            objectBuilder.RotateObject(-90f);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearWorld();
        }
    }

    void Raycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, groundLayer))
        {
            groundPos = hitInfo.point;

            selectedNodes = GridTransform.GetNodes(
                groundPos,
                objectBuilder.nowObject.xSize,
                objectBuilder.nowObject.zSize,
                objectBuilder.GetRotation().eulerAngles.y);

            if (oldNodes == null)
                oldNodes = selectedNodes;

            if (selectedNodes != null)
            {
                if (selectedNodes != oldNodes)
                    highlight.Enable();

                Node aNode = selectedNodes.First();
                Node bNode = selectedNodes.Last();

                Quaternion rotation = objectBuilder.GetRotation();

                if (aNode != null && bNode != null)
                {
                    Vector3 position = GridTransform.CenterVector3FromCoords(aNode.coords, bNode.coords);

                    if (!GridTransform.IsBlocked(selectedNodes))
                        highlight.PlaceHighlight(position, rotation);
                    else
                        highlight.PlaceHighlight(position, rotation, highlight.highlightMaterial);
                }
                else
                    highlight.Disable();

                oldNodes = selectedNodes;
            }
        }

        else
        {
            selectedNodes = null;
            highlight.Disable();
        }

    }

    public void CreateWorld(int xScale, int zScale)
    {
        Grid.CreateGridOfNodes(xScale, zScale);
        worldPlane.localScale = new Vector3(xScale, worldPlane.localScale.y, zScale);
    }

    public void ClearWorld()
    {
        for (int i = 0; i < objectBuilder.objectsParent.childCount; i++)
            Destroy(objectBuilder.objectsParent.GetChild(i).gameObject);
    }

    #region Gizmos
    void OnDrawGizmos()
    {
        if (showGrid && Application.isPlaying)
        {
            Gizmos.color = Color.blue;

            for (int x = 0; x < Grid.xSize; x++)
            {
                for (int z = 0; z < Grid.zSize; z++)
                {
                    #region Grid
                    float xOffset, zOffset;
                    if (Grid.xSize % 2 == 0) xOffset = 0.5f;
                    else xOffset = 0f;
                    if (Grid.zSize % 2 == 0) zOffset = 0.5f;
                    else zOffset = 0f;
                    Vector3 rawPosition = GridTransform.FromCoordsToVector3(Grid.gridOfNodes[x, z].coords);
                    Vector3 center = new Vector3(rawPosition.x + xOffset, worldPlane.localScale.y / 2f, rawPosition.z + zOffset);
                    Vector3 size = new Vector3(1f, 0f, 1f);

                    Gizmos.DrawWireCube(center, size);
                    #endregion

                }
            }

            #region Node
            if (selectedNodes != null)
            {
                Gizmos.color = new Color(0, 1f, 0, 0.35f);

                for (int i = 0; i < selectedNodes.Length; i++)
                {
                    Node n = selectedNodes[i];

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
    }
    #endregion
}

[Serializable]
public class ObjectBuilder
{
    #region PUBLIC_FIELDS
    [Header("General")]
    public Transform objectsParent;

    [Header("Objects")]
    public ObjectData[] objects;
    #endregion

    #region PRIVATE_FIELDS
    float yRotation;
    [HideInInspector] public ObjectData nowObject;
    #endregion

    public void BuildObject(Node[] nodes)
    {
        if (!GridTransform.IsBlocked(nodes))
        {
            Node aNode = nodes.First();
            Node bNode = nodes.Last();
            Vector3 position = GridTransform.CenterVector3FromCoords(aNode.coords, bNode.coords);

            GameObject go = GameObject.Instantiate(nowObject.Obj, position, GetRotation(), objectsParent);
            foreach (Node n in nodes)
                n.go = go;
        }
    }

    public void ChangeObject(int index)
    {
        nowObject = objects[index];
        WorldBuilder.Instance.highlight.ChangeHighlightModel(nowObject.MeshRenderer, nowObject.MeshFilter);
    }

    public void RotateObject(float yRot)
    {
        yRotation += yRot;
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Vector3.up * yRotation);
    }
}