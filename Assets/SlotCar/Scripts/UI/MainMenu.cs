using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class MainMenu : MonoBehaviour
	{
		public RaceManager raceManager;
		public GameObject raceUI;
		public Button startRaceButton;
		public Firebase.FirebaseApp firebaseApp;
		void Awake() {
			startRaceButton.onClick.AddListener(StartRaceClick);
		}

		void Start() {
			Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					// Create and hold a reference to your FirebaseApp,
					// where app is a Firebase.FirebaseApp property of your application class.
					firebaseApp = Firebase.FirebaseApp.DefaultInstance;

					// Set a flag here to indicate whether Firebase is ready to use by your app.
				} else {
					UnityEngine.Debug.LogError(System.String.Format(
						"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
					// Firebase Unity SDK is not safe to use here.
				}
			});
			Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventAppOpen);
		}
		void StartRaceClick() {
			raceUI.SetActive(true);
			gameObject.SetActive(false);
			raceManager.StartRace();
		}
	}
}
