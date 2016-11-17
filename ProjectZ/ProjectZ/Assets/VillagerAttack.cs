using UnityEngine;
using System.Collections;

public class VillagerAttack : MonoBehaviour {
    public bool attacking = false;
    public ZombieScript zombieToAttack;
    public VillagerScript theVillager;
    public float attackTimer = 0;
	// Use this for initialization
	void Start () {
        zombieToAttack = null;
        theVillager = GetComponent<VillagerScript>();
	}

    public void Attack(GameObject aZombie) {
        attacking = true;
        zombieToAttack = aZombie.GetComponent<ZombieScript>();
    }
	// Update is called once per frame
	void Update () {
        if (attacking) {
            if (attackTimer < theVillager.attackSpeed)
            {
                attackTimer += Time.deltaTime;
            }
            else {
                zombieToAttack.health -= theVillager.attack;
                attackTimer = 0;
            }
        }
	}
}
