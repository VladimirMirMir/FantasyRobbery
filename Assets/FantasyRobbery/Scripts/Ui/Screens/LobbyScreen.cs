using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRobbery.Scripts.Ui
{
    public class LobbyScreen : Screen
    {
        [SerializeField] private TMP_Text lobbyName;
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button leaveBtn;
        
        public override void Initialize(params string[] args)
        {
            if (args is not { Length: 2 })
                return;

            lobbyName.text = args[0];
            startGameBtn.gameObject.SetActive(args[1].Equals(true.ToString()));
        }

        protected override void OnShowComplete()
        {
            startGameBtn.onClick.AddListener(() =>
            {
                SteamLobbyService.LaunchGame();
            });
            leaveBtn.onClick.AddListener(() =>
            {
                UiService.Show<MainMenuScreen>();
                SteamLobbyService.LeaveLobby();
            });
        }
    }
}