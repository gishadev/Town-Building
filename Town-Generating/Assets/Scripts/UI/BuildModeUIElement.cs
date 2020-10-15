using UnityEngine;
using UnityEngine.UI;

public class BuildModeUIElement : MonoBehaviour
{
    public BuildMode newBuildMode;

    bool IsSelected { get => WorldManager.Instance.ObjectBuilder.NowBuildMode == newBuildMode; }

    #region COMPONENTS
    Animator animator;
    Button btn;
    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        btn = GetComponent<Button>();
    }

    void Start()
    {
        btn.onClick.AddListener(ClickEvent);
    }

    void LateUpdate()
    {
        animator.SetBool("isSelected", IsSelected);
    }

    void ClickEvent()
    {
        WorldManager.Instance.ObjectBuilder.ChangeBuildMode(newBuildMode);
    }
}
