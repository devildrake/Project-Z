using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {


    public bool isAlive;
    public int health;
    public int attack;
    public int defense;
    public float attackSpeed;
    bool confirmAlive;

    // Use this for initialization
    void Start () {
        confirmAlive = isAlive = true;
        health = 100;
        attack = 10;
        defense = 10;
        attackSpeed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        confirmAlive = CheckAlive();

        if (confirmAlive)
        {
            //Código de que hace el zombie normalmente
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
