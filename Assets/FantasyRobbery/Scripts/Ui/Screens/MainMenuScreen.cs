using UnityEngine;
using UnityEngine.UI;

namespace FantasyRobbery.Scripts.Ui
{
    public class MainMenuScreen : Screen
    {
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button joinBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button infoBtn;
        [SerializeField] private Button quitBtn;

        public override void Initialize(params string[] args) {}
        
        protected override void OnShowComplete()
        {
            hostBtn.onClick.AddListener(() =>
            {
                SteamLobbyService.CreateLobby();
            });
            joinBtn.onClick.AddListener(() =>
            {
                //TODO : VM : Implement FindLobbyScreen matchmaking
                //UiService.Show<FindLobbyScreen>();
            });
            settingsBtn.onClick.AddListener(() =>
            {
                UiService.Show<SettingsScreen>();
            });
            infoBtn.onClick.AddListener(() =>
            {
                UiService.Show<InfoScreen>();
            });
            quitBtn.onClick.AddListener(() =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            });
        }
    }
}