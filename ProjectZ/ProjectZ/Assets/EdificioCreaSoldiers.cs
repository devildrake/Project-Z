using UnityEngine;
using System.Collections;

public class EdificioCreaSoldiers : MonoBehaviour {
    GameObject elGameLogic;
    GameObject villager;
    public bool alert;
    public float spawnTimer;
    public float spawnTime;
    public GameLogicScript elGameLogicScript;
    public GameObject spawnPointObject;
    public Vector3 spawnPoint;
    public int cuantosSoldados;

    // Use this for initialization
    void Start () {
        spawnPoint = spawnPointObject.transform.position;
        spawnPoint.y = 0.3859999f;
        villager = Resources.Load("VillagerObject") as GameObject;
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
                spawn(Random.Range(0, 1));
                spawnTimer = 0;
            }
        }


	}

    void spawn(int tipo) {
        switch (tipo) {
            case 0: //VILLAGER
                GameObject villager2 = GameObject.Instantiate(villager, spawnPoint, Quaternion.identity) as GameObject;
                villager2.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
                break;
            case 1: //SOLDADO
                GameObject soldier = GameObject.Instantiate(villager, spawnPoint, Quaternion.identity) as GameObject;
                soldier.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
                break;
        }
    }
}
