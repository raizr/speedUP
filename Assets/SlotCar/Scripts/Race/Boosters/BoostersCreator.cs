using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race.Boosters;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[ExecuteInEditMode]
	public class BoostersCreator : MonoBehaviour {

		public Booster[] boosters;
		public GameObject turboBoosterObject;
		public GameObject stopBoosterObject;
		public GameObject moneyBoosterObject;

		public PathCreator pathCreator;
		
		void Awake() {
			pathCreator = GetComponent<PathCreator>();
		}

		public void UpdateBooster() {
			boosters = GetComponentsInChildren<Booster>();
		}

		public void CreateBooster(BoosterType type, float time) {
			var boosterObject = Instantiate(GetPrefab(type),
				Vector3.zero,
				Quaternion.identity,
				pathCreator.gameObject.transform);
			boosterObject.transform.position = pathCreator.path.GetPointAtTime(time);
			Booster booster = boosterObject.GetComponent<Booster>();
			booster.timePath = time;
			booster.pathCreator = pathCreator;
			UpdateBooster();
		}


		public GameObject GetPrefab(BoosterType type) {
			switch (type) {
				case BoosterType.Stop:
					return stopBoosterObject;
				case BoosterType.Money:
					return moneyBoosterObject;
			}
			return turboBoosterObject;
		}

		void Start() {
			UpdateBooster();
		}

		void Update() {

		}
	}
}
