using FantasyRobbery.Scripts;
using FantasyRobbery.Scripts.Ui;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private UiService _uiService;
    [SerializeField] private MultiplayerService _multiplayerService;

    public void InitServices()
    {
        _uiService.Init();
        _multiplayerService.Init();
    }
    
    public void LoadMainMenu()
    {
        UiService.Show<MainMenuScreen>();
    }

    public void OnSteamLoadError(string errorCode)
    {
        Debug.Log(errorCode);
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
