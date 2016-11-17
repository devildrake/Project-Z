using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {

    public bool attacking = false;
    public ZombieScript enemyToAttack;
    public ZombieScript theZombie;
    public float attackTimer = 0;
    // Use this for initialization
    void Start()
    {
        enemyToAttack = null;
        theZombie = GetComponent<ZombieScript>();
    }

    public void Attack(GameObject aZombie)
    {
        attacking = true;
        enemyToAttack = aZombie.GetComponent<ZombieScript>();
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
                theZombie.health -= theZombie.attack;
                attackTimer = 0;
            }
        }
    }
}
