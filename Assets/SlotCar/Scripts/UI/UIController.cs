using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {

	public enum UIStates {
		Main,
		RaceSlot,
		RaceTap,
		FinishRace
	}
	public class UIController : ASingleton<GameInstance> {
		public UIBase mainMenu;
		public UIBase raceSlot;
		public UIBase raceTap;
		public UIBase finishRace;

		public MoneyLabel moneyLabel;
		public Dictionary<UIStates, UIBase> menus = new Dictionary<UIStates, UIBase>();
		public HashSet<UIStates> moneyLabelStates = new HashSet<UIStates>() {
			UIStates.Main,
			UIStates.FinishRace
		}; 
		private UIStates currentUI;
		protected override void Init() {
			AddMenu(UIStates.Main, mainMenu);
			AddMenu(UIStates.RaceSlot, raceSlot);
			AddMenu(UIStates.RaceTap, raceTap);
			AddMenu(UIStates.FinishRace, finishRace);
			DisableAll();
		}

		private void AddMenu(UIStates state, UIBase menu) {
			if (menu == null) {
				return;
			}
			UIBase menuObject = Instantiate(menu);
			menuObject.transform.SetParent(transform, false);
			menus.Add(state, menuObject);
		}

		private void DisableAll() {
			foreach (var stateMenu in menus) {
				stateMenu.Value.SetHide();
			}

		}

		private void HideAll() {
			foreach (var stateMenu in menus) {
				stateMenu.Value.Hide();
			}

		}
		public void EnableUI(UIStates state) {
			currentUI = state;
			HideAll();
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

		public UIBase GetUI(UIStates state) {
			return menus[state];
		}
	}
}
