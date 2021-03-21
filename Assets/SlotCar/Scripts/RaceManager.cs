using System.Collections;
using System.Collections.Generic;
using Assets.SlotCar.Scripts;
using UnityEngine;

public class RaceManager : MonoBehaviour {
	public List<EnemyCar> enemyCars;
	public List<EnemyPreset> enemyPreset;
	public TextAsset jsonFile;
    public EnemyDifficult difficultData;
    // Start is called before the first frame update
    void Awake()
    {
		difficultData = EnemyDifficult.CreateFromJson(jsonFile.text);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
