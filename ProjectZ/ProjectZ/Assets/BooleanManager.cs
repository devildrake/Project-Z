using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanManager : MonoBehaviour {
    ZombieScript zombieScript;
    ZombieMovement zombieMovement;
    VisionRangeZombie visionRange;
    AttackRangeZombie attackRange;
    GameLogicScript gameLogic;

    public bool wasCommanded, isAlive, moving, attacking, changedTargetPos, enemyInSight, enemyInRange, noEnemiesInRange, 
        hasArrived, canAttack, movingToAttack, movingToBarricade,movingToBuilding,inBuilding;


	// Use this for initialization
	void Start () {
        zombieScript = gameObject.GetComponent<ZombieScript>();
        zombieMovement = gameObject.GetComponent<ZombieMovement>();
        visionRange = gameObject.GetComponentInChildren<VisionRangeZombie>();
        attackRange = visionRange.GetComponentInChildren<AttackRangeZombie>();
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogicScript>();
	}

    public void Command() {
        moving = attacking = changedTargetPos = hasArrived = movingToAttack = false;
    }

    private void MovementBooleans() {
        if (wasCommanded) {
            Command();
        }
    }

    private void StatusBooleans() {
        if (zombieScript.health >= 0) {
            isAlive = false;
        }
        
    }

	// Update is called once per frame
	void Update () {
        if (!gameLogic.isPaused) {
            MovementBooleans();
            StatusBooleans();
        }
	}
}
