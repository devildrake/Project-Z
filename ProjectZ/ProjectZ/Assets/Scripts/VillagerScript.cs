using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerScript : MonoBehaviour {
    public enum humanClass { villager, soldier }
    VisionRangeScript laVision;
    AttackRangeScript elAtaque;
    public bool moving = false;
    public Vector3 targetPosition;
    public float movementLinearSpeed;
    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float movSpeed;
    public float attackSpeed;
    public float theAttackRange;
    bool confirmAlive;
    public humanClass tipo;
    public bool canMove;

    VillagerMovement villagerMovement;
    VillagerAttack villagerAttack;
    private float attackTime;

    // Use this for initialization

    void Start()
    {
        confirmAlive = isAlive = true;
        laVision = GetComponentInChildren<VisionRangeScript>();
        elAtaque = GetComponentInChildren<AttackRangeScript>();
        villagerMovement = GetComponent<VillagerMovement>();
        villagerAttack = GetComponent<VillagerAttack>();

        switch (tipo){
            case humanClass.villager:
                theAttackRange = 1;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = 1;
                break;
            case humanClass.soldier:
                theAttackRange = 2;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 2;
                break;
        }
    }

    bool CheckAlive() {
        if (isAlive)
        {
            if (health <= 0)
            {
                isAlive = false;
            }
        }
        return isAlive;
    }

    void Update()
    {
        confirmAlive = CheckAlive();
        if (confirmAlive) { 
            if (laVision.enemyInSight)
            {
                if (canMove&&laVision.closestZombie!=null)
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
            else
            {
                canMove = true;
            }
        }
        else
        {
            Destroy(gameObject, 0.3f);
        }
    }
}
