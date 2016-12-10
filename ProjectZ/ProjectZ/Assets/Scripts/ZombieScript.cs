using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieScript : MonoBehaviour
{
    public enum zombieClass { walker, runner, mutank }
    private SpriteRenderer elSprite;
    public GameObject elCirculo;
    public zombieClass tipo;
    public bool isAlive;
    public bool isSelected;
    public bool startedMovingToAnEnemy;
    public float health;
    public float maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float movSpeed;
    public float theAttackRange;
    bool confirmAlive;
    public bool canMove;
    public bool canAttack;
    public bool moving;
    public Vector3 targetPosition;
    public float movementLinearSpeed;
    public bool wasCommanded;
    VisionRangeZombie laVision;
    AttackRangeZombie elAtaque;
    ZombieMovement elMovimiento;
    Vector3 originalPos;
    Vector3 groundPos;

    // Use this for initialization
    bool CheckAlive()
    {
        if (isAlive)
        {
            if (health <= 0)
            {
                isAlive = false;
            }
        }
        return isAlive;
    }

    void Start()
    {
         startedMovingToAnEnemy = false;
    originalPos = gameObject.transform.position;
        groundPos.y = originalPos.y;

        elMovimiento = this.gameObject.GetComponent<ZombieMovement>();
        elSprite = GetComponentInChildren<SpriteRenderer>();
        elCirculo = elSprite.gameObject;
		elCirculo.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;


        canMove = canAttack = true;
        laVision = gameObject.GetComponentInChildren<VisionRangeZombie>();
        elAtaque = gameObject.GetComponentInChildren<AttackRangeZombie>();
        confirmAlive = isAlive = true;

        switch (tipo) {
            case zombieClass.walker:
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1f;
                movSpeed = 1;
                theAttackRange = 0.8f;
                break;
            case zombieClass.runner:
                health = 50;
                attack = 5;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 4;
                theAttackRange = 0.8f;
                break;
            case zombieClass.mutank:
                health = 300;
                attack = 20;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = 0.75f;
                theAttackRange = 1f;
                break;
        }
        maxHealth = health;


    }

    /*public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;
    }*/
    // Update is called once per frame
    void heightCheck() {
        if (gameObject.transform.position.y > originalPos.y) {

            gameObject.transform.position = groundPos;

        }


    }



    void Update()
    {

        heightCheck();

        groundPos.x = gameObject.transform.position.x;
        groundPos.z = gameObject.transform.position.z;
        confirmAlive = CheckAlive();
        if (confirmAlive)
        {
            
            //Código de que hace el zombie normalmente
            if (isSelected)
            {
                /*Renderer theRenderer = gameObject.GetComponentInChildren<Renderer>();
                theRenderer.material.color = Color.yellow;*/
                elCirculo.SetActive(true);


            }
            else
            {

                elCirculo.SetActive(false);

            }
        }
        else
        {
            Destroy(gameObject, 0.3f);
        }

        if (!wasCommanded)
        {
            if (laVision.enemyInSight && !elAtaque.enemyInRange && canAttack && canMove)
            {
                if (laVision.closestEnemy != null)
                {
                    Debug.Log("SHould Move");
                    if (!startedMovingToAnEnemy)
                    {
                        elMovimiento.MoveTo(laVision.closestEnemy.transform.position);
                        startedMovingToAnEnemy = true;
                    }
                }
            }
           

        }
        //color vida
        if (health / maxHealth * 100 <= 20)
        {

            elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (health/maxHealth*100 <= 50) {

			elCirculo.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
		}


	
    }
}
