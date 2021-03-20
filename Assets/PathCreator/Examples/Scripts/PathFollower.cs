using System;
using TMPro;
using UnityEngine;

namespace PathCreation.Examples
{
	public enum Position {
		Left,
		Center,
		Right
	}


    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public RoadMeshCreator roadMeshCreator;
        public EndOfPathInstruction endOfPathInstruction;
		public WheelDrive wheelsDrive;
        public float speed = 5;
		public float maxSpeed = 270;
        float distanceTravelled;
		public Position linePosition = Position.Center;
		public Transform linePosTransform;
		public Vector3 position;
        public void Start() {
            if (roadMeshCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
				roadMeshCreator.pathCreator.pathUpdated += OnPathChanged;
				SetPosition();
			}
        }

        void Update()
        {
            if (roadMeshCreator != null)
            {
                distanceTravelled += speed / 10.0f * Time.deltaTime;
                transform.position = roadMeshCreator.pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = roadMeshCreator.pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
				position = roadMeshCreator.pathCreator.path.GetPointAtDistance(distanceTravelled + 3.0f, endOfPathInstruction);
				Vector3 relativeVec = transform.InverseTransformPoint(position);
				Debug.DrawLine(transform.position, position, Color.green);
				if (wheelsDrive != null) {
					wheelsDrive.currentTorque = speed / maxSpeed;
					wheelsDrive.steerAngle = relativeVec.x / relativeVec.magnitude;
				}
			}
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = roadMeshCreator.pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

		void SetPosition() {
			float coeff = 0;
			switch (linePosition) {
                case Position.Left:
					coeff = -roadMeshCreator.roadWidth;
					break;
				case Position.Right:
					coeff = roadMeshCreator.roadWidth;
					break;
                default:
                    break;
            }

			if (linePosTransform != null) {
				linePosTransform.localPosition = new Vector3(coeff, 0.0f , 0.0f);
			}

		}

		public void SpeedUp(float value) {
			if (speed + value < 0) {
				speed = 0;
			}
			else if(speed + value < maxSpeed) {
				speed += value;
			}
			else {
				speed = maxSpeed;
			}
		}
    }
}