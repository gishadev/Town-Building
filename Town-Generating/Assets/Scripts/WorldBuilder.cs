using UnityEngine;
using System;

public class WorldBuilder : MonoBehaviour
{
    #region Singleton
    public static WorldBuilder Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public bool showGrid = true;
    public Transform worldPlane;
    public LayerMask groundLayer;

    public RoadBuilder roadBuilder;
    #endregion

    #region PRIVATE_FIELDS
    Node selectedNode;
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
    }

    void Start()
    {
        CreateWorld(Grid.xSize, Grid.zSize);
    }

    void Update()
    {
        Raycast();
        if (Input.GetMouseButtonDown(0))
            if (selectedNode != null)
                roadBuilder.BuildRoad(selectedNode, roadBuilder.straight);
    }

    void Raycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, groundLayer))
        {
            groundPos = hitInfo.point;

            selectedNode = GridTransform.GetNode(groundPos);
        }
        else
            selectedNode = null;
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
            if (selectedNode != null)
            {
                Gizmos.color = new Color(0, 1f, 0, 0.35f);

                Vector3 center = GridTransform.FromCoordsToVector3(selectedNode.coords);
                Vector3 size = Vector3.one;

                Gizmos.DrawCube(center, size);
            }
            #endregion
        }
    }
    #endregion
}

[Serializable]
public class RoadBuilder
{
    [Header("General")]
    public Transform roadsParent;

    [Header("Roads Prefabs")]
    public GameObject straight;
    public GameObject turn;
    public GameObject cross;
    public GameObject triplet;


    public void BuildRoad(Node node, GameObject prefab)
    {
        if (!IsBlocked(node))
        {
            Vector3 position = GridTransform.FromCoordsToVector3(node.coords);
            position.y = 0.5f;

            node.road = GameObject.Instantiate(prefab, position, Quaternion.identity, roadsParent);
        }
    }

    bool IsBlocked(Node node)
    {
        return node.road != null;
    }
}