using UnityEngine;

public class Grid : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public Node[,] gridOfNodes;

    public int xSize;
    public int zSize;
    #endregion

    public void CreateGridOfNodes(int xS, int zS)
    {
        gridOfNodes = new Node[xS, zS];

        for (int x = 0; x < xS; x++)
            for (int z = 0; z < zS; z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                gridOfNodes[x, z] = new Node(coords);
            }
    }

    public Vector3 CoordsToVector3(Vector2Int _coords)
    {
        int x = Mathf.CeilToInt(_coords.x - (xSize / 2f));
        int z = Mathf.CeilToInt(_coords.y - (zSize / 2f));
        return new Vector3(x, 0f, z);
    }
}

public class Node
{
    public Vector2Int coords;

    public Node(Vector2Int _coords)
    {
        coords = _coords;
    }
}
