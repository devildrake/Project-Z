using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class IAPersonaje : MonoBehaviour {


    private CharacterController controlador;
    private Path camino;

    public float velocidad = 100;
    public float distanciaSiguientePunto = 0.5f;

    private int puntoActual = 0;

    public GameObject objetivoJugador;
    private Vector3 prevObj;
    private Seeker buscador;
	// Use this for initialization
	void Start () {
        buscador = GetComponent<Seeker>();
        controlador = GetComponent<CharacterController>();

        buscador.StartPath(gameObject.transform.position, objetivoJugador.transform.position, metodoCamino);
    }

    void metodoCamino(Path path) {
        if (!path.error)
        {
            camino = path;
            puntoActual = 0;
        }
    }

    void FixedUpdate() {
        if (camino==null)
            return;       

        if (puntoActual >= camino.vectorPath.Count) 
            return;

        Vector3 direccion = (camino.vectorPath[puntoActual] - transform.position).normalized;

        transform.position += direccion * Time.fixedDeltaTime;

        //controlador.SimpleMove(direccion);

        if (Vector3.Distance(transform.position, camino.vectorPath[puntoActual]) < distanciaSiguientePunto) {
            puntoActual++;
            return;
        }

    }

	// Update is called once per frame
	void Update () {
        if (prevObj != objetivoJugador.transform.position)
        {
            buscador.StartPath(gameObject.transform.position, objetivoJugador.transform.position, metodoCamino);
            prevObj = objetivoJugador.transform.position;
        }


    }
}
