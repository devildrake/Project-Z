using UnityEngine;
using System.Collections;
using Pathfinding;
using Pathfinding.RVO;

public class ZombieMovement : MonoBehaviour
{
    GameLogicScript gameLogic;
    public bool moving;
    public Vector3 targetPosition;
    public bool wasCommanded;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    private int puntoActual = 0;
    public float contador;
    public float tiempoAContar;
    //private bool startedMoving;


	//array
	private int suma;
	//array casa

	private GameObject posC1;
	private GameObject posC2;
	private GameObject posC3;

	private float posC1Y;
	private float posC2Y;
	private float posC3Y;

    IEnumerator Start()
    //void Start()
    {
        gameLogic = FindObjectOfType<GameLogicScript>();

        tiempoAContar = 1f;
        //startedMoving = false;
        buscador = gameObject.GetComponent<Seeker>();
      
        yield return StartCoroutine(buscarCamino(2f));
    }
	void start()
	{
          wasCommanded = false;
	}

    public void MoveTo(Vector3 newTargetPosition)
    {
        //if (!startedMoving)
        {
            //  startedMoving = true;
            if (targetPosition != newTargetPosition)
            {
                if (gameObject.GetComponent<ZombieScript>().movingToEnemy)
                {
                    contador += Time.deltaTime;
                    if (contador > tiempoAContar)
                    {
                        buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                        contador = 0;
                    }
                }
                else {
                    buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                    contador = 0;
                }
                gameObject.transform.LookAt(newTargetPosition);
                gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
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

    
    IEnumerator buscarCamino(float tiempo) {
        while (true) {
            yield return new WaitForSeconds(tiempo);
            buscador.StartPath(transform.position, targetPosition, MetodoCamino);
        }

    }




    void Update()
    {
        if (!gameLogic.isPaused)
        {
            if (moving)
            {

                if (!gameObject.GetComponent<ZombieScript>().isSelected)
                {
                    gameObject.GetComponent<ZombieScript>().elCirculo.SetActive(false);
                }
                else
                {
                    gameObject.GetComponent<ZombieScript>().elCirculo.SetActive(true);
                }



                if (camino == null)
                    return;
                if (puntoActual >= camino.vectorPath.Count)
                {
                    //LlegaAlFinal
                    moving = false;
                    wasCommanded = false;
                    gameObject.GetComponent<ZombieScript>().hasArrived = true;
                    //startedMoving = false;
                    return;
                }

                Vector3 direccion = (camino.vectorPath[puntoActual] - gameObject.transform.position).normalized;

                direccion *= gameObject.GetComponent<ZombieScript>().movSpeed * Time.fixedDeltaTime;

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

            //array limite

            if (suma > 3)
            {
                suma = 3;
            }


        }
    }
	void IntriggerEnter(Collision other)
	{

		if (this.gameObject.tag == "EntradaCasaZ") 
		{
			suma++;
		}
	}
}

   /* void Update()
    {
        movementLinearSpeed = gameObject.GetComponent<ZombieScript>().movSpeed;
        if (moving)
        {
            //Debug.DrawLine(targetPosition,targetPosition+Vector3.up*10);
            buscador.StartPath(gameObject.transform.position, targetPosition, MetodoCamino);

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
                wasCommanded = false;
                gameObject.GetComponent<ZombieScript>().wasCommanded = false;
            }
        }
	}   
    }
    */
  //  }
