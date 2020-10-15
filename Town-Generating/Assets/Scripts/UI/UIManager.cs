using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    #region PUBLIC_FIELDS
    public GameObject arrow;
    [Header("Transforms")]
    public Transform hotbarTrans;
    public Transform objectsTrans;
    public Transform categoriesTrans;
    [Header("UI Elements")]
    public GameObject ObjectUIElementPrefab;
    public GameObject CategoryUIElementPrefab;
    public GameObject objectsParentPrefab;
    #endregion

    #region PRIVATE_FIELDS
    bool isHotbarBottom = true;
    List<CategoryUIElement> CUIEs = new List<CategoryUIElement>();
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
        CreateHotbarUIElements();
    }

    void CreateHotbarUIElements()
    {
        for (int i = 0; i < builder.objectCategories.Length; i++)
        {
            ObjectCategory category = builder.objectCategories[i];

            // Creating objects parent for objects.
            GameObject objectsParent = Instantiate(objectsParentPrefab, objectsTrans.position, Quaternion.identity, objectsTrans);
            objectsParent.name = category.Name;

            // Creating Category UI Element.
            CategoryUIElement CUIE = Instantiate(CategoryUIElementPrefab, categoriesTrans.position, Quaternion.identity, categoriesTrans).GetComponent<CategoryUIElement>();
            CUIE.Text.text = category.Name;
            CUIE.ObjectsParent = objectsParent;
            CUIEs.Add(CUIE);

            CUIE.ObjectsParent.SetActive(false);
            if (i == 0)
                CUIE.ObjectsParent.SetActive(true);

            for (int j = 0; j < category.objectsData.Length; j++)
            {
                // Creating Object UI Element for current Category UI Element.
                ObjectUIElement OUIE = Instantiate(ObjectUIElementPrefab, objectsParent.transform.position, Quaternion.identity, objectsParent.transform).GetComponent<ObjectUIElement>();
                OUIE.Image.sprite = category.objectsData[j].Sprite;
                OUIE.Text.text = category.objectsData[j].Name;
            }
        }
    }

    #region Building
    public void onClick_ObjectUIElement(ObjectUIElement currOUIE)
    {
        if (builder.NowBuildMode != BuildMode.Build)
            builder.ChangeBuildMode(BuildMode.Build);

        builder.ChangeObject(currOUIE.transform.GetSiblingIndex());
    }

    public void onClick_CategoryUIElement(CategoryUIElement currCUIE)
    {
        for (int i = 0; i < builder.objectCategories.Length; i++)
            CUIEs[i].ObjectsParent.SetActive(false);

        currCUIE.ObjectsParent.SetActive(true);
        builder.ChangeCategory(currCUIE.transform.GetSiblingIndex());
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
