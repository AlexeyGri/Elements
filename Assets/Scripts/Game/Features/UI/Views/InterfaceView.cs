using System;
using Game.Features.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

public class InterfaceView : MonoBehaviour, IInterfaceView
{
    [SerializeField] private UIDocument _ui;

    private Button _restartBtn;
    private Button _nextBtn;

    public GameObject GameObject => gameObject;
    
    private void Awake()
    {
        var root = _ui.rootVisualElement;
        
        _restartBtn = root.Q<Button>("RestartBtn");
        _nextBtn = root.Q<Button>("NextBtn");

        _restartBtn.clicked += OnRestartBtnClicked;
        _nextBtn.clicked += OnNextBtnClicked;
    }

    // show button when level loaded
    private void Start()
    {
        Show();
    }

    public void Show()
    {
        _restartBtn.AddToClassList("button-left--show");
        _nextBtn.AddToClassList("button-right--show");
        
        _restartBtn.clicked += OnRestartBtnClicked;
        _nextBtn.clicked += OnNextBtnClicked;
    }

    public void Hide()
    {
        _restartBtn.clicked -= OnRestartBtnClicked;
        _nextBtn.clicked -= OnNextBtnClicked;
        
        _restartBtn.RemoveFromClassList("button-left--show");
        _nextBtn.RemoveFromClassList("button-right--show");
    }

    private void OnNextBtnClicked()
    {
        Hide();
    }

    private void OnRestartBtnClicked()
    {
        Hide();
    }

    private void OnDestroy()
    {
        _restartBtn.clicked -= OnRestartBtnClicked;
        _nextBtn.clicked -= OnNextBtnClicked;
    }
}
