using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    #region Singleton
    public static WorldBuilder Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public bool showGrid = true;
    public Transform worldPlane;
    #endregion

    #region COMPONENETS
    public Grid Grid { private set; get; }
    #endregion

    void Awake()
    {
        Instance = this;
        Grid = GetComponent<Grid>();
    }

    void Start()
    {
        CreateWorld(Grid.xSize, Grid.zSize);
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
                for (int z = 0; z < Grid.zSize; z++)
                {
                    float xOffset, zOffset;
                    if (Grid.xSize % 2 == 0) xOffset = 0.5f;
                    else xOffset = 0f;
                    if (Grid.zSize % 2 == 0) zOffset = 0.5f;
                    else zOffset = 0f;
                    Vector3 center = Grid.CoordsToVector3(Grid.gridOfNodes[x, z].coords) + Vector3.right * xOffset + Vector3.forward * zOffset;
                    Vector3 size = new Vector3(1f, 0f, 1f);

                    Gizmos.DrawWireCube(center, size);
                }
        }
    }
    #endregion
}
