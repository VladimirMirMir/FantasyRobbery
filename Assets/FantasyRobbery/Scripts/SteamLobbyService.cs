using System.Linq;
using FantasyRobbery.Scripts.Ui;
using FishNet;
using Steamworks;
using UnityEngine;

namespace FantasyRobbery.Scripts
{
    public class SteamLobbyService : MonoBehaviour
    {
        private static SteamLobbyService s_instance;
        
        [SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;

        private Callback<LobbyCreated_t> _lobbyCreated;
        private Callback<GameLobbyJoinRequested_t> _joinRequested;
        private Callback<LobbyEnter_t> _lobbyEntered;

        public static CSteamID currentLobbyId;

        public void Init(FishySteamworks.FishySteamworks steamworks)
        {
            if (s_instance != null)
            {
                Destroy(this);
                return;
            }

            s_instance = this;
            s_instance.fishySteamworks = steamworks;
        }

        private void Start()
        {
            _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            _joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        private void OnDestroy()
        {
            _lobbyCreated?.Unregister();
            _joinRequested?.Unregister();
            _lobbyEntered?.Unregister();
        }

        public static string GetLobbyData(string key)
        {
            return SteamMatchmaking.GetLobbyData(currentLobbyId, key);
        }

        public static void LaunchGame()
        {
            UiService.ToggleMainMenuCamera(false);
            var connections = InstanceFinder.ServerManager.Clients.Values.ToArray();
            MultiplayerService.ChangeNetworkScene("TestScene", connections);
            MultiplayerService.SpawnRobbers(connections);
        }

        public static void CreateLobby(ELobbyType lobbyType = ELobbyType.k_ELobbyTypeFriendsOnly, int maxPlayers = 4)
        {
            Debug.Log($"Creating lobby for : {maxPlayers}; Type : {lobbyType}");
            SteamMatchmaking.CreateLobby(lobbyType, maxPlayers);
        }

        public static void JoinLobby(CSteamID steamID)
        {
            if (SteamMatchmaking.RequestLobbyData(steamID))
                SteamMatchmaking.JoinLobby(steamID);
            else
                Debug.LogError($"Failed to join lobby with id : {steamID.m_SteamID}");
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            Debug.Log($"Lobby was create with result: {callback.m_eResult}");
            if (callback.m_eResult != EResult.k_EResultOK)
                return;
            currentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);
            SteamMatchmaking.SetLobbyData(currentLobbyId, "HostAddress", Steamworks.SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(currentLobbyId, "name", SteamFriends.GetPersonaName() + "'s lobby");
            fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
            fishySteamworks.StartConnection(true);
            Debug.Log("Lobby created and set");
            //JoinLobby(currentLobbyId);
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Join request");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            Debug.Log("OnLobbyentered");
            currentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);
            
            var screen = UiService.Show<LobbyScreen>();
            screen.Initialize(GetLobbyData("name"), InstanceFinder.IsServerStarted.ToString());

            fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(currentLobbyId, "HostAddress"));
            fishySteamworks.StartConnection(false);
        }

        public static void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(currentLobbyId);
            currentLobbyId = new CSteamID(0);
            s_instance.fishySteamworks.StopConnection(false);
            if (InstanceFinder.IsServerStarted)
                s_instance.fishySteamworks.StopConnection(true);
        }
    }
}