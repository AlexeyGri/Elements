using System;
using Game.Features.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

public class InterfaceView : MonoBehaviour, IInterfaceView
{
    [SerializeField] private UIDocument _ui;

    private Button _restartBtn;
    private Button _nextBtn;

    public event Action RestartButtonClicked = delegate { }; 
    public event Action NextButtonClicked = delegate { }; 
    
    private void Awake()
    {
        var root = _ui.rootVisualElement;
        
        _restartBtn = root.Q<Button>("RestartBtn");
        _nextBtn = root.Q<Button>("NextBtn");
    }
    
    public void Show()
    {
        _restartBtn.AddToClassList("button-left--show");
        _nextBtn.AddToClassList("button-right--show");
        
        _restartBtn.clicked += RestartButtonClicked.Invoke;
        _nextBtn.clicked += NextButtonClicked.Invoke;
    }

    public void Hide()
    {
        _restartBtn.clicked -= RestartButtonClicked.Invoke;
        _nextBtn.clicked -= NextButtonClicked.Invoke;
        
        _restartBtn.RemoveFromClassList("button-left--show");
        _nextBtn.RemoveFromClassList("button-right--show");
    }

    private void OnDestroy()
    {
        _restartBtn.clicked -= RestartButtonClicked.Invoke;
        _nextBtn.clicked -= NextButtonClicked.Invoke;
    }
}
