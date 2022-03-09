using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UiMenu : MonoBehaviour, InputActions.IUIActions
{
    [SerializeField] private List<Button> _buttons;

    private int _selectedButtonIndex;
    private EventSystem _eventSystem;
    private void Awake()
    {
        _eventSystem = EventSystem.current;
    }

    protected virtual void OnEnable()
    {
        _selectedButtonIndex = 0;
        _eventSystem.SetSelectedGameObject(_buttons[_selectedButtonIndex].gameObject);
        GameManager.Instance.InputActions.UI.Enable();
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.InputActions.UI.Disable();
    }

    public virtual void OnNext(InputAction.CallbackContext context)
    {
        _selectedButtonIndex++;
        if (_selectedButtonIndex >= _buttons.Count)
        {
            _selectedButtonIndex = 0;
        }

        _eventSystem.SetSelectedGameObject(_buttons[_selectedButtonIndex].gameObject);
    }

    public virtual void OnPrev(InputAction.CallbackContext context)
    {
        _selectedButtonIndex--;
        if (_selectedButtonIndex <= 0)
        {
            _selectedButtonIndex = _buttons.Count - 1;
        }

        _eventSystem.SetSelectedGameObject(_buttons[_selectedButtonIndex].gameObject);
    }

    public virtual void OnSelect(InputAction.CallbackContext context)
    {
        _buttons[_selectedButtonIndex].onClick.Invoke();
    }
}
