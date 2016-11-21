using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeScript : MonoBehaviour {

    
    public bool enemyInRange = false;
    public float triggerRad;
    public VisionRangeScript laVision;

    void Start()
    {
        laVision = gameObject.GetComponentInParent<VisionRangeScript>();
        triggerRad = 1.5f;
        GetComponent<CapsuleCollider>().radius = triggerRad;
    }
    // Update is called once per frame

    bool CheckAttack() {
        bool a = false;
        foreach (GameObject zombie in laVision._zombiesInRange)
        {
            if ((laVision.closestZombie.transform.position - gameObject.transform.position).magnitude <= triggerRad&&zombie!=null&&laVision.closestZombie.GetComponent<ZombieScript>().isAlive&&laVision.closestZombie!=null)
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
  
    

 
        

