using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    #region Singleton
    public static WorldBuilder Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
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

    public void UpdateWorldPlane(int x, int z)
    {
        worldPlane.localScale = new Vector3(x / 10f, 1f, z / 10f);
    }
}
