using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    [Header("Building")]
    public Transform hotbarTrans;
    public GameObject arrow;
    public Transform objectsTrans;
    public GameObject UIElementPrefab;
    #endregion

    #region PRIVATE_FIELDS
    bool isHotbarBottom = true;
    #endregion

    #region COMPONENTS
    ObjectBuilder builder;
    #endregion

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        builder = WorldManager.Instance.ObjectBuilder;
        CreateObjectsUIElements();
    }

    void CreateObjectsUIElements()
    {
        for (int i = 0; i < builder.objects.Length; i++)
        {
            ObjectUIElement UIE = Instantiate(UIElementPrefab, objectsTrans.position, Quaternion.identity, objectsTrans).GetComponent<ObjectUIElement>();
            UIE.image.sprite = builder.objects[i].Sprite;
            UIE.text.text = builder.objects[i].Name;
        }
    }

    #region Building
    public void onClick_ObjectUIElement(Transform thisTrans)
    {
        if (builder.NowBuildMode != BuildMode.Build)
            builder.ChangeBuildMode(BuildMode.Build);

        builder.ChangeObject(thisTrans.GetSiblingIndex());
    }

    public void onClick_ShowHotbar()
    {
        isHotbarBottom = !isHotbarBottom;
        hotbarTrans.GetComponent<Animator>().SetBool("isBottom", isHotbarBottom);

        //if (!isHotbarBottom)
        //    WorldBuilder.Instance.ChangeBuildMode(BuildMode.Build);
        //else
        //    WorldBuilder.Instance.ChangeBuildMode(BuildMode.Nothing);
    }

    public void onClick_ChangeMode(int buildMode)
    {
        builder.ChangeBuildMode((BuildMode)buildMode);
    }
    #endregion
}
