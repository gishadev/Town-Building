using UnityEngine;
using System.Linq;

public static class GridTransform
{
    #region Nodes
    public static Node GetNode(Vector2Int coords)
    {
        int x = coords.x;
        int z = coords.y;


        if (x >= 0 && x < Grid.Instance.XSize && z >= 0 && z < Grid.Instance.ZSize)
            return Grid.Instance.gridOfNodes[x, z];
        else
            return null;
    }

    public static Node GetNode(Vector3 position)
    {
        Vector2Int coords = FromVector3ToCoords(position);

        int x = coords.x;
        int z = coords.y;


        if (x >= 0 && x < Grid.Instance.XSize && z >= 0 && z < Grid.Instance.ZSize)
            return Grid.Instance.gridOfNodes[x, z];
        else
            return null;
    }

    public static Node[] GetNodes(Vector3 position, int xSize, int zSize, float yEulerAngles)
    {
        int yRotation = Mathf.RoundToInt(yEulerAngles);

        int newXSize = yRotation % 180f == 0 ? xSize : zSize;
        int newZSize = yRotation % 180f == 0 ? zSize : xSize;

        Vector2 inputCoords = FromVector3ToRawCoords(position);
        Vector2 offset = GetOffsetCoords(newXSize, newZSize);

        // Applying offsets differently for x/y % 2 == 0 or x/y % 2 != 0.
        int xCoord = newXSize % 2 == 0 ? Mathf.RoundToInt(inputCoords.x - offset.x - 0.5f) : Mathf.CeilToInt(inputCoords.x - offset.x);
        int zCoord = newZSize % 2 == 0 ? Mathf.RoundToInt(inputCoords.y - offset.y - 0.5f) : Mathf.CeilToInt(inputCoords.y - offset.y);
        Vector2Int startCoords = new Vector2Int(xCoord, zCoord);

        Node[] result = new Node[newXSize * newZSize];

        for (int aZ = 0; aZ < newZSize; aZ++)
        {
            for (int aX = 0; aX < newXSize; aX++)
            {
                int x = startCoords.x + aX;
                int z = startCoords.y + aZ;

                if (x >= 0 && x < Grid.Instance.XSize && z >= 0 && z < Grid.Instance.ZSize)
                    result[aX + (aZ * newXSize)] = Grid.Instance.gridOfNodes[x, z];
            }
        }

        return result;
    }
    #endregion

    #region Vector3 and Coords Transformations
    public static Vector3 FromCoordsToVector3(Vector2Int coords)
    {
        int x = Mathf.CeilToInt(coords.x - (Grid.Instance.XSize / 2f));
        float y = 0.501f;
        int z = Mathf.CeilToInt(coords.y - (Grid.Instance.ZSize / 2f));
        return new Vector3(x, y, z);
    }

    public static Vector2Int FromVector3ToCoords(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x + (Grid.Instance.XSize / 2f));
        int z = Mathf.FloorToInt(position.z + (Grid.Instance.ZSize / 2f));
        return new Vector2Int(x, z);
    }

    public static Vector2 FromVector3ToRawCoords(Vector3 position)
    {
        float x = position.x + (Grid.Instance.XSize / 2f);
        float z = position.z + (Grid.Instance.ZSize / 2f);
        return new Vector2(x, z);
    }

    public static Vector3 CenterVector3FromCoords(Vector2Int a, Vector2Int b)
    {
        Vector3 firstPos = FromCoordsToVector3(a);
        Vector3 lastPos = FromCoordsToVector3(b);

        return (firstPos + lastPos) / 2f;
    }
    #endregion

    #region Other
    public static bool IsBlocked(Node node, ObjectType objType)
    {
        if (node == null)
            return true;

        if (objType == ObjectType.Building)
            return node.building != null;
        else if (objType == ObjectType.Flooring)
            return node.flooring != null;
        else
        {
            Debug.LogError("Object Type wasn't found! Please, check the script.");
            return true;
        }
    }

    public static bool IsBlocked(Node[] nodes, ObjectType objType)
    {
        foreach (Node n in nodes)
        {
            if (IsBlocked(n, objType))
                return true;
        }

        return false;

        //if (nodes.Any(x => x == null))
        //    return true;

        //return nodes.Any(x => x.obj != null);
    }

    public static Vector2 GetOffsetCoords(int xSize, int zSize)
    {
        float xOffset = xSize % 2 == 0 ? xSize / 2f - 0.5f : Mathf.Ceil(xSize / 2f);
        float zOffset = zSize % 2 == 0 ? zSize / 2f - 0.5f : Mathf.Ceil(zSize / 2f);

        return new Vector2(xOffset, zOffset);
    }
    #endregion
}
