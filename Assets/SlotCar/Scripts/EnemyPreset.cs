using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eEnemyAiPreset {
	Ease,
	Normal,
	Hard
}
[System.Serializable]
public class EnemyPreset: MonoBehaviour {
	public int scrollFreqCounterMin = 2;
	public int scrollFreqCounterMax = 4;
	public int redChance = 65;
	public int yellowChance = 25;
	public int greenChance = 15;
}
