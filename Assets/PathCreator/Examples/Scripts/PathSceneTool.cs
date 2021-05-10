using UnityEngine;

namespace PathCreation.Examples {
    [ExecuteInEditMode]
    public class PathSceneTool : MonoBehaviour
    {
        public event System.Action onDestroyed;
        public PathCreator pathCreator;
        public bool autoUpdate = true;
		public RoadMeshCreator roadMeshCreator;
		protected VertexPath path {
			get {
				return pathCreator.path;
			}
		}

        public void TriggerUpdate() {
			if (roadMeshCreator != null) {
				roadMeshCreator.pathChanged = true;
				roadMeshCreator.path = path;
				roadMeshCreator.PathUpdated();
			}
            PathUpdated();
        }


        protected virtual void OnDestroy() {
            if (onDestroyed != null) {
                onDestroyed();
            }
        }

		protected virtual void PathUpdated() {

		}
    }
}
