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
    public int amount;
    public int counter;
    public Vector3[] posiciones;
    public bool isBlocked;
    public GameObject blocker;

    // Use this for initialization
    void Start () {

        isBlocked = false;
        posiciones = new Vector3[amount];
        counter = 0;
        amount = 4;
        posiciones[3] = gameObject.transform.position + new Vector3(1,0,-2);
        posiciones[2] = gameObject.transform.position + new Vector3(0,0,-1);
        posiciones[1] = gameObject.transform.position + new Vector3(1,0,0);
        posiciones[0] = gameObject.transform.position + new Vector3(2,0,0);

        posiciones[3].y = 0.3859999f;
        posiciones[2].y = 0.3859999f;
        posiciones[1].y = 0.3859999f;
        posiciones[0].y = 0.3859999f;

        spawnPoint.y = 0.3859999f;

        villager = Resources.Load("VillagerObject") as GameObject;
        spawnTimer = 0f;
        spawnTime = 4f;
        alert = false;
        elGameLogic = GameObject.FindWithTag("GameLogic");
        elGameLogicScript = elGameLogic.GetComponent<GameLogicScript>();
        if (!elGameLogicScript._bases.Contains(gameObject)) {
            elGameLogicScript._bases.Add(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!elGameLogicScript.isPaused)
        {
            /*if (blocker == null)
             {
                 isBlocked = false;
             }
             else {
                 isBlocked = true;
             }*/

            if (alert && amount > 0 && !isBlocked)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTime <= spawnTimer)
                {
                    spawn(Random.Range(0, 1));
                    amount--;
                    spawnTimer = 0;
                }
            }
        }
	}

    void spawn(int tipo) {
        switch (tipo) {
            case 0: //VILLAGER
                GameObject villager2 = GameObject.Instantiate(villager, posiciones[counter], Quaternion.identity) as GameObject;
                villager2.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
                elGameLogicScript._villagers.Add(villager2);
                counter++;
                break;
            case 1: //SOLDADO
                GameObject soldier = GameObject.Instantiate(villager, posiciones[counter], Quaternion.identity) as GameObject;
                soldier.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
                elGameLogicScript._villagers.Add(soldier);
                counter++;
                break;
        }
    }
}
