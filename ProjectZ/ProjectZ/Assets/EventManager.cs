using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventManager : MonoBehaviour
{
    //Esta clase maneja todos los eventos del juego, tiene un array con los eventos que hay (eventList).
    //Y un booleano que hace las veces de pause pero justo por debajo de este.

    //BOOLEANO TEMPORAL A BORRAR, SOLO ES PARA PRUEBAS DE FUNCIONALIDAD O PARA UN PRIMER EVENTO DEL JUEGO
    bool once = false;


    //Referencia al InputHandler
    private InputHandlerScript _input;

    //Booleano que genera la pseudoPausa en todos los demás scripts salvo el del pausado
    public bool onEvent;

    //Listado de eventos
    public Evento[] eventList;

    //Número de eventos totales
    int numEvents = 1;

    //Evento actual
    public Evento currentEvent;

    //Texto que se muestra en pantalla
    public Text currentText;

    //Canvas que se activa/desactiva
    public Canvas canvasChild;

    //Imagen un poco transparente blanca
    public RawImage blancoTrans;

    //Struct Evento que mantiene track de si esta ocurriendo, si ha ocurrido, cuantas interacciones tiene, en que interaccion se 
    //Encuentra y que mensajes tiene
    public struct Evento
    {
        public bool isHappening;
        public bool hasHappened;
        public int numInteracts;
        public int currInteract;
        public string[] messages;
    }

    //Esta funcion crea un evento con el numero de mensajes que se pasa como parametro
    private Evento CrearEvento(int i)
    {
        Evento anEvent = new Evento();
        anEvent.numInteracts = i;
        anEvent.messages = new string[i];
        anEvent.currInteract = 0;
        anEvent.isHappening = anEvent.hasHappened = false;
        return anEvent;
    }

    //Método que activa el evento en la posicion which del array de evento
    public void activateEvent(int which)
    {
        if (!eventList[which].hasHappened&&!eventList[which].isHappening&&!onEvent)
        {
            onEvent = true;
            currentEvent = eventList[which];
            currentEvent.isHappening = true;
        }
    } 

    //Método que termina el evento actual de forma interna y externa
    public void endCurrentEvent()
    {
        if (!currentEvent.hasHappened&&onEvent)
        {
            onEvent = false;
            currentEvent.isHappening = false;
            currentEvent.hasHappened = true;
        }
    }

    //Metodo que enciende/apaga el canvas en función del parametro bool 
    void setCanvas(bool tellme)
    {
        canvasChild.gameObject.SetActive(tellme);
    }
    

    // Use this for initialization
    void Start ()
    {
        _input = GameObject.Find("GameLogic").GetComponent<InputHandlerScript>();
        canvasChild = gameObject.GetComponentInChildren<Canvas>();
        blancoTrans = gameObject.GetComponentInChildren<RawImage>();
        currentText = gameObject.GetComponentInChildren<Text>();
        eventList = new Evento[numEvents];
        eventList[0] = CrearEvento(5);
        eventList[0].messages[0] = "Bueeno.. veamos si esto funciona..";
        eventList[0].messages[1] = "Va bene";
        eventList[0].messages[2] = "Va super bene";
        eventList[0].messages[3] = "Nunca se me dió bien el italiano";
        eventList[0].messages[4] = "Que más da si ya no hay italianos..";
        currentText.gameObject.SetActive(true);
        blancoTrans.gameObject.SetActive(true);
        setCanvas(false);
        onEvent = false;
    }

       
    void Update()
    {
        if (!once)
        {
            once = true;
            activateEvent(0);
        }

        if (onEvent && currentEvent.isHappening)
        {
            currentText.text = currentEvent.messages[currentEvent.currInteract];
            setCanvas(true);

            if (_input._continue)
            {
                currentEvent.currInteract++;
                if (currentEvent.currInteract < currentEvent.numInteracts)
                {
                    _input._continue = false;
                }
                else
                {
                    endCurrentEvent();
                }
            }

        }
        else
        {
            setCanvas(false);
        }
    }
}
