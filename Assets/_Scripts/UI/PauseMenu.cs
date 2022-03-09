using UnityEngine;

public class PauseMenu : UiMenu
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
        GameManager.Instance.InputActions.Character.Disable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = 1f;
        GameManager.Instance.InputActions.Character.Enable();
    }
}
