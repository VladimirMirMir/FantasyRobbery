using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRobbery.Scripts.Ui
{
    public class CreateLobbyScreen : Screen
    {
        [SerializeField] private TMP_Dropdown _lobbyTypeDropdown;
        [SerializeField] private Slider _maxPlayersCountSlider;
        [SerializeField] private Button _createLobbyBtn;

        protected override void OnShowComplete()
        {
            _createLobbyBtn.onClick.AddListener(() =>
            {
                MultiplayerService.CreateLobby((ELobbyType)_lobbyTypeDropdown.value, (int)_maxPlayersCountSlider.value);
            });
        }
    }
}