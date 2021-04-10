using DG.Tweening;
using PathCreation;
using PathCreation.Examples;
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

		public int speed = 0;
		public int maxSpeed = 270;
		[Header("% max speed")]
		public float redZoneSpeed = 10.0f;
		[Header("% max speed")]
		public float greenZoneSpeed = 25.0f;
		[Header("% max speed")]
		public float yellowZoneSpeed = 15.0f;
		
		public float distanceTravelled;
		public int currentLap = 1;
		public float currentTravelled;

		public float accelerationOnStart = 20.0f;
		public float accelerationOnStartTime = 2.0f;
		public float speedUpTime = 0.1f;
		private float maxSpeedCoeff;
		public RaceState state = RaceState.PAUSE;

		public void Start() {
			maxSpeedCoeff = maxSpeed / 100.0f;
			if (roadMeshCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
				roadMeshCreator.pathCreator.pathUpdated += OnPathChanged;
				SetPosition();
				SetCurrentPositionOnRoad();
			}
		}

		public void SetStateRace(RaceState settedState) {
			state = settedState;
			if (state == RaceState.RACE) {
				DOTween.To(() => (float)speed, x => speed = (int)x,
					accelerationOnStart * maxSpeedCoeff, accelerationOnStartTime);
			}
		}

		void Update() {
				SetCurrentPositionOnRoad();
				CheckCurrentLap();
		}

		void SetCurrentPositionOnRoad() {
			if (roadMeshCreator != null) {
				if (state == RaceState.RACE) {
					distanceTravelled += speed / 10.0f * Time.deltaTime;
				}
				transform.position = roadMeshCreator.pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
				transform.rotation = roadMeshCreator.pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
				Vector3 position = roadMeshCreator.pathCreator.path.GetPointAtDistance(distanceTravelled + 3.0f, endOfPathInstruction);
				Vector3 relativeVec = transform.InverseTransformPoint(position);
				if (wheelsDrive != null) {
					wheelsDrive.currentTorque = (float)speed / maxSpeed;
					wheelsDrive.steerAngle = relativeVec.x / relativeVec.magnitude * 3.0f;
				}
			}
		}

		void CheckCurrentLap() {
			int currentIndex = roadMeshCreator.pathCreator.path.GetPointIndexAtTime(distanceTravelled, endOfPathInstruction);
			currentTravelled = roadMeshCreator.pathCreator.path.GetTravelledDistance(distanceTravelled, EndOfPathInstruction.Stop);
			if (roadMeshCreator.pathCreator.path.IsLastPointOnPath(currentIndex)) {
				currentLap++;
			}
		}

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

		protected float ChooseSpeedByZone(ScrollZone zone) {
			float speed = 0.0f;
			switch (zone) {
				case ScrollZone.Yellow:
					speed = maxSpeedCoeff * yellowZoneSpeed;
					break;
				case ScrollZone.Green:
					speed = maxSpeedCoeff * greenZoneSpeed;
					break;
				default:
					speed = maxSpeedCoeff * redZoneSpeed;
					break;
			}
			return speed;
		}
		public void SpeedUp(ScrollZone zone) {
			float speed = ChooseSpeedByZone(zone);
			SpeedUp((int)speed, speedUpTime);
		}

		public void SpeedUp(int value, float duration) {
			if (speed + value < 0) {
				speed = 0;
			}
			else if (speed + value < maxSpeed) {
				if (value > 0) {
					DOTween.To(() => (float)speed, x => speed = (int)x,
						speed + value, duration);
				}
				else {
					speed += value;
				}
			}
			else {
				speed = maxSpeed;
			}
		}

		public void SetFirstPos(bool status) {
			crown.SetActive(status);
		}

		public void ResetValues() {
			distanceTravelled = 0.0f;
			currentTravelled = 0.0f;
			currentLap = 0;
			speed = 0;
			SetCurrentPositionOnRoad();
		}
}
}