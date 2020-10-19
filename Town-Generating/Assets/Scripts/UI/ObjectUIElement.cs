using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIElement : MonoBehaviour
{
    #region PROPERTIES
    public Image iconImg;
    public TMP_Text Text { get => GetComponentInChildren<TMP_Text>(); }
    public Button Btn { get => GetComponent<Button>(); }
    #endregion

    void Start()
    {
        Btn.onClick.AddListener(BtnAction);
    }

    void BtnAction()
    {
        UIManager.Instance.onClick_ObjectUIElement(this);
    }
}
