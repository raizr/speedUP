using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation {
    /// Stores state data for the path creator editor

    [System.Serializable]
    public class PathCreatorData {
        public event System.Action bezierOrVertexPathModified;
        public event System.Action bezierCreated;

        [SerializeField]
        BezierPath _bezierPath;
        VertexPath _vertexPath;

        [SerializeField]
        bool vertexPathUpToDate;

        // vertex path settings
        public float vertexPathMaxAngleError = .3f;
        public float vertexPathMinVertexSpacing = 0.01f;

        // bezier display settings
        public bool showTransformTool = false;
        public bool showPathBounds;
        public bool showPerSegmentBounds;
        public bool displayAnchorPoints = true;
        public bool displayControlPoints = true;
        public float bezierHandleScale = 1;
        public bool globalDisplaySettingsFoldout;
        public bool keepConstantHandleSize;

        // vertex display settings
        public bool showNormalsInVertexMode;
        public bool showBezierPathInVertexMode;

        // Editor display states
        public bool showDisplayOptions;
        public bool showPathOptions = true;
        public bool showVertexPathDisplayOptions;
        public bool showVertexPathOptions = true;
        public bool showNormals;
        public bool showNormalsHelpInfo;
        public int tabIndex;

        //Editor spiral
		public float spiralA = 1.0f;
		public float spiralB = 1.0f;
		public float Count = 6.0f;
		public Vector3 lineLength = Vector3.right;
        public void Initialize (bool defaultIs2D) {
            if (_bezierPath == null) {
                CreateBezier (Vector3.zero, defaultIs2D);
            }
            vertexPathUpToDate = false;
            _bezierPath.OnModified -= BezierPathEdited;
            _bezierPath.OnModified += BezierPathEdited;
        }

        public void ResetBezierPath (Vector3 centre, bool defaultIs2D = false) {
            CreateBezier (centre, defaultIs2D);
        }

        void CreateBezier (Vector3 centre, bool defaultIs2D = false) {
            if (_bezierPath != null) {
                _bezierPath.OnModified -= BezierPathEdited;
            }

            var space = (defaultIs2D) ? PathSpace.xy : PathSpace.xyz;
            _bezierPath = new BezierPath (centre, false, space);

            _bezierPath.OnModified += BezierPathEdited;
            vertexPathUpToDate = false;

            if (bezierOrVertexPathModified != null) {
                bezierOrVertexPathModified ();
            }
            if (bezierCreated != null) {
                bezierCreated ();
            }
        }

		public void CreateSpiral() {
			if (_bezierPath == null) {
				return;
			}

			var point = _bezierPath.GetPoint(_bezierPath.NumPoints - 1) + new Vector3(spiralA / 2.0f, spiralA / 2.0f, 0.0f);
            for (float t = 0; t < 6; t += 0.5f) {
				Vector3 pos = new Vector3(
					spiralA * Mathf.Sin(t),
					spiralB * t,
                    spiralA * Mathf.Cos(t)
                );
				pos.x += point.x;
				pos.y += point.y;
                _bezierPath.AddSegmentToEnd(pos);
            }
			if (bezierOrVertexPathModified != null) {
				bezierOrVertexPathModified();
			}
			if (bezierCreated != null) {
				bezierCreated();
			}
		}

		public void CreateLine() {
			if (_bezierPath == null) {
				return;
			}

			var point = _bezierPath.GetPoint(_bezierPath.NumPoints - 1) + lineLength;
			_bezierPath.AddSegmentToEnd(point);
			if (bezierOrVertexPathModified != null) {
				bezierOrVertexPathModified();
			}
			if (bezierCreated != null) {
				bezierCreated();
			}

		}

        public BezierPath bezierPath {
            get {
                return _bezierPath;
            }
            set {
                _bezierPath.OnModified -= BezierPathEdited;
                vertexPathUpToDate = false;
                _bezierPath = value;
                _bezierPath.OnModified += BezierPathEdited;

                if (bezierOrVertexPathModified != null) {
                    bezierOrVertexPathModified ();
                }
                if (bezierCreated != null) {
                    bezierCreated ();
                }
            }
        }

        // Get the current vertex path
        public VertexPath GetVertexPath (Transform transform) {
            // create new vertex path if path was modified since this vertex path was created
            if (!vertexPathUpToDate || _vertexPath == null) {
                vertexPathUpToDate = true;
                _vertexPath = new VertexPath (bezierPath, transform, vertexPathMaxAngleError, vertexPathMinVertexSpacing);
            }
            return _vertexPath;
        }

        public void PathTransformed () {
            if (bezierOrVertexPathModified != null) {
                bezierOrVertexPathModified ();
            }
        }

        public void VertexPathSettingsChanged () {
            vertexPathUpToDate = false;
            if (bezierOrVertexPathModified != null) {
                bezierOrVertexPathModified ();
            }
        }

        public void PathModifiedByUndo () {
            vertexPathUpToDate = false;
            if (bezierOrVertexPathModified != null) {
                bezierOrVertexPathModified ();
            }
        }

        void BezierPathEdited () {
            vertexPathUpToDate = false;
            if (bezierOrVertexPathModified != null) {
                bezierOrVertexPathModified ();
            }
        }

    }
}