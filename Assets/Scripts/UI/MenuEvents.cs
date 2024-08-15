using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class MenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private List<Button> _menuButtons = new List<Button>();


    private void Awake() {
        _document = GetComponent<UIDocument>();
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++) {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable() {
        for (int i = 0; i < _menuButtons.Count; i++) {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnAllButtonsClick(ClickEvent evt) {
        Debug.Log("All Buttons");
    }

}
