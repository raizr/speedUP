using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class CollideObstacle : MonoBehaviour
	{
		public Vector3 origin;
		RaycastHit hitInfo;
		public float lengthOfRay = 2.0f;
		public Ray ray;
		public float angle = -16.0f;
		public Quaternion angleAxis;
		public Vector3 rayDirection;
		private GameObject lastCollidedObject;
		private GameObject previousCollidedObject;
		void Start()
		{
			var bounds = GetComponent<Collider>().bounds;
			//lengthOfRay = bounds.extents.z;
		}

		void Update() {
			if (IsCollidingVertically()) {
				ApplyObstacle();
				Debug.DrawRay(origin, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
			}
			else {
				CollisionExit();
			}
		}

		bool IsCollidingVertically ()  {
			origin = GetComponent<Collider>().bounds.center;
			angleAxis = Quaternion.AngleAxis(angle, Vector3.right);
			rayDirection = transform.TransformDirection(angleAxis * Vector3.forward);

			ray = new Ray(origin, rayDirection * lengthOfRay);
			if (Physics.Raycast (ray, out hitInfo, lengthOfRay)) {
				return true;
			}
			Debug.DrawRay (ray.origin, ray.direction * lengthOfRay, Color.yellow);
			return false;
		}

		void ApplyObstacle() {
			lastCollidedObject = hitInfo.collider.gameObject;
			if (lastCollidedObject != null
				&& previousCollidedObject != lastCollidedObject) {
				previousCollidedObject = lastCollidedObject;
				var booster = lastCollidedObject.GetComponentInParent<Booster>();
				if (booster != null) {
					booster.ApplyEffect(GetComponentInParent<Car>());
				}
				var boosterLine = lastCollidedObject.GetComponentInParent<BoosterLine>();
				if (boosterLine != null) {
					boosterLine.ApplyEffect(GetComponentInParent<Car>());
				}
			}
		}

		void CollisionExit() {
			if (lastCollidedObject != null) {
				var booster = lastCollidedObject.GetComponentInParent<Booster>();
				if (booster != null) {
					booster.StopEffect(GetComponentInParent<Car>());
				}
				var boosterLine = lastCollidedObject.GetComponentInParent<BoosterLine>();
				if (boosterLine != null) {
					boosterLine.StopEffect(GetComponentInParent<Car>());
				}
				lastCollidedObject = null;
			}
		}
	}
}
