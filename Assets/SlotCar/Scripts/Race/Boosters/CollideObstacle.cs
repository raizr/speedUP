using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class CollideObstacle : MonoBehaviour
	{
		public Vector3 origin;
		RaycastHit hitInfo;
		public float lengthOfRay;
		public Ray ray;
		private GameObject lastObstacle;
		void Start()
		{
			var bounds = GetComponent<Collider>().bounds;
			lengthOfRay = bounds.extents.z;
		}

		void Update() {
			if (IsCollidingVertically()) {
				ApplyObstacle();

				Debug.DrawRay(origin, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
			}
		}

		bool IsCollidingVertically ()  {
			origin = GetComponent<Collider>().bounds.center;
			ray = new Ray (origin, transform.TransformDirection(Vector3.forward));
			if (Physics.Raycast (ray, out hitInfo, lengthOfRay)) {
				return true;
			}
			Debug.DrawRay (ray.origin, ray.direction * lengthOfRay, Color.yellow);
			return false;
		}

		void ApplyObstacle() {
			var collidedGameObject = hitInfo.collider.gameObject;
			if (collidedGameObject != null
				&& lastObstacle != collidedGameObject) {
				lastObstacle = collidedGameObject;
				var booster = collidedGameObject.GetComponentInParent<Booster>();
				if (booster != null) {
					booster.ApplyEffect(GetComponentInParent<Car>());
				}
			}
		}
	}
}
