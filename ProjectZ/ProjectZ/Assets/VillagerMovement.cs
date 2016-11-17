using UnityEngine;
using System.Collections;

public class VillagerMovement : MonoBehaviour
{
    public bool moving;
    public Vector3 targetPosition;
    public float movementLinearSpeedVillager = 10;
    public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;

    }
    void Update()
    {
        if (moving)
        {
          

            Debug.DrawLine(targetPosition,targetPosition+Vector3.up*10);

            Vector3 currentGroundPosition = transform.position;
            currentGroundPosition.y = 0;

            Vector3 groundTargetPosition = targetPosition;
            groundTargetPosition.y = 0;

            Vector3 direction = groundTargetPosition - currentGroundPosition;
            float remainingDistance = direction.magnitude;
            Debug.Log(remainingDistance);
            direction.Normalize();

            Vector3 nextMovement = direction * movementLinearSpeedVillager * Time.deltaTime;

            Debug.Log("direction");
            Debug.Log(direction);
            Debug.Log("DeltaTime");
            Debug.Log(Time.deltaTime);
            Debug.Log("LinearSpeed");
            Debug.Log(movementLinearSpeedVillager);

            Debug.Log(nextMovement);
            float movementDistance = nextMovement.magnitude;

            if (movementDistance < remainingDistance)
            {
                Debug.Log(nextMovement);
                transform.position += nextMovement;
                Debug.Log("SHOULDMOVE");
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
