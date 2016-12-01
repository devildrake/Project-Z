using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieScript : MonoBehaviour
{
    public enum zombieClass { walker, runner, mutank }
    private SpriteRenderer elSprite;
    private GameObject elCirculo;
    public zombieClass tipo;
    public bool isAlive;
    public bool isSelected;
    public int health;
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
        elSprite = GetComponentInChildren<SpriteRenderer>();
        elCirculo = elSprite.gameObject;


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
                movSpeed = 2;
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
                movSpeed = 1;
                theAttackRange = 1f;
                break;
        }


    }

    public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;

    }
    // Update is called once per frame
    void Update()
    {


        confirmAlive = CheckAlive();
        if (!wasCommanded)
        {
            if (laVision.enemyInSight && !elAtaque.enemyInRange&&canAttack)
            {
                if (laVision.closestEnemy != null)
                {
                    MoveTo(laVision.closestEnemy.transform.position);
                    
                }
            }
            {
                if (confirmAlive)
                {
                    //Código de que hace el zombie normalmente
                    if (isSelected)
                    {
                        /*Renderer theRenderer = gameObject.GetComponentInChildren<Renderer>();
                        theRenderer.material.color = Color.yellow;*/
                        elCirculo.SetActive(true);    

                    }
                    else {
                        /*
                        Renderer theRenderer = gameObject.GetComponentInChildren<Renderer>();
                        theRenderer.material.color = Color.grey;*/
                        elCirculo.SetActive(false);

                    }
                }
                else
                {
                    Destroy(gameObject, 0.3f);
                }
            }

        }
    }
}
