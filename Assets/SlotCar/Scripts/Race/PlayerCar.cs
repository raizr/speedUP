using System.Collections;
using Cinemachine;
using DG.Tweening;
using PathCreation;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts.Race {
	public class PlayerCar : PathFollower {
		public int speedDownCoeff = 1;
		[Header("Seconds")]
		public float timeSpeedDown = 1.0f;
		public CinemachineVirtualCamera camera;
		public float greenZoneSpeedUpTime = 1.0f;
		public float turboTime = 3.0f;
		private float cameraDistance;
		private int currentSavedSpeed;
		public UnityEvent turboEnd;
		private bool isTurbo;
		private ParticleSystem turboParticle;

		new void Awake() {
			linePosition = 2;
			base.Awake();
		}

		public void Init(RoadMeshCreator road, int carId, int position) {
			base.Init(road, carId);
			if (position > 0) {
				linePosition = position;
			}
			turboParticle = GetComponentInChildren<ParticleSystem>(true);
		}

		// Start is called before the first frame update
		new void Start() {
			base.Start();
			camera = GameInstance.Instance.camera;
			if (camera) {
				var transposer = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
				cameraDistance = transposer.m_CameraDistance;
			}
			StartCoroutine(SpeedTick());
		}

		IEnumerator SpeedTick() {
			if (state != RaceState.RACE) {
				yield return null;
			}
			yield return new WaitForSeconds(timeSpeedDown);
			while (true) {
				if (!isTurbo) {
					SpeedUp(-speedDownCoeff, speedUpTime);
				}
				yield return new WaitForSeconds(timeSpeedDown);
			}
		}

		public void SpeedUpByGreenZone() {
			float zoneSpeed = ChooseSpeedByZone(ScrollZone.Green);
			SpeedUp((int) zoneSpeed, greenZoneSpeedUpTime);
		}

		override public void OnIncrementSpeed(float duration) {
			if (camera == null) {
				return;
			}
			var transposer = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
			DOTween.To(() => transposer.m_CameraDistance,
					x => transposer.m_CameraDistance = x,
					cameraDistance + 2, 0.7f)
				.OnComplete(() => {
					DOTween.To(() => transposer.m_CameraDistance,
						x => transposer.m_CameraDistance = x,
						cameraDistance, 0.3f);
				});
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
	}
}
