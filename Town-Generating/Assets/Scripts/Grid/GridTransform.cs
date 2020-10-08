using UnityEngine;

public static class GridTransform
{
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

    public static Vector3 FromCoordsToVector3(Vector2Int coords)
    {
        int x = Mathf.CeilToInt(coords.x - (Grid.Instance.xSize / 2f));
        int z = Mathf.CeilToInt(coords.y - (Grid.Instance.zSize / 2f));
        return new Vector3(x, 0f, z);
    }

    public static Vector2Int FromVector3ToCoords(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x + (Grid.Instance.xSize / 2f));
        int z = Mathf.FloorToInt(position.z + (Grid.Instance.zSize / 2f));
        return new Vector2Int(x, z);
    }
}
