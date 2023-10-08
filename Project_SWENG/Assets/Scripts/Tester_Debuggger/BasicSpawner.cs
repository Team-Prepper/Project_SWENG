using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Fusion106
{
	public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
	{
		private NetworkRunner _runner;
		[SerializeField] GridMaker _gridMaker;
        [SerializeField] PlayerSpawner _playerSpawner;

        private void OnGUI()
		{
			if (_runner == null)
			{
				if (GUI.Button(new Rect(0,0,200,40), "Host"))
				{
					StartGame(GameMode.Host);
				}
				if (GUI.Button(new Rect(0,40,200,40), "Join"))
				{
					StartGame(GameMode.Client);
				}
			}
			
		}

		async void StartGame(GameMode mode)
		{
			// Create the Fusion runner and let it know that we will be providing user input
			_runner = gameObject.AddComponent<NetworkRunner>();
			_runner.ProvideInput = true;

			// Start or join (depends on gamemode) a session with a specific name
			await _runner.StartGame(new StartGameArgs()
			{
				GameMode = mode, 
				SessionName = "TestRoom", 
				Scene = SceneManager.GetActiveScene().buildIndex,
				SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
			});
		}

		[SerializeField] private NetworkPrefabRef _playerPrefab; // Character to spawn for a joining player
		private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public UnityEvent<GameObject> EventPlayerSpawn;

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			if (runner.IsServer)
			{
				_gridMaker.CreateHexGrid();
			}

			if (runner.IsPlayer)
			{
                Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();

                Transform spawnPos = spawnHex.transform;
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPos.position, spawnPos.rotation, player);

                UIManager.OpenGUI<GUI_PlayerInfor>("UnitInfor").SetPlayer(networkPlayerObject.gameObject);
                
                _spawnedCharacters.Add(player, networkPlayerObject);

                spawnHex.Entity = networkPlayerObject.gameObject;

                EventPlayerSpawn?.Invoke(networkPlayerObject.gameObject);

                GameManager.Instance.player = networkPlayerObject.gameObject;
                GameManager.Instance.gamePhase = GameManager.Phase.Start;

                CloudBox.Instance.CloudActiveFalse(spawnHex.HexCoords);
            }
		}

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
			{
				runner.Despawn(networkObject);
				_spawnedCharacters.Remove(player);
			}
		}

		private bool _mouseButton0;
		private bool _mouseButton1;

		private void Update()
		{
			_mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
			_mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
		}

		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			
		}

		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
		public void OnConnectedToServer(NetworkRunner runner) { }
		public void OnDisconnectedFromServer(NetworkRunner runner) { }
		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
		public void OnSceneLoadDone(NetworkRunner runner) { }
		public void OnSceneLoadStart(NetworkRunner runner) { }
	}
}
