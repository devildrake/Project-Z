using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {

    public bool attacking = false;
    public VillagerScript enemyToAttack;
    public ZombieScript theZombie;
    public float attackTimer = 0;
    // Use this for initialization
    void Start()
    {
        enemyToAttack = null;
        theZombie = GetComponent<ZombieScript>();
       
    }

    public void Attack(GameObject anEnemy)
    {
        attacking = true;
        enemyToAttack = anEnemy.GetComponent<VillagerScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            if (attackTimer < theZombie.attackSpeed)
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
