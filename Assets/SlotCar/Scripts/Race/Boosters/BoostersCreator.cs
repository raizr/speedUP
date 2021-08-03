using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race.Boosters;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[ExecuteInEditMode]
	public class BoostersCreator : MonoBehaviour {

		public GameObject turboBoosterObject;
		public GameObject stopBoosterObject;
		public GameObject moneyBoosterObject;
		public GameObject turboLineBoosterObject;
		public GameObject speedDownLineBoosterObject;

		public PathCreator pathCreator;
		
		void Awake() {
			pathCreator = GetComponent<PathCreator>();
		}

		public void CreateBooster(BoosterType type, float time) {
			var boosterObject = Instantiate(GetPrefab(type),
				Vector3.zero,
				Quaternion.identity,
				pathCreator.gameObject.transform);
			Booster booster = boosterObject.GetComponent<Booster>();
			if (booster != null) {
				boosterObject.transform.position = pathCreator.path.GetPointAtTime(time);
				booster.timePath = time;
				booster.pathCreator = pathCreator;
			}
			BoosterLine boosterLine = boosterObject.GetComponent<BoosterLine>();
			if (boosterLine != null) {
				boosterLine.timePathStart = time;
				boosterLine.pathCreator = pathCreator;
			}
		}

		public GameObject GetPrefab(BoosterType type) {
			switch (type) {
				case BoosterType.Stop:
					return stopBoosterObject;
				case BoosterType.Money:
					return moneyBoosterObject;
				case BoosterType.TureboLine:
					return turboLineBoosterObject;
				case BoosterType.SpeedDownLine:
					return speedDownLineBoosterObject;
			}
			return turboBoosterObject;
		}

	}
}
