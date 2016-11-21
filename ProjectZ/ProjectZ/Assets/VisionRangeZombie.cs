using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionRangeZombie : MonoBehaviour
{

    public bool enemyInSight = false;
    public GameObject anEnemyToAdd;
    public GameObject anEnemyToRemove;
    public List<GameObject> _enemiesInRange;
    public GameObject closestEnemy;
    public bool hasCheckedFirst = false;

    void Start()
    {
        _enemiesInRange = new List<GameObject>();
    }

    void OnTriggerEnter(Collider col)
    {


        if (col.tag == "Enemy")
        {
            anEnemyToAdd = col.gameObject;
            _enemiesInRange.Add(anEnemyToAdd);

            enemyInSight = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            anEnemyToRemove = col.gameObject;
            _enemiesInRange.Remove(anEnemyToRemove);
            if (_enemiesInRange.Count == 0)
            {
                enemyInSight = false;
            }
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
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

    bool IsNotAlive(GameObject z)
    {
        return !z.GetComponent<VillagerScript>().isAlive;
    }

    void CheckEnemyAlive()
    {
        _enemiesInRange.RemoveAll(IsNotAlive);
        foreach (GameObject enemy in _enemiesInRange)
        {
            if (!enemy.GetComponent<VillagerScript>().isAlive && enemy != null)
            {
                _enemiesInRange.Remove(enemy);
            }
        }
    }
    void Update()
    {
        if (_enemiesInRange.Count > 0)
        {
            CheckEnemyAlive();
            closestEnemy = GetClosestEnemy(_enemiesInRange);
        }
        else {
            hasCheckedFirst = false;
            closestEnemy = null;
        }
    }
}

