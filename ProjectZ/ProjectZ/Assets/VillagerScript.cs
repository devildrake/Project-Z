using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerScript : MonoBehaviour {

    VisionRangeScript laVision;
    AttackRangeScript elAtaque;

    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float attackCounter;

    // Use this for initialization


    void Start()
    {


        health = 100;
        attack = 10;
        defense = 10;
        attackSpeed = 0.5f;
        laVision = GetComponentInChildren<VisionRangeScript>();
        elAtaque = GetComponentInChildren<AttackRangeScript>();
    }

    void Update ()
    { 

        if (laVision.enemyInSight) {
            Debug.Log("Can see some");
     

        }
        if (elAtaque.enemyInRange) {
            Debug.Log("Should Attack");
           // AttackEnemy();
        }
	}

    void ApproachEnemy() {

    }

    void AttackEnemy(GameObject enemy)
    {
        attackCounter += Time.deltaTime;
        if (attackCounter >= attackSpeed)
        {
            ZombieScript enemyScript = enemy.GetComponent<ZombieScript>();
            enemyScript.health -= attack;
            Debug.Log("I attacked");
            attackCounter = 0;
        }
    }
}
