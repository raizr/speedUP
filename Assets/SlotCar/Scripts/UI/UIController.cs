using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class UIController : ASingleton<GameInstance> {
		public UIBase mainMenu;
		public UIBase raceSlot;
		public UIBase raceTap;
		public UIBase finishRace;

		public MoneyLabel moneyLabel;
		public Dictionary<GameState, UIBase> menus = new Dictionary<GameState, UIBase>();
		public HashSet<GameState> moneyLabelStates = new HashSet<GameState>() {
			GameState.Main,
			GameState.FinishRace
		}; 
		private GameState currentUI;
		protected override void Init() {
			AddMenu(GameState.Main, mainMenu);
			//AddMenu(GameState.RaceSlot, raceSlot);
			AddMenu(GameState.RaceTap, raceTap);
			AddMenu(GameState.FinishRace, finishRace);
			DisableAll();
		}

		private void AddMenu(GameState state, UIBase menu) {
			if (menu == null) {
				return;
			}
			UIBase menuObject = Instantiate(menu);
			menuObject.transform.SetParent(transform, false);
			menus.Add(state, menuObject);
		}

		private void DisableAll() {
			foreach (var stateMenu in menus) {
				stateMenu.Value.Hide();
			}

		}
		public void EnableUI(GameState state) {
			currentUI = state;
			DisableAll();
			if (menus.ContainsKey(state)) {
				menus[state].Show();
			}

			if (moneyLabelStates.Contains(state)) {
				moneyLabel.Show();
			}
			else {
				moneyLabel.Hide();
			}
		}

		public void HideUI() {
			DisableAll();
		}

		public void ShowUI() {
			EnableUI(currentUI);
		}

		public UIBase GetUI(GameState state) {
			return menus[state];
		}
	}
}
