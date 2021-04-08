using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class MainMenu : MonoBehaviour
	{
		public RaceManager raceManager;
		public GameObject raceUI;
		public Button startRaceButton;
		void Awake() {
			startRaceButton.onClick.AddListener(StartRaceClick);
		}

		void StartRaceClick() {
			raceUI.SetActive(true);
			gameObject.SetActive(false);
			raceManager.StartRace();
		}

		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
