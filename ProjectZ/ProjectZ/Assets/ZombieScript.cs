using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {


    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;

    // Use this for initialization
    void Start () {
        isAlive = true;
        health = 100;
        attack = 10;
        defense = 10;
        attackSpeed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        checkAlive();

        if (!isAlive)
        {
            Debug.Log("I died");
        }
	}

    void checkAlive() {
        if (health <= 0)
        {
            isAlive = false;
        }
    }

}
