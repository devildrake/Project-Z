﻿using UnityEngine;
using System.Collections;

public class VillagerMovement : MonoBehaviour
{
    public bool moving;
    public Vector3 targetPosition;
    private float movementLinearSpeedVillager;
    public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;

    }
    void Update()
    {
        movementLinearSpeedVillager = gameObject.GetComponent<VillagerScript>().movSpeed;
        if (moving)
        {
          

            

            Vector3 currentGroundPosition = transform.position;
            currentGroundPosition.y = 0;

            Vector3 groundTargetPosition = targetPosition;
            groundTargetPosition.y = 0;

            Vector3 direction = groundTargetPosition - currentGroundPosition;
            float remainingDistance = direction.magnitude;
            
            direction.Normalize();

            Vector3 nextMovement = direction * movementLinearSpeedVillager * Time.deltaTime;

            
            float movementDistance = nextMovement.magnitude;

            if (movementDistance < remainingDistance)
            {
               
                transform.position += nextMovement;
              
            }
            else
            {
                //float oldY = transform.position.y;
                targetPosition.y = transform.position.y;
                transform.position = targetPosition;
                //   transform.position += Vector3.up * oldY;
                moving = false;
            }
        }
    }
}
