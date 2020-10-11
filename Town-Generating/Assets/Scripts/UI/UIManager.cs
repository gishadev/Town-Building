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
    public GameObject UIElementPrefab;
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
            ObjectUIElement UIE = Instantiate(UIElementPrefab, hotbarTrans.position, Quaternion.identity, hotbarTrans).GetComponent<ObjectUIElement>();
            UIE.image.sprite = builder.objects[i].Sprite;
            UIE.text.text = builder.objects[i].Name;
        }
    }

    public void onClick_ObjectUIElement(Transform thisTrans)
    {
        WorldBuilder.Instance.objectBuilder.ChangeObject(thisTrans.GetSiblingIndex());
    }
}
