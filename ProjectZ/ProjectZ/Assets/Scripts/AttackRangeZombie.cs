using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeZombie : MonoBehaviour
{


    public bool enemyInRange = false;
    public float triggerRad;
    public VisionRangeZombie laVision;

    void Start()
    {
        laVision = gameObject.GetComponentInParent<VisionRangeZombie>();
        triggerRad = 1.5f;
        GetComponent<CapsuleCollider>().radius = triggerRad;
    }
    // Update is called once per frame

    bool CheckAttack()
    {
        bool a = false;
        foreach (GameObject enemy in laVision._enemiesInRange)
        {
            if ((laVision.closestEnemy.transform.position - gameObject.transform.position).magnitude <= triggerRad && enemy != null && laVision.closestEnemy.GetComponent<VillagerScript>().isAlive && laVision.closestEnemy != null)
            {
                a = true;
            }
        }
        return a;
    }
    void Update()
    {
        enemyInRange = CheckAttack();
    }
}