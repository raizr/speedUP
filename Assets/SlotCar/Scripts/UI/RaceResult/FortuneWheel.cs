using System;
using System.Linq;
using Assets.SlotCar;
using Assets.SlotCar.Scripts.Race;
using Cinemachine.Utility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.SlotCar.Scripts.UI.RaceResult {
	public class FortuneWheel : MonoBehaviour {
		public Button getButton;
		public Image slider;
		public TextMeshProUGUI multiplierOnButton;
		public TextMeshProUGUI moneyCounter;
		public int playerPosition;
		public float rotateDuration = 0.5f;
		public float[] sectorAngles = new float[] {
			29.0f,
			-14.0f,
			-54.0f
		};

		public float[] centerSectorAngles = new float[] {
			59.0f,
			6.0f,
			-34.0f,
			-73.0f
		};

		private float angleRight = -82.0f;
		private float angleLeft = 82.0f;
		private RectTransform sliderTransform;
		private TweenerCore<Quaternion, Vector3, QuaternionOptions> moveHandle;
		private int multiplier = 2;
		private static readonly Random random = new Random();
		public float localZ;
		private (int item, double weight)[] items;

		public void Awake() {
			sliderTransform = slider.GetComponent<RectTransform>();
			sliderTransform.localEulerAngles = new Vector3(0.0f, 0.0f, angleLeft);
			getButton.onClick.AddListener(GetReward);
		}

		public void Start() {
			items ??= new (int item, double weight)[] {
				(0, RewardSystem.Instance.rewards.rewardMultiplier[0]),
				(1, RewardSystem.Instance.rewards.rewardMultiplier[1]),
				(2, RewardSystem.Instance.rewards.rewardMultiplier[2]),
				(3, RewardSystem.Instance.rewards.rewardMultiplier[3])
			};
		}

		public void StartWheel() {
			moveHandle = sliderTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, angleRight), rotateDuration)
				.OnComplete(() => {
					moveHandle = sliderTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, angleLeft), rotateDuration)
						.OnComplete(StartWheel);
				});
		}

		public int Rand() {
			var totalWeight = items.Sum(p => p.weight);
			var targetWeight = (1 - random.NextDouble()) * totalWeight;

			int i = 0;
			while (i < items.Length && (targetWeight -= items[i].weight) > 0) {
				i++;
			}
			return items[i].item;
		}

		public void GetReward() {
			moveHandle.Kill();
			float angle = centerSectorAngles[Rand()];
			sliderTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, angle), rotateDuration)
				.OnComplete(() => {
					int currentReward = RewardSystem.Instance.GetLevelCompleteReward(playerPosition);
					RewardSystem.Instance.ApplyMultiplierReward(currentReward, multiplier);
					GameInstance.Instance.SwitchTo(GameState.Main);
				});
		}

		public void Update() {
			localZ = sliderTransform.localEulerAngles.z;
			if (sliderTransform.localEulerAngles.z > 180.0f) {
				localZ -= 360.0f;
			}
			multiplier = 2;
			for (int i = sectorAngles.Length - 1; i >= 0; i--) {
				if (localZ < sectorAngles[i]) {
					multiplier = i + 3;
					break;
				}
			}
		}

		public void OnGUI() {
			multiplierOnButton.text = "x<size=130%>" + multiplier.ToString();
			moneyCounter.text = RewardSystem.Instance.GetMultiplierReward(RewardSystem.Instance.GetLevelCompleteReward(playerPosition),
				multiplier).ToString();
		}
	}
}
