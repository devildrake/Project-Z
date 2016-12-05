using UnityEngine;
using System.Collections;

public class EdificioCreaSoldiers : MonoBehaviour {
    GameObject elGameLogic;

    public bool alert;
    public float spawnTimer;
    public float spawnTime;
    public GameLogicScript elGameLogicScript;
    GameObject villager = Resources.Load("VillagerObject") as GameObject;
    public int cuantosSoldados;

    // Use this for initialization
    void Start () {
        spawnTimer = 0f;
        spawnTime = 2f;
        alert = false;
        elGameLogic = GameObject.FindWithTag("GameLogic");
        elGameLogicScript = elGameLogic.GetComponent<GameLogicScript>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (alert) {
            spawnTimer += Time.deltaTime;
            if (spawnTime <= spawnTimer) {
                spawnTimer = 0;
            }
        }


	}

    void SpawnSoldier() {
        
    }

    void SpawnVillager() {

    }
}
