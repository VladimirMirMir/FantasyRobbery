using FishNet.Managing;
using Steamworks;
using UnityEngine;

namespace FantasyRobbery.Scripts
{
    [RequireComponent(typeof(NetworkManager), typeof(FishySteamworks.FishySteamworks))]
    public class MultiplayerService : MonoBehaviour
    {
        private static MultiplayerService s_instance;
        private NetworkManager _networkManager;
        private FishySteamworks.FishySteamworks _fishySteamworks;

        private Callback<LobbyCreated_t> _lobbyCreated;
        private Callback<GameLobbyJoinRequested_t> _joinRequested;
        private Callback<LobbyEnter_t> _lobbyEntered;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            _fishySteamworks = GetComponent<FishySteamworks.FishySteamworks>();
            
            if (s_instance != null)
            {
                Destroy(this);
                return;
            }

            s_instance = this;
        }

        private void Start()
        {
            _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            _joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public static void CreateLobby(ELobbyType lobbyType, int maxPlayers)
        {
            SteamMatchmaking.CreateLobby(lobbyType, maxPlayers);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            
        }
    }
}