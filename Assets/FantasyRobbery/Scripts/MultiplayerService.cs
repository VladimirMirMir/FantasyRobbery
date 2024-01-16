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

        public static ulong currentLobbyId;

        public void Init()
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

        private void OnDestroy()
        {
            _lobbyCreated.Unregister();
            _joinRequested.Unregister();
            _lobbyEntered.Unregister();
        }

        public static void CreateLobby(ELobbyType lobbyType, int maxPlayers)
        {
            Debug.Log($"Creating lobby for : {maxPlayers}; Type : {lobbyType}");
            SteamMatchmaking.CreateLobby(lobbyType, maxPlayers);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
                return;
            currentLobbyId = callback.m_ulSteamIDLobby;
            SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId), "HostAddress", Steamworks.SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId), "name", SteamFriends.GetPersonaName() + "'s lobby");
            _fishySteamworks.SetClientAddress(Steamworks.SteamUser.GetSteamID().ToString());
            _fishySteamworks.StartConnection(true);
        }

        public static string GetLobbyData(string key)
        {
            return SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), key);
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            currentLobbyId = callback.m_ulSteamIDLobby;
            _fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), "HostAddress"));
            _fishySteamworks.StartConnection(false);
        }

        public static void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyId));
            currentLobbyId = 0;
            s_instance._fishySteamworks.StopConnection(false);
            if (s_instance._networkManager.IsServerStarted)
                s_instance._fishySteamworks.StopConnection(true);
        }
    }
}