﻿using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour
{
    GameLogicScript gameLogic;

    public bool attacking;
    public bool atHuman;
    public bool atBarricade;
    public VillagerScript enemyToAttack;
    public GameObject barricade;
    public float attackTimer = 0;
    // Use this for initialization
    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;

        atHuman = false;
        atBarricade = false;
        enemyToAttack = null;
        attacking = false;
    }

    public void Attack(GameObject anEnemy)
    {
        if (anEnemy != null)
        {
            if (anEnemy.GetComponent<VillagerScript>() != null)
            {
                if (anEnemy.GetComponent<VillagerScript>().isAlive)
                {
                    attacking = true;
                    atHuman = true;
                    enemyToAttack = anEnemy.GetComponent<VillagerScript>();
                }
            }
            else
            {
                if (anEnemy.GetComponent<BarricadaScript>() != null)
                {
                    barricade = anEnemy;
                    attacking = true;
                    atBarricade = true;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameLogic.isPaused&&!gameLogic.eventManager.onEvent)
        {
            if (attacking)
            {
                if (atHuman)
                {
                    if (enemyToAttack != null && gameObject.GetComponentInChildren<AttackRangeZombie>().enemyInRange)
                    {
                        if (attackTimer < GetComponent<ZombieScript>().attackSpeed)
                        {
                            attackTimer += Time.deltaTime;
                        }
                        else
                        {
                            enemyToAttack.health -= enemyToAttack.attack;
                            attackTimer = 0;
                        }
                    }
                }
            }
            else if (atBarricade)
            {
                if (barricade != null && gameObject.GetComponentInParent<ZombieScript>().theAttackRange < (barricade.transform.position - gameObject.transform.position).magnitude)
                {
                    if (attackTimer < GetComponent<ZombieScript>().attackSpeed)
                    {
                        attackTimer += Time.deltaTime;
                    }
                    else
                    {
                        barricade.GetComponent<BarricadaScript>().health -= enemyToAttack.attack;
                        attackTimer = 0;
                    }

                }
            }
        }
    }
}
