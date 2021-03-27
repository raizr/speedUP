using System;
using Assets.SlotCar.Scripts;
using PathCreation;
using PathCreation.Examples;
using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public enum Position {
		Left,
		Center,
		Right
	}
	
	public enum ScrollZone {
		Red,
		Yellow,
		Green
	}

// Moves along a path at constant speed.
	// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
	public class PathFollower : MonoBehaviour
    {
        public RoadMeshCreator roadMeshCreator;
        public EndOfPathInstruction endOfPathInstruction;
		public WheelDrive wheelsDrive;
		public Position linePosition = Position.Center;
		public Transform linePosTransform;
		public GameObject crown;

		public int speed = 5;
		public int maxSpeed = 270;
		[Header("% max speed")]
		public float redZoneSpeed = 10.0f;
		[Header("% max speed")]
		public float greenZoneSpeed = 25.0f;
		[Header("% max speed")]
		public float yellowZoneSpeed = 15.0f;
		
		public float distanceTravelled;
		public int currentLap = 1;
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
				Vector3 position = roadMeshCreator.pathCreator.path.GetPointAtDistance(distanceTravelled + 3.0f, endOfPathInstruction);
				Vector3 relativeVec = transform.InverseTransformPoint(position);
				if (wheelsDrive != null) {
					wheelsDrive.currentTorque = (float)speed / maxSpeed;
					wheelsDrive.steerAngle = relativeVec.x / relativeVec.magnitude * 3.0f;
				}
			}

			CheckCurrentLap();
		}

		void CheckCurrentLap() {
			currentLap = roadMeshCreator.pathCreator.path.GetPointIndexAtTime(distanceTravelled, endOfPathInstruction);
			/*if (roadMeshCreator.pathCreator.path.IsLastPointOnPath(currentIndex)) {
				currentLap++;
			}*/
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
				linePosTransform.localPosition = new Vector3(coeff, linePosTransform.localPosition.y, linePosTransform.localPosition.z);
			}

		}
		public void SpeedUp(ScrollZone zone) {
			float speed = 0.0f;
			switch (zone) {
				case ScrollZone.Yellow:
					speed = maxSpeed / yellowZoneSpeed;
					break;
				case ScrollZone.Green:
					speed = maxSpeed / greenZoneSpeed;
					break;
				default:
					speed = maxSpeed / redZoneSpeed;
					break;
			}
			SpeedUp((int)speed);
		}

		public void SpeedUp(int value) {
			if (speed + value < 0) {
				speed = 0;
			}
			else if (speed + value < maxSpeed) {
				speed += value;
			}
			else {
				speed = maxSpeed;
			}
		}

		public void SetFirstPos(bool status) {
			crown.SetActive(status);
		}
}
}