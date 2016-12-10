using UnityEngine;
using System.Collections;
using Pathfinding;
public class VillagerMovement : MonoBehaviour
{
    public bool moving;
    public Vector3 targetPosition;
    public bool wasCommanded;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    private int puntoActual = 0;
    private bool startedMoving;
    private float distance;
    public float lindarDistancia;

    //IEnumerator Start()
    void Start()
    {
        lindarDistancia = 3.3f;
        startedMoving = false;
        buscador = gameObject.GetComponent<Seeker>();
        wasCommanded = false;
        // yield return StartCoroutine(buscarCamino(1));
    }

    public void MoveTo(Vector3 newTargetPosition)
    {
        if (!startedMoving)
        {
            startedMoving = true;
            targetPosition = newTargetPosition;
            buscador.StartPath(transform.position, targetPosition, MetodoCamino);
        }
        moving = true;

    }

    void MetodoCamino(Path path)
    {
        if (!path.error)
        {
            camino = path;
            puntoActual = 0;
        }
    }

    /*IEnumerator buscarCamino(float tiempo) {
        while (true) {
            yield return new WaitForSeconds(tiempo);
            buscador.StartPath(transform.position, targetPosition, MetodoCamino);
        }

    }*/

    void Update()
    {
        if (moving)
        {

            distance = (gameObject.transform.position - targetPosition).magnitude;

            if (camino == null)
                return;
            if (puntoActual >= camino.vectorPath.Count)
            {
                return;
            }

            Vector3 direccion = (camino.vectorPath[puntoActual] - gameObject.transform.position).normalized;

            direccion *= gameObject.GetComponent<VillagerScript>().movSpeed * Time.fixedDeltaTime;

            gameObject.transform.position += direccion * 0.5f;

            if (Vector3.Distance(transform.position, camino.vectorPath[puntoActual]) < distanciaSiguientePunto)
            {
                puntoActual++;
                return;
            }
            else
            {
                startedMoving = false;
            }

        }
    }
}




/*
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
*/
