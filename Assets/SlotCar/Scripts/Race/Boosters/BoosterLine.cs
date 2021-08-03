using System;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	[ExecuteInEditMode]
	public class BoosterLine : MonoBehaviour {
		public PathCreator pathCreator;
		public RoadMeshCreator roadMeshCreator;
		[Range(0.0f, 1.0f)]
		public float timePathStart;
		public float stepPath = 0.01f;
		public int linePosition = 0;
		private float center;
		public Transform linePosTransform;
		public float coeff;
		public int length;
		public Vector3[] positions;
		private LineRenderer lineRenderer;
		private int previousLength;
		void Start() {
			Init();
		}

		void OnEnable() {
			Init();
		}

		void Init() {
			pathCreator = transform.GetComponentInParent<PathCreator>();
			roadMeshCreator = transform.GetComponentInParent<RoadMeshCreator>();
			linePosTransform = transform.GetChild(0);
			lineRenderer = GetComponentInChildren<LineRenderer>();
 
			
		}
		void SetLine() {
			if (roadMeshCreator != null) {
				center = (float)Math.Ceiling((roadMeshCreator.lines - 1) / 2.0f);
				coeff = (linePosition - center) * (roadMeshCreator.lineWidth * 1.5f);
				//linePosTransform.localPosition = new Vector3(coeff, linePosTransform.localPosition.y, linePosTransform.localPosition.z);
			}
		}

		void Update() {
#if UNITY_EDITOR
			if (pathCreator == null) {
				return;
			}

			if (lineRenderer != null && previousLength != length) {
				if (positions == null) {
					positions = new Vector3[length];
				}
				else {
					Array.Resize(ref positions, length);
				}
				for (var j = 0; j < positions.Length; j++) {
					positions[j] = pathCreator.path.GetPointAtTime(timePathStart + j * stepPath);
				}

				lineRenderer.positionCount = length;
				lineRenderer.SetPositions(positions);

				Mesh mesh = new Mesh();
				lineRenderer.BakeMesh(mesh, true);
				MeshCollider meshCollider = GetComponentInChildren<MeshCollider>();
				if (meshCollider != null && IsMeshOk(mesh)) {
					meshCollider.sharedMesh = mesh;
				}
			}
#endif
		}

		static bool IsMeshOk(Mesh m) {
			foreach (Vector3 v in m.vertices) {
				if (!IsFinite(v.x) || !IsFinite(v.y) || !IsFinite(v.z)) {
					return false;
				}
			}
			return true;
		}

		static bool IsFinite(float f) {
			return !float.IsInfinity(f) && !float.IsNaN(f);
		}

		public virtual void ApplyEffect(Car car) {
		}

		public virtual void StopEffect(Car car) {

		}
	}
}
