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
        if (aZombie.GetComponent<ZombieScript>().isAlive && aZombie != null)
        {
            attacking = true;
            zombieToAttack = aZombie.GetComponent<ZombieScript>();
        }else
        {
            attacking = false;
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

	void Update () {
        if (attacking) {
            zombieToAttack.GetComponent<ZombieScript>().beingAttacked = true;
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
