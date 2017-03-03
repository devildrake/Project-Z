using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1Script : MonoBehaviour {
    public  GameLogicScript gameLogic;
    public GameObject spawner;
    public bool[] hasHappened;
    public GameObject[] objetosZona = new GameObject[3];
    // Use this for initialization
    void Start() {
        gameLogic = GameLogicScript.gameLogic;
        spawner = GameObject.FindGameObjectWithTag("SpecialSpawner");
        hasHappened = new bool[10];


    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
        }
        if (objetosZona[0] != null && objetosZona[1] != null && objetosZona[2] != null) { 
            if (objetosZona[0].GetComponent<ZonaTutorial>().steppedOn && objetosZona[1].GetComponent<ZonaTutorial>().steppedOn && objetosZona[2].GetComponent<ZonaTutorial>().steppedOn)
            {
                gameLogic.eventManager.activateEvent(1);

                if (gameLogic.eventos[1] && !hasHappened[1] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
                {
                    if (gameLogic != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            objetosZona[i].GetComponent<ZonaTutorial>().DestroyThis();
                        }

                        Debug.Log(gameLogic.gameObject);
                        gameLogic.SpawnVillager(spawner.GetComponent<EdificioCreaSoldiers>().spawnPointObject.transform.position);
                        gameLogic.SpawnWalker(new Vector3(2.5f, 0.0249f, -9.5f));
                        gameLogic.SpawnWalker(new Vector3(6f, 0.0249f, -9.5f));
                        hasHappened[1] = true;
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            objetosZona[i].GetComponent<ZonaTutorial>().DestroyThis();
                        }
                        gameLogic = GameLogicScript.gameLogic;
                        gameLogic.SpawnVillager(spawner.GetComponent<EdificioCreaSoldiers>().spawnPointObject.transform.position);
                        gameLogic.SpawnWalker(new Vector3(2.5f, 0.0249f, -9.5f));
                        gameLogic.SpawnWalker(new Vector3(6f, 0.0249f, -9.5f));
                        hasHappened[1] = true;
                    }
                }
            }
    }
        if (gameLogic.eventos[1] && !gameLogic.eventos[2] && gameLogic._villagers.Count == 0)
        {
            gameLogic.eventManager.activateEvent(2);
            Debug.Log("Event2Activated"); }
            Debug.Log("This should be true" + gameLogic.eventManager.eventList[2].hasHappened);
            Debug.Log("This should be true" + !hasHappened[2]);



            if (gameLogic.eventManager.eventList[2].hasHappened&& !hasHappened[2] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
            {
                Debug.Log("SpawnSoldierPls");
                gameLogic.SpawnSoldier(spawner.GetComponent<EdificioCreaSoldiers>().spawnPointObject.transform.position);
                hasHappened[2] = true;
            }
        
        for (int i = 0; i < 10; i++)
        {
            hasHappened[i] = gameLogic.eventos[i];
        }
    }
}
