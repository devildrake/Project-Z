using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionRangeScript : MonoBehaviour {

    public int visionRadius;
    public bool enemyInSight = false;
    public GameObject aZombieToAdd;
    public GameObject aZombieToRemove;
    public List<GameObject> _zombiesInRange;
    public GameObject closestZombie;

    void Start () {
        GetComponent<CapsuleCollider>().radius = visionRadius;
        _zombiesInRange = new List<GameObject>();
    }

    void OnTriggerEnter(Collider col) {

      
        if (col.tag == "Zn") {
            aZombieToAdd = col.gameObject;
            _zombiesInRange.Add(aZombieToAdd);

            enemyInSight = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.tag == "Zn")
        {
            aZombieToRemove = col.gameObject;
            _zombiesInRange.Remove(aZombieToRemove);
            enemyInSight = false;
        }
    }

    void checkClosest() {
        
        float closestDistance=0f;
        Vector3 distance;
        foreach (GameObject aZombie in _zombiesInRange) {
            
            distance = (transform.position-aZombie.transform.position);
            if (closestDistance < distance.magnitude) {
                closestZombie = aZombie;
            }
        }
    }

	void Update () {
        if (_zombiesInRange.Count != 0)
        {
            checkClosest();
        }
        else {
            closestZombie = null;
        }
        
	}
}
