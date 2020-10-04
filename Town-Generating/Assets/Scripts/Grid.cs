using UnityEngine;

public class Grid : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public Node[,] grid;

    public int xSize;
    public int zSize;
    #endregion

    void Start()
    {
        grid = new Node[xSize, zSize];
    }
}

public class Node
{

}
