using UnityEngine;
using System.Collections;


public class ZombieScript : MonoBehaviour {
    public enum zombieClass { walker, runner, mutank }

    public zombieClass tipo;
    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;
    bool confirmAlive;
    public bool beingAttacked;
    // Use this for initialization
    void Start () {
        tipo = zombieClass.walker;
        confirmAlive = isAlive = true;
        health = 100;
        attack = 10;
        defense = 10;
        attackSpeed = 0.5f;
        beingAttacked = false;
    }
	
	// Update is called once per frame
	void Update () {
        confirmAlive = CheckAlive();

        if (confirmAlive)
        {
            //Código de que hace el zombie normalmente
            if (beingAttacked) {
                GetComponent<ZombieAttack>().Attack(GetComponentInChildren<VisionRangeZombie>().closestEnemy);
            }
        }
        else {
            //INTRODUCIR CÓDIGO PARA QUE EL ZOMBIE DESAPAREZCA
            Destroy(this.gameObject, 0.3f);
        }
	}

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

}
