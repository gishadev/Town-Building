using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    #region Singleton
    public static WorldBuilder Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public bool showGrid = true;
    public Transform worldPlane;
    public LayerMask groundLayer;
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
    }

    void Raycast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, groundLayer))
        {
            groundPos = hitInfo.point;
            selectedNode = Grid.GetNode(Grid.Vector3ToCoords(groundPos));
        }
        else
            selectedNode = null;
    }

    public void CreateWorld(int x, int z)
    {
        Grid.CreateGridOfNodes(x, z);
        worldPlane.localScale = new Vector3(x / 10f, 1f, z / 10f);
    }

    #region Gizmos
    void OnDrawGizmos()
    {
        if (showGrid)
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
                    Vector3 center = Grid.CoordsToVector3(Grid.gridOfNodes[x, z].coords) + Vector3.right * xOffset + Vector3.forward * zOffset;
                    Vector3 size = new Vector3(1f, 0f, 1f);

                    Gizmos.DrawWireCube(center, size);
                    #endregion

                }
            }

            #region Node
            if (selectedNode != null)
            {
                Gizmos.color = new Color(0f, 255f, 0f, 120f);

                Vector3 center = Grid.CoordsToVector3(selectedNode.coords);
                Vector3 size = Vector3.one;

                Gizmos.DrawCube(center, size);
            }
            #endregion
        }
    }
    #endregion
}
