using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public GameObject damageTextPopUp;
	public void ShowPopup(int points, int multplier) {
		GameObject floatingText = Instantiate(damageTextPopUp, this.transform, false);
		TextPopUp popUp = floatingText.GetComponent<TextPopUp>();
		if (popUp) {
			popUp.Show(points, multplier);
		}
	}
}
