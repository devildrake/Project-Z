using UnityEngine;
using System.Collections;


public class ZombieScript : MonoBehaviour
{
    public enum zombieClass { walker, runner, mutank }

    public zombieClass tipo;
    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float movSpeed;
    public float theAttackRange;
    bool confirmAlive;
    public bool beingAttacked;
    public bool canMove;

    ZombieMovement elMovimiento;
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
        canMove = true;
        elMovimiento = gameObject.GetComponent<ZombieMovement>();
        laVision = gameObject.GetComponentInChildren<VisionRangeZombie>();
        elAtaque = gameObject.GetComponentInChildren<AttackRangeZombie>();
        confirmAlive = isAlive = true; beingAttacked = false;

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

    // Update is called once per frame
    void Update()
    {
        confirmAlive = CheckAlive();
        if (!elMovimiento.wasCommanded)
        {
            if (laVision.enemyInSight && !elAtaque.enemyInRange)
            {
                if (laVision.closestEnemy != null)
                {
                    elMovimiento.MoveTo(laVision.closestEnemy.transform.position);
                    
                }
            }
            {
                if (confirmAlive)
                {
                    //Código de que hace el zombie normalmente
                    if (beingAttacked)
                    {
                        GetComponent<ZombieAttack>().Attack(GetComponentInChildren<VisionRangeZombie>().closestEnemy);
                    }
                }
                else
                {
                    Destroy(this.gameObject, 0.3f);
                }
            }

        }
    }
}
