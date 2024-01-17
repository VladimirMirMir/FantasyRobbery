using FantasyRobbery.Scripts;
using FantasyRobbery.Scripts.Ui;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;
    [SerializeField] private UiService uiService;
    [SerializeField] private SteamLobbyService steamLobbyService;

    public void InitServices()
    {
        uiService.Init();
        steamLobbyService.Init(fishySteamworks);
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
