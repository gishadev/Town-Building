using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIElement : MonoBehaviour
{
    [HideInInspector] public Image image;
    [HideInInspector] public TMP_Text text;
    [HideInInspector] public Button btn;

    void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
        btn = GetComponent<Button>();
    }

    void Start()
    {
        btn.onClick.AddListener(BtnAction);
    }

    void BtnAction()
    {
        UIManager.Instance.onClick_ObjectUIElement(transform);
    }
}
