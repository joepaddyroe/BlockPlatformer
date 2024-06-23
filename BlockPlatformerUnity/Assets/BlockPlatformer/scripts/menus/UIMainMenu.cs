using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MenuScreenBase
{
    [SerializeField] private MenuScreenBase _connectPanel;
    [SerializeField] private MenuScreenBase _connectingPanel;
    [SerializeField] private MenuScreenBase _hostOrJoinPanel;
    [SerializeField] private MenuScreenBase _hostPanel;
    [SerializeField] private MenuScreenBase _joinPanel;

    private MenuScreenBase _currentPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentPanel = _connectPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnConnectionStarted()
    {
        GoToConnectingPanel();
    }

    public void OnHostClicked()
    {
        GoToHostPanel();
    }
    
    public void OnJoinClicked()
    {
        GoToJoinPanel();
    }

    public void OnBackToHostJoinClicked()
    {
        GoToHostOrJoinPanel();
    }
    
    
    

    public void GoToConnectPanel()
    {
        _currentPanel.HidePanel();
        _connectPanel.ShowPanel();
        _currentPanel = _connectPanel;
    }
    
    public void GoToConnectingPanel()
    {
        _currentPanel.HidePanel();
        _connectingPanel.ShowPanel();
        _currentPanel = _connectingPanel;
    }
    
    public void GoToHostOrJoinPanel()
    {
        _currentPanel.HidePanel();
        _hostOrJoinPanel.ShowPanel();
        _currentPanel = _hostOrJoinPanel;
    }
    
    public void GoToHostPanel()
    {
        _currentPanel.HidePanel();
        _hostPanel.ShowPanel();
        (_hostPanel as UICreateRoomMenu)?.OnEnter();
        _currentPanel = _hostPanel;
    }
    
    public void GoToJoinPanel()
    {
        _currentPanel.HidePanel();
        _joinPanel.ShowPanel();
        (_joinPanel as UIJoinRoomMenu)?.OnEnter();
        _currentPanel = _joinPanel;
    }
}
