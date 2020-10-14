using UnityEngine;
public class WorldManager : MonoBehaviour
{
    #region Singleton
    public static WorldManager Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    [Header("World Sizes")]
    public int xSize = 55;
    public int zSize = 55;

    [Header("Debugging")]
    public bool showGrid = true;

    [Header("World Plane")]
    public Transform worldPlane;
    #endregion

    #region PROPERTIES
    public Grid Grid { get { return Grid.Instance; } }
    public ObjectBuilder ObjectBuilder { private set; get; }
    #endregion


    void Awake()
    {
        Instance = this;

        ObjectBuilder = GetComponent<ObjectBuilder>();
    }

    void Start()
    {
        CreateWorld(xSize, zSize);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
            ClearWorld();
    }

    #region World
    public void CreateWorld(int xScale, int zScale)
    {
        Grid.CreateGridOfNodes(xScale, zScale);
        worldPlane.localScale = new Vector3(xScale, worldPlane.localScale.y, zScale);
    }

    public void ClearWorld()
    {
        for (int i = 0; i < ObjectBuilder.objectsParent.childCount; i++)
            Destroy(ObjectBuilder.objectsParent.GetChild(i).gameObject);
    }
    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        if (showGrid && Application.isPlaying)
        {
            Gizmos.color = Color.blue;

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    #region Grid
                    float xOffset, zOffset;
                    if (xSize % 2 == 0) xOffset = 0.5f;
                    else xOffset = 0f;
                    if (zSize % 2 == 0) zOffset = 0.5f;
                    else zOffset = 0f;
                    Vector3 rawPosition = GridTransform.FromCoordsToVector3(Grid.gridOfNodes[x, z].coords);
                    Vector3 center = new Vector3(rawPosition.x + xOffset, worldPlane.localScale.y / 2f, rawPosition.z + zOffset);
                    Vector3 size = new Vector3(1f, 0f, 1f);

                    Gizmos.DrawWireCube(center, size);
                    #endregion
                }
            }
        }
    }
    #endregion
}