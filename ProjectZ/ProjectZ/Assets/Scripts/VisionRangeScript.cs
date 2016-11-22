using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionRangeScript : MonoBehaviour {

    public bool enemyInSight = false;
    public GameObject aZombieToAdd;
    public GameObject aZombieToRemove;
    public List<GameObject> _zombiesInRange;
    public GameObject closestZombie;
    public bool hasCheckedFirst = false;

    void Start () {
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
            if (_zombiesInRange.Count == 0) {
                enemyInSight = false;
            }
        }
    }

    GameObject GetClosestZombie(List<GameObject> zombies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in zombies)
        {
            if (t != null)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    bool IsNotAlive(GameObject z) { 
        return !z.GetComponent<ZombieScript>().isAlive;
    }

    void CheckZombieAlive() {
        _zombiesInRange.RemoveAll(IsNotAlive);
        foreach (GameObject zombie in _zombiesInRange)
        {
            if (!zombie.GetComponent<ZombieScript>().isAlive && zombie != null)
            {
                _zombiesInRange.Remove(zombie);
            }
        }
    }
    void Update() {
        if (_zombiesInRange.Count >0)
        {
            CheckZombieAlive();
            closestZombie = GetClosestZombie(_zombiesInRange);
        }
        else {
            hasCheckedFirst = false;
            closestZombie = null;
        }
	}
}
