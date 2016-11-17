using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerScript : MonoBehaviour {

    VisionRangeScript laVision;
    AttackRangeScript elAtaque;
    public bool moving = false;
    public Vector3 targetPosition;
    public float movementLinearSpeed;
    public bool isAlive = true;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float attackCounter;
    public bool canMove;

    VillagerMovement villagerMovement;
    VillagerAttack villagerAttack;
    private float attackTime;

    // Use this for initialization

    void Start()
    {
        health = 100;
        attack = 10;
        defense = 10;
        attackSpeed = 0.5f;
        laVision = GetComponentInChildren<VisionRangeScript>();
        elAtaque = GetComponentInChildren<AttackRangeScript>();
        villagerMovement = GetComponent<VillagerMovement>();
        villagerAttack = GetComponent<VillagerAttack>();
    }

    void Update ()
    {

        if (laVision.enemyInSight) {
            if (canMove)
            {
                villagerMovement.MoveTo(laVision.closestZombie.transform.position);
            }
        }
        if (elAtaque.enemyInRange)
        {
            canMove = false;
            villagerAttack.Attack(laVision.closestZombie);
            villagerMovement.moving = false;
            // AttackEnemy();
        }
        else {
            canMove = true;
        }
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
