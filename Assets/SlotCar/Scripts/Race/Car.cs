using System;
using System.Collections;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race;
using DG.Tweening;
using PathCreation;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts {
	public enum ScrollZone {
		Red,
		Yellow,
		Green
	}

	public enum MoveLine {
		ToLeft,
		ToRight
	}

	public class Car : MonoBehaviour {
		public RoadMeshCreator roadHolder;
		public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Loop;
		public WheelDrive wheelsDrive;
		public int linePosition = 0;
		public Transform linePosTransform;
		public GameObject carPrefab;
		public GameObject crown;
		private EngineSound soundEngine;

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
		[Header("% max speed")]
		public float accelerationOnStart = 20.0f;
		public float accelerationOnStartTime = 2.0f;
		public float speedUpTime = 0.1f;
		private float maxSpeedCoeff;
		public RaceState state = RaceState.PAUSE;

		
		public float turboTime = 3.0f;
		protected float cameraDistance;
		protected int currentSavedSpeed;
		public UnityEvent turboEnd;
		protected bool isTurbo;
		protected ParticleSystem turboParticle;

		private float center;

		public void Awake() {
			soundEngine = GetComponent<EngineSound>();
			if (soundEngine != null) {
				soundEngine.maxRPMLimit = maxSpeed;
			}
			wheelsDrive = GetComponent<WheelDrive>();
		}

		public void Start() {
			maxSpeedCoeff = maxSpeed / 100.0f;
			if (roadHolder != null) {
				SetLine();
				SetCurrentPositionOnRoad();
			}
		}

		public virtual void Init(RoadMeshCreator road, int carId) {
			roadHolder = road;
			carPrefab = Instantiate(GameDataInstance.Instance.GetCar(carId).prefab, Vector3.zero, Quaternion.identity);
			linePosTransform = carPrefab.transform;
			linePosTransform.SetParent(transform);

			crown = Instantiate(crown, Vector3.zero, Quaternion.identity);
			crown.transform.SetParent(carPrefab.transform);
			crown.SetActive(false);

			center = (float)Math.Ceiling(GameInstance.Instance.raceManager.levelData.lines / 2.0f);
		}

		public void SetStateRace(RaceState settedState) {
			state = settedState;
			if (state == RaceState.RACE) {
				DOTween.To(() => (float)speed, x => speed = (int)x,
					accelerationOnStart * maxSpeedCoeff, accelerationOnStartTime);
			}
		}

		public void Update() {
			SetLine();
			SetCurrentPositionOnRoad();
			CheckCurrentLap();
			if (soundEngine != null) {
				soundEngine.engineCurrentRPM = speed;
			}
		}

		public virtual void SetCurrentPositionOnRoad() {
			if (roadHolder != null) {
				if (state == RaceState.RACE) {
					distanceTravelled += speed / 10.0f * Time.deltaTime;
				}
				transform.position = roadHolder.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
				transform.rotation = roadHolder.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
				Vector3 position = roadHolder.path.GetPointAtDistance(distanceTravelled + 3.0f, endOfPathInstruction);
				Vector3 relativeVec = transform.InverseTransformPoint(position);
				if (wheelsDrive != null) {
					wheelsDrive.currentTorque = (float)speed / maxSpeed;
					wheelsDrive.steerAngle = relativeVec.x / relativeVec.magnitude * 3.0f;
				}
			}
		}

		void CheckCurrentLap() {
			if (roadHolder != null) {
				int currentIndex = roadHolder.path.GetPointIndexAtTime(distanceTravelled, endOfPathInstruction);
				currentTravelled =
					roadHolder.path.GetTravelledDistance(distanceTravelled, EndOfPathInstruction.Stop);
				if (roadHolder.path.IsLastPointOnPath(currentIndex)) {
					currentLap++;
				}
			}
		}

		void SetLine() {
			if (roadHolder != null) {
				//int pos = (GameInstance.Instance.raceManager.levelData.lines % 2);
					float coeff = (linePosition - center) * (roadHolder.lineWidth * 1.5f);
				if (linePosTransform != null) {
					linePosTransform.localPosition = new Vector3(coeff, linePosTransform.localPosition.y, linePosTransform.localPosition.z);
				}
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
					OnIncrementSpeed(duration);
					DOTween.To(() => (float)speed, x => speed = (int)x,
						speed + value, duration);
				}
				else {
					speed += value;
				}
			}
			else {
				OnIncrementSpeed(duration);
				DOTween.To(() => (float)speed, x => speed = (int)x,
					maxSpeed, duration);
			}
		}

		public virtual void OnIncrementSpeed(float duration) {

		}

		public void StartTurbo() {
			currentSavedSpeed = speed;
			isTurbo = true;
			SpeedUp(maxSpeed, 1.0f);
			StartCoroutine(StartTurboInternal());
			if (turboParticle != null) {
				turboParticle.gameObject.SetActive(true);
				turboParticle.Play();
			} 
		}

		IEnumerator StartTurboInternal() {
			yield return new WaitForSeconds(turboTime);
			speed = currentSavedSpeed;
			turboEnd.Invoke();
			turboEnd.RemoveAllListeners();
			isTurbo = false;
			if (turboParticle != null) {
				turboParticle.Stop();
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

		public void Stop() {
			state = RaceState.PAUSE;
			currentSavedSpeed = speed;
			speed = 0;
		}

		public void MoveTo(MoveLine dir) {
			bool dirChanged = false;
			switch (dir) {
				case MoveLine.ToLeft:
					if (linePosition > 1) {
						linePosition--;
						dirChanged = true;
					}
					break;
				case MoveLine.ToRight:
					if (linePosition < roadHolder.lines) {
						linePosition++;
						dirChanged = true;
					}
					break;
			}
			if (dirChanged && state == RaceState.PAUSE) {
				state = RaceState.RACE;
				SpeedUp(currentSavedSpeed, 0.0f);
			}
		}
}
}