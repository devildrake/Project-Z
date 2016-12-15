using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {

    public bool attacking;
    public VillagerScript enemyToAttack;
    public float attackTimer = 0;
    // Use this for initialization
    void Start()
    {
        enemyToAttack = null;
        attacking = false;
    }

    public void Attack(GameObject anEnemy)
    {
        if (anEnemy != null)
        {
            if (anEnemy.GetComponent<VillagerScript>().isAlive)
            {
                attacking = true;
                enemyToAttack = anEnemy.GetComponent<VillagerScript>();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (attacking)
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
}
