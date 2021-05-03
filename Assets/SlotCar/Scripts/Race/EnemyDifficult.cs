using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDifficult {
	public EnemyPreset[] easy;
	public EnemyPreset[] normal;
	public EnemyPreset[] hard;

	public static EnemyDifficult CreateFromJson(string json) {
		return JsonUtility.FromJson<EnemyDifficult>(json);
	}
}


