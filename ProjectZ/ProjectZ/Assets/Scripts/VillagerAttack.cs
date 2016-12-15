﻿using UnityEngine;
using System.Collections;

public class VillagerAttack : MonoBehaviour {
    
    public bool attacking = false;
    public ZombieScript zombieToAttack;
    public VillagerScript theVillager;
    public AttackRangeScript elRango;
    public float attackTimer = 0;

	// Use this for initialization
	void Start () {
        
        zombieToAttack = null;
        theVillager = GetComponent<VillagerScript>();
        elRango = gameObject.GetComponentInChildren<AttackRangeScript>();
	}

    public void Attack(GameObject aZombie)
    {
        if (aZombie != null)
        {
            if (aZombie.GetComponent<ZombieScript>().isAlive&&elRango.enemyInRange)
            {
                Debug.Log("MustAttack");
                attacking = true;
                zombieToAttack = aZombie.GetComponent<ZombieScript>();
            }
            else
            {
                attacking = false;
            }
        }
    }
    // Update is called once per frame
    void takeDamageColor() {
        Component[] renders = zombieToAttack.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer render in renders)
        {
            render.material.color += Color.red;
        }
    }

    void noRed() {
        Component[] renders = zombieToAttack.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer render in renders)
        {
            render.material.color -= Color.red;
            Debug.Log("NoMoreRed?");
        }
    }

    void Update() {
        if (attacking)
        {
            if (zombieToAttack != null)
            {
                if (attackTimer < theVillager.attackSpeed)
                {
                    attackTimer += Time.deltaTime;
                }
                else
                {
                    zombieToAttack.health -= theVillager.attack;
                    attackTimer = 0;

                }
            }
        }

        if (!elRango.enemyInRange) {
            attacking = false;
        }
	}
}
