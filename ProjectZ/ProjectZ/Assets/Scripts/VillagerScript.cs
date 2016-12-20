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
    public int health;
    public int attack;
    public int defense;
    public float movSpeed;
    public float attackSpeed;
    public float theAttackRange;

    public bool isAlive;
    public bool hasTransformed;
    public bool canMove;
    public bool goingToPat;
    public bool goingBack;
    public bool goingToCheck;

    bool confirmAlive;
    public humanClass tipo;

    public Vector3 groundPos;

    public Vector3 originalPos;
    public Vector3 patrolPoint;
    public bool freeRoam;
    public int patrolType;

    VillagerMovement villagerMovement;
    VillagerAttack villagerAttack;
    private float attackTime;

	private GameObject Latas;
	private bool goCheck = false;
    // Use this for initialization

    void Start()
    {

        freeRoam = true;
        goingToCheck = false;
        originalPos = transform.position;
        groundPos.y = originalPos.y;
        confirmAlive = isAlive = true;
        laVision = GetComponentInChildren<VisionRangeScript>();
        elAtaque = GetComponentInChildren<AttackRangeScript>();
        villagerMovement = GetComponent<VillagerMovement>();
        villagerAttack = GetComponent<VillagerAttack>();
        hasTransformed = false;

         Renderer render = this.gameObject.GetComponentInChildren<Renderer>();
                

        switch (tipo){
            case humanClass.villager:
                theAttackRange = 1;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = 1;
                render.material.color += Color.yellow;
                break;
            case humanClass.soldier:
                theAttackRange = 3;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 2;
                render.material.color += Color.green;
                break;
        }

        switch (patrolType)
        {
            case 0:
                patrolPoint = originalPos + new Vector3(3,0,0);
                break;
            case 1:
                patrolPoint = originalPos + new Vector3(0, 3, 0);
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
    void heightCheck()
    {
        if (gameObject.transform.position.y > originalPos.y)
        {

            gameObject.transform.position = groundPos;

        }


    }
    void Patrol() {

        if (!goingToPat && !goingBack) {
            goingToPat = true;
        }

        if (goingToPat)
        {
            villagerMovement.MoveTo(patrolPoint);
            
            if ((patrolPoint-gameObject.transform.position).magnitude < 0.3f) {
                goingToPat = false;
                goingBack = true;
            }

            
        }
        else if (goingBack) {
            villagerMovement.MoveTo(originalPos);
            if ((originalPos - gameObject.transform.position).magnitude < 0.3f)
            {
                goingToPat = true;
                goingBack = false;
            }
                
        }


    }
    void Update()
    {


        groundPos.x = transform.position.x;
        groundPos.z = transform.position.z;
        heightCheck();
        confirmAlive = CheckAlive();
        if (confirmAlive) {
            if (laVision.enemyInSight)
            {
                Debug.Log("Enemy Spotted");
                freeRoam = false;

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
            else if(!laVision.enemyInSight&&!goingToCheck)
            {
                freeRoam = true;
                canMove = true;
            }
            else
            {
                canMove = true;
            }
            if (canMove&&freeRoam&&!goingToCheck) {
                Debug.Log("patrolling");
                Patrol();
             //   villagerMovement.MoveTo(patrolPoint);
            }
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.3f);
        }
    }

	public void heardSomething(Vector3 somewhere)
	{
		freeRoam = false;
		goingToPat = false;
        goingBack = false;
        goingToCheck = true;
		villagerMovement.MoveTo(somewhere);
        Debug.Log("I heard something");
	}
}
