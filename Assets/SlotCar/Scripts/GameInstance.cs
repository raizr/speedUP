using System;
using System.Collections.Generic;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.UI;
using Assets.SlotCar.Scripts.Utils;
using Cinemachine;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class GameInstance : ASingleton<GameInstance> {
		public CinemachineVirtualCamera camera;
		public UIController uiController;
		public GameObject roadHolder;
		public static event Action OnAppQuit;
		private PlayerProfile playerProfile;
		private GameDataInstance gameDataInstance;
		private GameState gameState = GameState.Main;

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
		void Start()
		{

			uiController.EnableUI(gameState);
		}

		void SelectLevel() {

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
