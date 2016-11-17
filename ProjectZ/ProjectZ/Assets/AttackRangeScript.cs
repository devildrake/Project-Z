using UnityEngine;
using System.Collections;

public class AttackRangeScript : MonoBehaviour {

    
    public bool enemyInRange = false;

    void Start()
    {
        

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
