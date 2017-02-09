﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieScript : MonoBehaviour
{

	public enum zombieClass { walker, runner, mutank }
    Animator elAnimator;
    private SpriteRenderer elSprite;
    public GameObject elCirculo;
    public zombieClass tipo;
    public bool isAlive;
    public bool isSelected;
    public bool canMove;
    public bool canAttack;
    public bool moving;
    public bool goBarricade;
    public bool inBuilding;
    private GameLogicScript gameLogic;
    bool confirmAlive;
    public bool hasArrived;
    public float health;
    float prevHealth;
    float healthCounter;
    float defenseTime;
    public float maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float movSpeed;
    public float theAttackRange;
    public bool irCasa;
    float initSpeedAn;
    public Vector3 puntoCasa;
    public Vector3 targetPosition;
    public Vector3 prevTargetPos;
    public float movementLinearSpeed;
    public bool defenseMode;
    VisionRangeZombie laVision;
    AttackRangeZombie elAtaque;
    ZombieMovement elMovimiento;
    bool movingToEnemy = false;
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

    public void CasaBehaviour(CasaDestruidaScript laCasa) {
        irCasa = true;
        elMovimiento.wasCommanded = true;
        if (laCasa.CheckTrues() != 12)
        {
             puntoCasa= laCasa.AssignarSitio();
        }
        }

    public void attackBarricade(GameObject laBarricada) {
        goBarricade = true;
        elMovimiento.MoveTo(laBarricada.transform.position);

    }

    void Start()
    {
        gameLogic = FindObjectOfType<GameLogicScript>();
        defenseMode = false;
        defenseTime = 2f;
        elAnimator = gameObject.GetComponent<Animator>();
        elAnimator.SetBool("moviendose", false);

        goBarricade = false;
        hasArrived = false;
    originalPos = gameObject.transform.position;
        groundPos.y = originalPos.y;

        elMovimiento = this.gameObject.GetComponent<ZombieMovement>();
        elSprite = GetComponentInChildren<SpriteRenderer>();
        elCirculo = elSprite.gameObject;
		elCirculo.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;

        inBuilding = false;
        canMove = canAttack = true;
        laVision = gameObject.GetComponentInChildren<VisionRangeZombie>();
        elAtaque = gameObject.GetComponentInChildren<AttackRangeZombie>();
        confirmAlive = isAlive = true;
        initSpeedAn = elAnimator.speed;

        switch (tipo) {
            case zombieClass.walker:
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1f;
                movSpeed = 5f;
                theAttackRange = 0.8f;
                break;
            case zombieClass.runner:
                health = 50;
                attack = 5;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 6f;
                theAttackRange = 0.8f;
                break;
            case zombieClass.mutank:
                elAnimator.SetBool("ModoDefensa", false);
                health = 300;
                attack = 20;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = 3f;
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
        if (!gameLogic.isPaused)
        {
            if (elAnimator.speed == 0) {
                elAnimator.speed = initSpeedAn;
            }

            #region comportamiento Mutank
            if (tipo == zombieClass.mutank)
            {
                //Comportamiento especifico de mutank

                if (prevHealth != health)
                {
                    defenseMode = false;
                }
                else
                {
                    healthCounter += Time.deltaTime;
                    if (healthCounter > defenseTime)
                    {
                        defenseMode = false;
                    }
                }
                if (defenseMode)
                {
                    elAnimator.SetBool("ModoDefensa", true);
                    defense = 30;
                }
                else
                {
                    elAnimator.SetBool("ModoDefensa", false);
                    defense = 15;
                    healthCounter = 0;
                }
                prevHealth = health;

            }
            #endregion


            if (elMovimiento.moving) //Codigo que pone true el booleano del animador "moviendose" cuando moving es true
            {
                elAnimator.SetBool("moviendose", true);
            }
            else
            {
                elAnimator.SetBool("moviendose", false);
            }

            groundPos.x = gameObject.transform.position.x;
            groundPos.z = gameObject.transform.position.z;
            heightCheck();
            confirmAlive = CheckAlive();
            if (confirmAlive)
            {
                if (irCasa)
                {
                    elMovimiento.MoveTo(puntoCasa);
                    if (hasArrived)
                    {
                        irCasa = false;
                        elMovimiento.wasCommanded = false;
                    }
                }
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

            if (!elMovimiento.wasCommanded)
            {
                if (laVision.enemyInSight)
                {

                    if (!elAtaque.enemyInRange)
                    {

                        if (canAttack)
                        {


                            if (canMove)
                            {


                                if (laVision.closestEnemy != null)
                                {

                                    if (laVision.closestEnemy.transform.position != prevTargetPos)
                                    {
                                        movingToEnemy = false;
                                    }
                                    prevTargetPos = laVision.closestEnemy.transform.position;
                                    if (!movingToEnemy)
                                    {

                                        movingToEnemy = true;
                                        elMovimiento.MoveTo(laVision.closestEnemy.transform.position);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //color vida
            if (health / maxHealth * 100 <= 20)
            {

                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (health / maxHealth * 100 <= 50)
            {

                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }



        }
        else {
            elAnimator.speed = 0;
        }
    }
	
}
