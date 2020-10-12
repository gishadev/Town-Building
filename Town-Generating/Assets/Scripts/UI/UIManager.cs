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
    bool isHotbarBottom = false;
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
        builder = WorldBuilder.Instance.objectBuilder;
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

    public void onClick_ObjectUIElement(Transform thisTrans)
    {
        WorldBuilder.Instance.objectBuilder.ChangeObject(thisTrans.GetSiblingIndex());
    }

    public void onClick_ShowHotbar()
    {
        isHotbarBottom = !isHotbarBottom;
        hotbarTrans.GetComponent<Animator>().SetBool("isBottom", isHotbarBottom);

        if (isHotbarBottom)
            WorldBuilder.Instance.ChangeBuildMode(BuildMode.Build);
        else
            WorldBuilder.Instance.ChangeBuildMode(BuildMode.Nothing);
    }
}
