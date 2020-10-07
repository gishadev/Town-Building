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

    public Node GetNode(Vector2Int coords)
    {
        int x = coords.x;
        int z = coords.y;

        if (gridOfNodes[x, z] != null)
            return gridOfNodes[x, z];
        else
        {
            Debug.LogError("Node wasn't found!");
            return null;
        }
    }

    public Vector3 CoordsToVector3(Vector2Int _coords)
    {
        int x = Mathf.CeilToInt(_coords.x - (xSize / 2f));
        int z = Mathf.CeilToInt(_coords.y - (zSize / 2f));
        return new Vector3(x, 0f, z);
    }

    public Vector2Int Vector3ToCoords(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x + (xSize / 2f));
        int z = Mathf.FloorToInt(position.z + (zSize / 2f));
        return new Vector2Int(x, z);
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
