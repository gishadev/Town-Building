using UnityEngine;
using System;

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
    Node selectedNode;
    Node oldNode;
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

        objectBuilder.nowObject = objectBuilder.straightRoad;
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
            if (selectedNode != null)
            {
                objectBuilder.BuildObject(selectedNode);
                highlight.Disable();
            }


        // Rotation.
        if (Input.mouseScrollDelta.y > 0)
            objectBuilder.RotateObject(90f);
        else if (Input.mouseScrollDelta.y < 0)
            objectBuilder.RotateObject(-90f);
    }

    void Raycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, groundLayer))
        {
            groundPos = hitInfo.point;

            selectedNode = GridTransform.GetNode(groundPos);

            if (oldNode == null)
                oldNode = selectedNode;

            if (selectedNode != null)
            {
                if (selectedNode != oldNode)
                    highlight.Enable();

                Vector3 position = GridTransform.FromCoordsToVector3(selectedNode.coords) + Vector3.up * worldPlane.localScale.y / 2f;
                Quaternion rotation = objectBuilder.GetRotation();

                if (!GridTransform.IsBlocked(selectedNode))
                    highlight.PlaceHighlight(position, rotation);
                else
                    highlight.PlaceHighlight(position, rotation, highlight.highlightMaterial);

                oldNode = selectedNode;
            }
        }
        else
        {
            selectedNode = null;
            highlight.Disable();
        }

    }

    public void CreateWorld(int xScale, int zScale)
    {
        Grid.CreateGridOfNodes(xScale, zScale);
        worldPlane.localScale = new Vector3(xScale, worldPlane.localScale.y, zScale);
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
            //if (selectedNode != null)
            //{
            //    Gizmos.color = new Color(0, 1f, 0, 0.35f);

            //    Vector3 center = GridTransform.FromCoordsToVector3(selectedNode.coords);
            //    Vector3 size = Vector3.one;

            //    Gizmos.DrawCube(center, size);
            //}
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

    [Header("Roads Prefabs")]
    public Object straightRoad;
    public Object turnRoad;
    public Object crossRoad;
    public Object tripletRoad;
    #endregion

    #region PRIVATE_FIELDS
    float yRotation;
    [HideInInspector] public Object nowObject;
    #endregion

    public void BuildObject(Node node)
    {
        if (!GridTransform.IsBlocked(node))
        {
            Vector3 position = GridTransform.FromCoordsToVector3(node.coords);
            position.y = 0.5f;

            node.go = GameObject.Instantiate(nowObject.go, position, GetRotation(), objectsParent);
        }
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

[Serializable]
public class Object
{
    public GameObject go;

    public MeshRenderer MeshRenderer
    {
        get
        {
            return go.GetComponent<MeshRenderer>();
        }
    }

    public MeshFilter MeshFilter
    {
        get
        {
            return go.GetComponent<MeshFilter>();
        }
    }
}