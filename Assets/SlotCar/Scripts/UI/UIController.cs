using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class UIController : ASingleton<GameInstance> {
		public GameObject mainMenu;
		public GameObject raceSlot;
		public GameObject raceTap;
		public GameObject finishRace;
		public Dictionary<GameState, GameObject> menus = new Dictionary<GameState, GameObject>();
		protected override void Init() {
			AddMenu(GameState.Main, mainMenu);
			AddMenu(GameState.RaceSlot, raceSlot);
			AddMenu(GameState.RaceTap, raceTap);
			AddMenu(GameState.FinishRace, finishRace);
			DisableAll();
		}

		private void AddMenu(GameState state, GameObject menu) {
			GameObject menuObject = Instantiate(menu);
			menuObject.transform.SetParent(transform, false);
			menus.Add(state, menuObject);
		}

		private void DisableAll() {
			foreach (var stateMenu in menus) {
				stateMenu.Value.SetActive(false);
			}
		}
		public void EnableUI(GameState state) {
			DisableAll();
			if (menus.ContainsKey(state)) {
				menus[state].SetActive(true);
			}
		}
	}
}
