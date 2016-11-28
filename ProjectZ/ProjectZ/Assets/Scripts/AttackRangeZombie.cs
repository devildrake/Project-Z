using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeZombie : MonoBehaviour
{
    public bool enemyInRange = false;
    private float attackRange;
    private VisionRangeZombie laVision;

    void Start()
    {
        laVision = gameObject.GetComponentInParent<VisionRangeZombie>();
    }
    // Update is called once per frame

    bool CheckAttack()
    {
        bool a = false;
        foreach (GameObject enemy in laVision._enemiesInRange)
        {
            if ((laVision.closestEnemy.transform.position - gameObject.transform.position).magnitude <= attackRange && enemy != null && laVision.closestEnemy.GetComponent<VillagerScript>().isAlive && laVision.closestEnemy != null)
            {
                a = true;
            }
        }
        return a;
    }
    void Update()
    {
        attackRange = gameObject.GetComponentInParent<ZombieScript>().theAttackRange;
        enemyInRange = CheckAttack();
    }
}