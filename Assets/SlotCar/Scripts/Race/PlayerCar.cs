using System.Collections;
using Cinemachine;
using DG.Tweening;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race {
	public class PlayerCar : Car {
		public int speedDownCoeff = 1;
		[Header("Seconds")]
		public float timeSpeedDown = 1.0f;
		public CinemachineVirtualCamera camera;
		public float greenZoneSpeedUpTime = 1.0f;
		public float cameraShakeIntensity = 5.0f;
		public float cameraShakeTime = 0.5f;

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
			camera = GameInstance.Instance.GetCamera();
			if (camera) {
				var transposer = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
				if (transposer) {
					cameraDistance = transposer.m_CameraDistance;
				}
			}
			StartCoroutine(SpeedTick());
		}

		new void Update() {
			SetLine();
			CheckStaticSpeed();
			SetCurrentPositionOnRoad();
			CheckCurrentLap();
			if (soundEngine != null) {
				soundEngine.engineCurrentRPM = speed;
			}
		}

		IEnumerator SpeedTick() {
			if (raceState != RaceState.RACE) {
				yield return null;
			}
			yield return new WaitForSeconds(timeSpeedDown);
			while (true) {
				if (raceState != RaceState.RACE) {
					yield return null;
				}
				else {
					if (!isTurbo) {
						SpeedUp(speedDownCoeff, speedUpTime);
					}
					yield return new WaitForSeconds(timeSpeedDown);
				}
			}
		}

		public void SpeedUpByGreenZone() {
			float zoneSpeed = ChooseSpeedByZone(ScrollZone.Green);
			SpeedUp((int) zoneSpeed, greenZoneSpeedUpTime);
		}

		public override void OnIncrementSpeed(float duration) {
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

		public override void Stop() {
			CameraShake(cameraShakeIntensity, cameraShakeTime);
			base.Stop();
			camera.m_Follow = null;
		}

		public override void Restart()
		{
			base.Restart();
			camera.m_Follow = linePosTransform;
		}

		public void CameraShake(float intensity, float time)
		{
			CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
				camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

			cameraShakeTime = time;
			StartCoroutine(CameraShaking());
		}

		IEnumerator CameraShaking()
        {
			yield return new WaitForSeconds(cameraShakeTime);
			CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
				camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
		}
	}
}
