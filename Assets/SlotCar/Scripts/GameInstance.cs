using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race;
using Assets.SlotCar.Scripts.UI;
using Assets.SlotCar.Scripts.Utils;
using Cinemachine;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class GameInstance : ASingleton<GameInstance> {
		public CinemachineVirtualCamera camera;
		public UIController uiController;
		public GameObject roadHolder;
		public static event Action OnAppQuit;
		[HideInInspector]
		public RaceManager raceManager;
		public RaceTap raceTap;
		public GameState gameState = GameState.Main;

		private PlayerProfile playerProfile;
		private GameDataInstance gameDataInstance;
		private GameObject levelHolder;
		protected override void Init() {
			playerProfile = PlayerProfile.Instance;
			playerProfile.Initialize();
			LoadData();
		}

		private void LoadData() {
			gameDataInstance = GameDataInstance.Instance;
			gameDataInstance.Initialize();
		}

		// Start is called before the first frame update
		void Start() {
			SwitchTo(gameState);
		}

		void SpawnCurrentLevel() {
			var currentLevel = gameDataInstance.GetLevel(playerProfile.GetCurrentLevelId());
			if (levelHolder != null) {
				Destroy(levelHolder);
			}
			levelHolder = Instantiate(currentLevel.prefab, new Vector3(0, 0, 0), Quaternion.identity);
			levelHolder.transform.SetParent(roadHolder.transform);
			levelHolder.transform.position = Vector3.zero;
		}

		void SpawnCurrentCar() {
			var currentCar = gameDataInstance.GetCar(playerProfile.GetCurrentCarId());
			var car = Instantiate(currentCar.prefab, new Vector3(0, 0, 0), Quaternion.identity);
			car.transform.SetParent(roadHolder.transform);
		}

		void InitRace() {
			var currentLevel = gameDataInstance.GetLevel(playerProfile.GetCurrentLevelId());
			if (raceManager != null) {
				if (raceManager.type != currentLevel.raceType) {
					Destroy(raceManager);
				}
			}
			else {
				if (currentLevel.raceType == RaceType.Tap) {
					raceManager = Instantiate(raceTap);
				}
			}
			SpawnCurrentLevel();
			raceManager.roadHolder = levelHolder.GetComponent<RoadMeshCreator>();
			raceManager.Init(currentLevel);
		}

		public void SwitchTo(GameState aGameState) {
			gameState = aGameState;
			uiController.EnableUI(gameState);
			switch (aGameState) {
				case GameState.RaceTap:
					raceManager.StartRace();
					break;
				case GameState.Main:
					InitRace();
					break;
			}
		}

		// Update is called once per frame
		void Update()
		{
		
		}

		void OnApplicationQuit() {
			Action onQuit = OnAppQuit;
			onQuit?.Invoke();
		}
	}
}
