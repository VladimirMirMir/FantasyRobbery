using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

namespace FantasyRobbery.Scripts
{
    public class MultiplayerService : NetworkBehaviour
    {
        private static MultiplayerService s_instance;

        [SerializeField] private GameObject robberPrefab;

        private void Start()
        {
            if (s_instance == null)
                s_instance = this;
        }
        
        public static void ChangeNetworkScene(string sceneToLoad, NetworkConnection[] connections, params string[] scenesToUnload)
        {
            s_instance.UnloadScenes(scenesToUnload);
            var sceneData = new SceneLoadData(sceneToLoad);
            InstanceFinder.SceneManager.LoadConnectionScenes(connections, sceneData);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UnloadScenes(params string[] scenesToUnload)
        {
            UnloadScenesObserver(scenesToUnload);
        }

        [ObserversRpc]
        private void UnloadScenesObserver(params string[] scenesToUnload)
        {
            if (scenesToUnload == null)
                return;
            foreach (string sceneName in scenesToUnload)
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }

        public static void SpawnRobbers(NetworkConnection[] connections)
        {
            foreach (var connection in connections)
                s_instance.SpawnRobberServer(connection);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnRobberServer(NetworkConnection connection)
        {
            var spawnPoint = Random.insideUnitCircle * 3;
            var robber = Instantiate(robberPrefab, new Vector3(spawnPoint.x, 0, spawnPoint.y), Quaternion.identity);
            Spawn(robber, connection);
        }
    }
}