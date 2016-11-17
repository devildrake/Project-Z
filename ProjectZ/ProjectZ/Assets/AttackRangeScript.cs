using UnityEngine;
using System.Collections;

public class AttackRangeScript : MonoBehaviour {

    public int attackRadius;
    public bool enemyInRange = false;

    void Start()
    {
        GetComponent<CapsuleCollider>().radius = attackRadius;

    }
    // Update is called once per frame
    void Update () {
        
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Zn")
        {
            enemyInRange = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Zn")
        {
            enemyInRange = false;
        }
    }
}
