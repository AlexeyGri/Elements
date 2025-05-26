using UnityEngine;
using UnityEngine.UIElements;

public class InterfaceView : MonoBehaviour
{
    [SerializeField] private UIDocument _ui;

    private Button _restart;
    private Button _next;
    
    private void Start()
    {
        var root = _ui.rootVisualElement;
        
        _restart = root.Q<Button>("RestartBtn");
        _next = root.Q<Button>("NextBtn");

        _restart.clicked += OnRestartClicked;
        _next.clicked += OnNextClicked;
    }

    private void OnNextClicked()
    {
        Debug.Log("Next");
    }

    private void OnRestartClicked()
    {
        Debug.Log("Restart");
    }

    private void OnDestroy()
    {
        _restart.clicked -= OnRestartClicked;
        _next.clicked -= OnNextClicked;
    }
}
