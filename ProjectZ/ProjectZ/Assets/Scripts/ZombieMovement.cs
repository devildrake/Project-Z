using UnityEngine;
using System.Collections;

public class ZombieMovement : MonoBehaviour
{
    public bool moving;
    public Vector3 targetPosition;
    public float movementLinearSpeed;
    public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;
        
    }
	void Update ()
    {
	    if (moving)
        {
            //Debug.DrawLine(targetPosition,targetPosition+Vector3.up*10);

            Vector3 currentGroundPosition = transform.position;
            currentGroundPosition.y = 0;

            Vector3 groundTargetPosition = targetPosition;
            groundTargetPosition.y = 0;

            Vector3 direction = groundTargetPosition - currentGroundPosition;
            float remainingDistance = direction.magnitude;

            direction.Normalize();

            Vector3 nextMovement = direction * movementLinearSpeed * Time.deltaTime;
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
