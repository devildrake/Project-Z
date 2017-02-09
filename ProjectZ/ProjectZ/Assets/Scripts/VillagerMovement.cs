using UnityEngine;
using System.Collections;
using Pathfinding;
public class VillagerMovement : MonoBehaviour
{
    /*public bool moving;
    public Vector3 targetPosition;
    public Vector3 prevTargetPos;
    public bool wasCommanded;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    private int puntoActual = 0;
    private bool startedMoving;
    public float lindarDistancia;
    public float temporizador;
    public float tiempoParaBuscar;
    public bool hasToCharge = false;
    private bool hasChanged = false;

    IEnumerator Start()
   // void Start()
    {
        temporizador = 0;
        lindarDistancia = 3.3f;
        startedMoving = false;
        buscador = gameObject.GetComponent<Seeker>();
        wasCommanded = false;
        yield return StartCoroutine(buscarCamino(1));
    }

    public void MoveTo(Vector3 newTargetPosition)
    {

        if (!gameObject.GetComponent<VillagerScript>().freeRoam&&gameObject.GetComponent<VillagerScript>().canMove) {

            if (!hasToCharge)
            {
                hasToCharge = true;
                startedMoving = false;
            }
            else hasToCharge = false;
        }

        if (!startedMoving)
        {
            startedMoving = true;

            if (targetPosition != newTargetPosition)
            {
                //if (temporizador > tiempoParaBuscar)
                {
                    buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                    targetPosition = newTargetPosition;
                }
            }
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

    IEnumerator buscarCamino(float tiempo) {
        while (true) {
            yield return new WaitForSeconds(tiempo);
            buscador.StartPath(transform.position, targetPosition, MetodoCamino);
        }

    }

    void Change() {
        hasChanged = false;
        buscador.StartPath(transform.position, targetPosition, MetodoCamino);
    }

    void Update()
    {

        if (moving)
        {

            if (targetPosition != prevTargetPos) {
                hasChanged = true;
            }

            if (hasChanged) {
                Invoke("Change", 1);
            }

            if (camino == null)
                return;
            if (puntoActual >= camino.vectorPath.Count)
            {
                startedMoving = false;
                moving = false;

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
                
            }

        }

        if (targetPosition != null)
        {
            prevTargetPos = targetPosition;
        }
    }
}

    */


    GameLogicScript gameLogic;


    public bool moving;
    public Vector3 targetPosition;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    private int puntoActual = 0;
    public float contador;
    public float tiempoAContar;
    //private bool startedMoving;



    IEnumerator Start()
    //void Start()
    {
        gameLogic = FindObjectOfType<GameLogicScript>();

        tiempoAContar = 1;
        //startedMoving = false;
        buscador = gameObject.GetComponent<Seeker>();
        yield return StartCoroutine(buscarCamino(1));
    }

    public void MoveTo(Vector3 newTargetPosition)
    {
        //if (!startedMoving)
        {
            //  startedMoving = true;
            if (targetPosition != newTargetPosition)
            {
                contador += Time.deltaTime;
                if (contador > tiempoAContar)
                {
                    buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                    contador = 0;
                }
            }
            targetPosition = newTargetPosition;

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


    IEnumerator buscarCamino(float tiempo)
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempo);
            buscador.StartPath(transform.position, targetPosition, MetodoCamino);
        }

    }




    void Update()
    {
        if (!gameLogic.isPaused){

            if (moving)
            {

                if (camino == null)
                    return;
                if (puntoActual >= camino.vectorPath.Count)
                {
                    //LlegaAlFinal
                    moving = false;
                    gameObject.GetComponent<VillagerScript>().goingToCheck = false;

                    //startedMoving = false;
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

                }

            }
        }
    } }

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
