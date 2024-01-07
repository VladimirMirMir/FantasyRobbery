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

        protected override void OnShowComplete()
        {
            hostBtn.onClick.AddListener(() =>
            {
                UiService.Hide<MainMenuScreen>();
                UiService.Show<CreateLobbyScreen>();
            });
        }
    }
}