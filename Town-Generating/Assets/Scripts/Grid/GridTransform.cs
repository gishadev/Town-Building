using UnityEngine;
using System.Linq;

public static class GridTransform
{
    #region Nodes
    public static Node GetNode(Vector2Int coords)
    {
        int x = coords.x;
        int z = coords.y;


        if (x >= 0 && x < Grid.Instance.xSize && z >= 0 && z < Grid.Instance.zSize)
            return Grid.Instance.gridOfNodes[x, z];
        else
            return null;
    }

    public static Node GetNode(Vector3 position)
    {
        Vector2Int coords = FromVector3ToCoords(position);

        int x = coords.x;
        int z = coords.y;


        if (x >= 0 && x < Grid.Instance.xSize && z >= 0 && z < Grid.Instance.zSize)
            return Grid.Instance.gridOfNodes[x, z];
        else
            return null;
    }

    public static Node[] GetNodes(Vector3 position, int xSize, int zSize, float yEulerAngles)
    {
        int yRotation = Mathf.RoundToInt(yEulerAngles);

        int newXSize = yRotation % 180f == 0 ? xSize : zSize;
        int newZSize = yRotation % 180f == 0 ? zSize : xSize;

        Vector2Int inputCoords = FromVector3ToCoords(position);
        Vector2Int offset = GetOffsetCoords(newXSize, newZSize);
        Vector2Int startCoords = new Vector2Int(inputCoords.x - offset.x, inputCoords.y - offset.y);

        Node[] result = new Node[newXSize * newZSize];

        for (int aZ = 0; aZ < newZSize; aZ++)
        {
            for (int aX = 0; aX < newXSize; aX++)
            {
                int x = startCoords.x + aX;
                int z = startCoords.y + aZ;

                if (x >= 0 && x < Grid.Instance.xSize && z >= 0 && z < Grid.Instance.zSize)
                    result[aX + (aZ * newXSize)] = Grid.Instance.gridOfNodes[x, z];
            }
        }

        return result;
    }
    #endregion

    #region Vector3 and Coords Transformations
    public static Vector3 FromCoordsToVector3(Vector2Int coords)
    {
        int x = Mathf.CeilToInt(coords.x - (Grid.Instance.xSize / 2f));
        float y = 0.501f;
        int z = Mathf.CeilToInt(coords.y - (Grid.Instance.zSize / 2f));
        return new Vector3(x, y, z);
    }

    public static Vector2Int FromVector3ToCoords(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x + (Grid.Instance.xSize / 2f));
        int z = Mathf.FloorToInt(position.z + (Grid.Instance.zSize / 2f));
        return new Vector2Int(x, z);
    }

    public static Vector3 CenterVector3FromCoords(Vector2Int a, Vector2Int b)
    {
        Vector3 firstPos = FromCoordsToVector3(a);
        Vector3 lastPos = FromCoordsToVector3(b);

        return (firstPos + lastPos) / 2f;
    }
    #endregion

    #region Other
    public static bool IsBlocked(Node node)
    {
        return node.go != null;
    }

    public static bool IsBlocked(Node[] nodes)
    {
        if (nodes.Any(x => x == null))
            return true;

        return nodes.Any(x => x.go != null);
    }

    public static Vector2Int GetOffsetCoords(int xSize, int zSize)
    {
        int xOffset = xSize % 2 == 0 ? Mathf.RoundToInt(xSize / 2f) : Mathf.CeilToInt(xSize / 2f) - 1;
        int zOffset = zSize % 2 == 0 ? Mathf.RoundToInt(zSize / 2f) : Mathf.CeilToInt(zSize / 2f) - 1;

        return new Vector2Int(xOffset, zOffset);
    }
    #endregion
}
