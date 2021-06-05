using Assets.SlotCar.Scripts.Data;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Assets.SlotCar.Scripts.UI {
	public class FollowTarget : MonoBehaviour {

		public TextMeshProUGUI text;
		public Image image;
		public Transform lookAt;
		public Vector3 offset = new Vector3(0.0f, 1.0f, -1.0f);

		public void GenerateFlag() {
			var flags = GameDataInstance.Instance.flags;
			int index = Random.Range(0, flags.Length);
			image.sprite = GameDataInstance.Instance.flags[index];
		}

		void Update() {
			text.transform.position = lookAt.position + offset;
		}
	}
}
