using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoScript : MonoBehaviour {
    public int health;
    public float dmgEluded;
    private bool isAlive;
    private int maxHealth;
    public Vector3 attackPos1;
    public Vector3 attackPos2;
    public Vector3 attackPos3;
    public Vector3 attackPos4;
    public Vector3 attackPos5;
    public Vector3 attackPos6;
    public Vector3 attackPos7;
    public Vector3 attackPos8;

    // Use this for initialization
    void Start () {
        isAlive = true;
        health = 100;
        dmgEluded = 1.3f;
	}
	
	// Update is called once per frame
	void Update () {
        if (health < 0) {
            isAlive = false;
            Destroy(gameObject, 0.3f);
        }
	}

    public void isAttacked(float dmg) {
        float dmgToTake = dmg / dmgEluded;
        health -= (int)dmgToTake;
    }
}
