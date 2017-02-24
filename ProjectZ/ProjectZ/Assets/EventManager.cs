using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Esta clase maneja todos los eventos del juego, tiene un array con los eventos que hay (eventList).
    //Y un booleano que hace las veces de pause pero justo por debajo de este.
    //RECUERDA PONER EN FALSE EL BOOL DE INPUTHANDLERSCRIPT _continue CADA VEZ QUE SE DETECTE UN TRUE
    private InputHandlerScript _input;
    public bool onEvent;
    public Evento[] eventList;
    int numEvents = 1;
    public Evento currentEvent;
    
    public struct Evento
    {
        public bool isHappening;
        public bool hasHappened;
        public int numInteracts;
        public int currInteract;
        public string[] messages;
    }

    //Esta funcion crea un evento con el numero de mensajes que se pasa como parametro
    private Evento CrearEvento(int i) {
        Evento anEvent = new Evento();
        anEvent.numInteracts = i;
        anEvent.messages = new string[i];
        anEvent.currInteract = 0;
        anEvent.isHappening = anEvent.hasHappened = false;
        return anEvent;
    }

    public void activateEvent(int which)
    {
        if (!eventList[which].hasHappened&&!eventList[which].isHappening&&!onEvent)
        {
            onEvent = true;
            currentEvent = eventList[which];
            currentEvent.isHappening = true;
        }
    } 

    public void endCurrentEvent()
    {
        if (!currentEvent.hasHappened&&onEvent)
        {
            onEvent = false;
            currentEvent.isHappening = false;
            currentEvent.hasHappened = true;
        }
    }

	// Use this for initialization
	void Start ()
    {
        _input = GameObject.Find("GameLogic").GetComponent<InputHandlerScript>();
        eventList = new Evento[numEvents];
        eventList[0] = CrearEvento(5);
        eventList[0].messages[0] = "Bueeno.. veamos si esto funciona..";
        eventList[0].messages[1] = "Va bene";
        eventList[0].messages[2] = "Va super bene";
        eventList[0].messages[3] = "Nunca se me dió bien el italiano";
        eventList[0].messages[4] = "Que más da si ya no hay italianos..";


    }

    // Update is called once per frame
    void Update()
    {
        if (_input._continue&&onEvent) {
            currentEvent.currInteract++;
            Debug.Log(currentEvent.currInteract);
            _input._continue = false;
        }

    }

}
