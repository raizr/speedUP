using System;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	[ExecuteInEditMode]
	public class Booster : MonoBehaviour {
		public PathCreator pathCreator;
		public RoadMeshCreator roadMeshCreator;
		[Range(0.0f, 1.0f)]
		public float timePath;
		public int linePosition = 0;
		public float center;
		public Transform linePosTransform;
		public float coeff;

		protected bool isApplied;
		// Start is called before the first frame update
		void Start() {
			pathCreator = transform.GetComponentInParent<PathCreator>();
			roadMeshCreator = transform.GetComponentInParent<RoadMeshCreator>();
			linePosTransform = transform.GetChild(0);
		}

		void OnEnable() {
			pathCreator = transform.GetComponentInParent<PathCreator>();
			roadMeshCreator = transform.GetComponentInParent<RoadMeshCreator>();
			linePosTransform = transform.GetChild(0);
		}

		void SetLine() {
			if (roadMeshCreator != null) {
				center = (float)Math.Ceiling((roadMeshCreator.lines - 1) / 2.0f);
				coeff = (linePosition - center) * (roadMeshCreator.lineWidth * 1.5f);
				linePosTransform.localPosition = new Vector3(coeff, linePosTransform.localPosition.y, linePosTransform.localPosition.z);
			}
		}

		void Update() {
			
#if UNITY_EDITOR
			if (pathCreator == null) {
				return;
			}
			transform.position = pathCreator.path.GetPointAtTime(timePath);
			transform.rotation = pathCreator.path.GetRotation(timePath);
#endif
			SetLine();
		}

		public virtual void ApplyEffect(Car car) {

		}
		public virtual void StopEffect(Car car) {

		}
	}
}
