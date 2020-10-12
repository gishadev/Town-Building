﻿using UnityEngine;

public class Grid : MonoBehaviour
{
    #region Singleton
    public static Grid Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public Node[,] gridOfNodes;

    public int xSize;
    public int zSize;

    [HideInInspector] public Node firstNode;
    [HideInInspector] public Node lastNode;
    #endregion

    void Awake()
    {
        Instance = this;
    }

    public void CreateGridOfNodes(int xS, int zS)
    {
        gridOfNodes = new Node[xS, zS];

        for (int x = 0; x < xS; x++)
            for (int z = 0; z < zS; z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                gridOfNodes[x, z] = new Node(coords);
            }

        firstNode = gridOfNodes[0, 0];
        lastNode = gridOfNodes[xS - 1, zS - 1];
    }
}

public class Node
{
    public Vector2Int coords;
    public GameObject go;

    public Node(Vector2Int _coords)
    {
        coords = _coords;
    }
}
