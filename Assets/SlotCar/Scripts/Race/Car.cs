using System;
using System.Collections;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race;
using DG.Tweening;
using PathCreation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

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
		public float turnTime = 5f;
		public Transform linePosTransform;
		public GameObject carPrefab;
		public GameObject crown;
		protected EngineSound soundEngine;
		public Rigidbody rigidbody;

		public float speed = 0;
		public int maxSpeed = 270;

		[Header("% max speed"), Range(-100.0f, 100.0f)]
		public float redZoneSpeed = 10.0f;

		[Header("% max speed"), Range(-100.0f, 100.0f)]
		public float greenZoneSpeed = 25.0f;

		[Header("% max speed"), Range(-100.0f, 100.0f)]
		public float yellowZoneSpeed = 15.0f;

		public float distanceTravelled;
		public int currentLap = 1;
		public float currentTravelled;
		[Header("% max speed")] public float accelerationOnStart = 20.0f;
		public float accelerationOnStartTime = 2.0f;
		public float speedUpTime = 0.1f;
		[Range(-100.0f, 100.0f)] public float staticSpeedUp = 70.0f;
		[Range(-100.0f, 100.0f)] public float staticSpeedDown = 40.0f;
		private float maxSpeedCoeff;
		public RaceState raceState = RaceState.PAUSE;


		public float turboTime = 3.0f;
		protected float cameraDistance;
		protected float currentSavedSpeed;
		public UnityEvent turboEnd;
		protected bool isTurbo;
		protected ParticleSystem turboParticle;
		[SerializeField]
		protected bool isFollowLine;

		private float center;
		private bool isStaticSpeedUp;
		private bool isStaticSpeedDown;

		private float turnPosition;
		private bool isTurn = false;

		public void Awake() {
			soundEngine = GetComponent<EngineSound>();
			if (soundEngine != null) {
				soundEngine.maxRPMLimit = maxSpeed;
			}

			wheelsDrive = GetComponent<WheelDrive>();
		}

		public void Start() {
			turnPosition = linePosition;
			maxSpeedCoeff = maxSpeed / 100.0f;
			isFollowLine = true;
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
			rigidbody = GetComponent<Rigidbody>();
			crown = Instantiate(crown, Vector3.zero, Quaternion.identity);
			crown.transform.SetParent(carPrefab.transform);
			crown.SetActive(false);

			center = (float) Math.Ceiling(GameInstance.Instance.raceManager.levelData.lines / 2.0f);
		}

		public void SetStateRace(RaceState settedState) {
			raceState = settedState;
			if (raceState == RaceState.RACE) {
				DOTween.To(() => (float) speed, x => speed = (int) x,
					accelerationOnStart * maxSpeedCoeff, accelerationOnStartTime);
			}
		}

		public void SetFollowLine(bool followState) {
			//if (raceState == RaceState.RACE) {
				isFollowLine = followState;
			//}
		}

		

		void OnCollisionEnter(Collision collision) {
			if (raceState != RaceState.RACE) {
				return;
			}
			EnemyCar anotherCar = collision.collider.GetComponentInParent<EnemyCar>();
			if (anotherCar) {
				Debug.Log("OnCollisionEnter");
				int dir = (anotherCar.linePosition < turnPosition) ? 1 : -1;
				if (anotherCar.push == 1) {
					anotherCar.SetFollowLine(false);
					anotherCar.rigidbody.AddForce(new Vector3(0, 10, 10 * dir), ForceMode.Impulse);
				}
				else{
					--anotherCar.push;
					isTurn = false;
					if (dir < 0)
                    {
						MoveTo(MoveLine.ToLeft);
					}
					else
                    {
						MoveTo(MoveLine.ToRight);
					}

				}

			}
			foreach (ContactPoint contact in collision.contacts)
			{
				Debug.DrawRay(contact.point, contact.normal * 10.0f, Color.white);
			}
		}

		void OnCollisionExit(Collision collision) {
			Car anotherCar = collision.collider.GetComponentInParent<EnemyCar>();
			if (anotherCar) {
				//anotherCar.SetFollowLine(true);
			}
		}

		public void Update() {
			SetLine();
			CheckStaticSpeed();
			SetCurrentPositionOnRoad();
			CheckCurrentLap();
			if (soundEngine != null) {
				soundEngine.engineCurrentRPM = speed;
			}
		}

		public virtual void SetCurrentPositionOnRoad() {
			if (roadHolder != null && isFollowLine) {
				if (raceState == RaceState.RACE) {
					distanceTravelled += speed / 10.0f * Time.deltaTime;
				}
				transform.position = roadHolder.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
				transform.rotation = roadHolder.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
				Vector3 position = roadHolder.path.GetPointAtDistance(distanceTravelled + 3.0f, endOfPathInstruction);
				Vector3 relativeVec = transform.InverseTransformPoint(position);
				if (wheelsDrive != null) {
					wheelsDrive.currentTorque = (float) speed / maxSpeed;
					wheelsDrive.steerAngle = relativeVec.x / relativeVec.magnitude * 3.0f;
				}
			}
		}

		protected void CheckCurrentLap() {
			if (roadHolder != null) {
				int currentIndex = roadHolder.path.GetPointIndexAtTime(distanceTravelled, endOfPathInstruction);
				currentTravelled =
					roadHolder.path.GetTravelledDistance(distanceTravelled, EndOfPathInstruction.Stop);
				if (roadHolder.path.IsLastPointOnPath(currentIndex)) {
					currentLap++;
				}
			}
		}

		protected void SetLine() {
			if (roadHolder != null) {
				//int pos = (GameInstance.Instance.raceManager.levelData.lines % 2);
				float coeff = (turnPosition - center) * (roadHolder.lineWidth * 1.5f);
				if (linePosTransform != null) {
					linePosTransform.localPosition = new Vector3(coeff, linePosTransform.localPosition.y,
						linePosTransform.localPosition.z);
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
			SpeedUp(speed, speedUpTime);
		}

		public void SpeedUp(float value, float duration) {
			if (isStaticSpeedUp || isStaticSpeedDown) {
				return;
			}

			if (speed + value < 0) {
				speed = 0;
			}
			else if (speed + value < maxSpeed) {
				if (value > 0) {
					OnIncrementSpeed(duration);
					DOTween.To(() => (float) speed, x => speed = (int) x,
						speed + value, duration);
				}
				else {
					speed += value;
				}
			}
			else {
				OnIncrementSpeed(duration);
				DOTween.To(() => (float) speed, x => speed = (int) x,
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

		public virtual void Stop() {
			raceState = RaceState.PAUSE;
			currentSavedSpeed = speed;
			speed = 0;
			SetFollowLine(false);
			rigidbody.AddForce(new Vector3(-20, 50, 0), ForceMode.Impulse);
		}

		public virtual void Restart()
		{
			SetFollowLine(true);
		}

			public void SetupStaticSpeedUp() {
			isStaticSpeedUp = true;
			currentSavedSpeed = speed;
		}

		public void SetupStaticSpeedDown() {
			isStaticSpeedDown = true;
			currentSavedSpeed = speed;
		}

		public void CheckStaticSpeed() {
			if (isStaticSpeedUp) {
				speed = (int) (staticSpeedUp * maxSpeedCoeff);
			}
			if (isStaticSpeedDown) {
				speed = (int) (staticSpeedDown * maxSpeedCoeff);
			}
		}

		public void DisableStaticSpeedUp() {
			if (isStaticSpeedUp) {
				isStaticSpeedUp = false;
				speed = currentSavedSpeed;
			}
		}

		public void DisableStaticSpeedDown() {
			if (isStaticSpeedDown) {
				isStaticSpeedDown = false;
				speed = currentSavedSpeed;
			}
		}

		public void MoveTo(MoveLine dir) {
			bool dirChanged = false;
			switch (dir) {
				case MoveLine.ToLeft:
					if (linePosition > 1) {
						linePosition--;
						dirChanged = true;
						StartCoroutine(TurnLeft());
					}
					break;
				case MoveLine.ToRight:
					if (linePosition < roadHolder.lines) {
						linePosition++;
						dirChanged = true;
						StartCoroutine(TurnRight());
					}
					break;
			}
			if (dirChanged && raceState == RaceState.PAUSE) {
				raceState = RaceState.RACE;
				SpeedUp(currentSavedSpeed, 0.0f);
			}
		}

		IEnumerator TurnRight()
		{
			isTurn = true;
			yield return new WaitForFixedUpdate();

			float t = linePosition - 1;
			while (t < linePosition && isTurn)
			{
				t += Time.deltaTime* turnTime;
				turnPosition = t;
				Debug.Log("TurnRight " + turnPosition);
				yield return null;
			}
		}
		IEnumerator TurnLeft()
		{
			isTurn = true;
			yield return new WaitForFixedUpdate();

			float t = linePosition + 1;
			while (t > linePosition && isTurn)
			{
				t -= Time.deltaTime * turnTime;
				turnPosition = t;
				Debug.Log("TurnLeft " + turnPosition);
				yield return null;
			}
		}
	}
}