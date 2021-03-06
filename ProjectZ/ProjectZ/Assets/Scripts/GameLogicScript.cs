﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogicScript : MonoBehaviour
{
    public static GameLogicScript gameLogic;

    /*Este código está pensado para manejar la lógica de selección y movimiento de los zombies, así como el listado de estos 
     y de los villagers en partida*/
    /*Se hace una referencia general al InputHandler
     * Referencias Temporales-> ZombieScript: Cambiar el tipo de zombie que se esta creando
                               *VillagerScript: Cambiar el tipo de villager que se esta creando
                               *ZombieMovement: Triggerear el booleano de que se le ha dado orden de movimiento y el boolean 
                               *Que genera el movimiento
                               */

    public bool isPaused;
    public PausaCanvasScript elPausaScript;
    //Vector que en su momento representara el punto destino de los zombies que se mueven
    private Vector3 endPoint;
    

    //vertical position of the gameobject
    private float yAxis;

    //Las máscaras se indican en el inspector

    //Mascara para el suelo (mask1)
    public LayerMask mascaraSuelo;

    //Mascara para los zombies (mask2)
    public LayerMask mascaraZombies;


    //Mascara para las casas (mask3)
    public LayerMask mascaraCasas;
    //Referenca al script de inputs

    public LayerMask mascaraRompible;

    InputHandlerScript _input;

    //Listas de zombies: Existentes, seleccionados en un momento y los que se quedan en la lista tras terminar la selección
    public List<GameObject> _zombies;
    public List<GameObject> _selectedZombies;
    public List<GameObject> _keptSelectedZombies;
    public List<GameObject> _bases;
    public List<GameObject> _barricadas;
    //Lista de villagers

    public List<GameObject> _villagers;

    //Objeto de cuadro de selección
    public GameObject _selectionBox;

    //Origen de la selección actual
    public Vector3 _selectionOrigin;

    //Con esta variable sabemos si hemos comenzado una selección
    public bool _selecting;

    //Estas variables son las posiciones de los 3 zombies que se crean por codigo más abajo
    //public Vector3 position1;
   // public Vector3 position2;
   // public Vector3 position3;

    public GameObject elPathfinder;


    //public Vector3 posicionBase1;

    GameObject zombie;
    GameObject walker;
    GameObject mutank;
    GameObject runner; //por ahora los runner usarán el modelo que estaba ya
    GameObject selectedBarricade;
    GameObject villager;
    GameObject baseHumana;
    public bool[] eventos;

    public Assets.Scripts.EventManager eventManager;

    #region MetodosDeAparicion

    public void SpawnVillager(Vector3 unaPos, Vector3 patrolPos) {
        GameObject villagerToSpawn = Instantiate(villager, unaPos, Quaternion.identity) as GameObject;
        GameObject anEmptyGameObject = new GameObject();
        villagerToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
        anEmptyGameObject.transform.position = patrolPos;
        _villagers.Add(villagerToSpawn);
        villagerToSpawn.GetComponent<VillagerScript>().patrolPointObject = anEmptyGameObject;
    }

    public void SpawnVillager(Vector3 unaPos) {
        GameObject villagerToSpawn = Instantiate(villager, unaPos, Quaternion.identity) as GameObject;
        villagerToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
        _villagers.Add(villagerToSpawn);
    }

    public void SpawnSoldier(Vector3 unaPos) {
        GameObject villagerToSpawn = Instantiate(villager, unaPos, Quaternion.identity) as GameObject;
        villagerToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
        _villagers.Add(villagerToSpawn);
    }


    public void SpawnZombie(ZombieScript.zombieClass unTipo, Vector3 unaPos)
    {
        GameObject zombieToSpawn = Instantiate(zombie, unaPos, Quaternion.identity) as GameObject;
        zombieToSpawn.GetComponent<ZombieScript>().tipo = unTipo;
        _zombies.Add(zombieToSpawn);
    }

    public void SpawnWalker(Vector3 unaPos) {
        GameObject zombieToSpawn = Instantiate(walker, unaPos, Quaternion.identity) as GameObject;
        _zombies.Add(zombieToSpawn);
    }

    public void SpawnMutank(Vector3 unaPos) {
        GameObject zombieToSpawn = Instantiate(mutank, unaPos, Quaternion.identity) as GameObject; 
        _zombies.Add(zombieToSpawn);
    }

    public void SpawnRunner(Vector3 unaPos) {
        GameObject zombieToSpawn = Instantiate(runner, unaPos, Quaternion.identity) as GameObject;
        _zombies.Add(zombieToSpawn);
    }
    #endregion

    void Awake()
    {
        if (gameLogic == null)
        {
            DontDestroyOnLoad(gameObject);
            gameLogic = this;
        }
        else if (gameLogic != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        eventos = new bool[10];
        
        for(int i = 0; i < 10; i++) {
            eventos[i] = false;
        }

        eventManager = FindObjectOfType<Assets.Scripts.EventManager>();

        eventManager.SetEvents(eventos,10);

        elPathfinder = GameObject.FindGameObjectWithTag("A*");

        isPaused = false;

        elPausaScript = FindObjectOfType<PausaCanvasScript>();
           
        //posicionBase1 = new Vector3(0.69f,0.05f,13.72f);

        yAxis = gameObject.transform.position.y;

        //Guardamos la referencia al input en nuestra clase
        _input = this.GetComponent<InputHandlerScript>();

        //Inicializamos las listas
        _zombies = new List<GameObject>();
        _selectedZombies = new List<GameObject>();
        _keptSelectedZombies = new List<GameObject>();
        _villagers = new List<GameObject>();

        //Se cargan los prefabs
        zombie = Resources.Load("ZombieObject") as GameObject;
        walker = Resources.Load("WalkerObject") as GameObject;
        runner = zombie;
        mutank = Resources.Load("MutankObject") as GameObject;

        villager = Resources.Load("VillagerObject") as GameObject;
        baseHumana = Resources.Load("OriginadorSoldados") as GameObject;

        //GameObject base1 = Instantiate(baseHumana, posicionBase1, Quaternion.identity) as GameObject;

      //  SpawnWalker(position1);

      //  SpawnMutank(position2);

      //  SpawnWalker(position3);

      //  SpawnVillager(new Vector3(2, 0.4f, 13));
      //  SpawnSoldier(new Vector3(4, 0.4f, 10));

        //Se oblga al pathfinder a hacer un escaneo inicial del mapa tras inicializar los elementos
        elPathfinder.GetComponent<AstarPath>().Scan();
    }


    void Update()
    {
        if (eventManager == null)
        {
            eventManager = FindObjectOfType<Assets.Scripts.EventManager>();

        }
        else {
            for (int i = 0; i < 10; i++)
            {
                eventos[i] = eventManager.eventList[i].hasHappened;
            }
        }

        //Por encima de todo lo demás se maneja el booleano del pausado
        if (!eventManager.onEvent&&Input.GetKeyDown(KeyCode.Escape)) {
            changePause();
        }


        if (!isPaused)
        {

            if (!eventManager.onEvent) { 

            //Se hace true el booleano attackToggle en cada zombie seleccionado al pulsar la tecla A
            if (_input._attackToggle && _keptSelectedZombies.Count > 0)
            {
                foreach (GameObject t in _keptSelectedZombies)
                {
                    t.GetComponent<ZombieScript>().attackToggle = true;
                }
            }

            //O Se hace false en los zombies seleccionados al pulsar la tecla S
            else if (!_input._attackToggle && _keptSelectedZombies.Count > 0)
            {
                foreach (GameObject t in _keptSelectedZombies)
                {
                    t.GetComponent<ZombieScript>().attackToggle = false;
                }
            }
            //Funcion que dibuja la caja de seleccion
            DrawSelectionBox();

            //Funcion que actualiza la seleccion
            UpdateSelection();

            //Funcion de apoyo para actualizar la seleccion cuando los zombies mueren
            UpdateSelection2();

            //Al pulsar el boton derecho del ratón, se genera un rayo en el mundo

            if ((Input.GetMouseButtonDown(0)))
            {
                RaycastHit hit;

                //Se crea la variable de rayo
                Ray ray;

                //Al ser el editor de unity se utiliza esta funcion para el Rayo

#if UNITY_EDITOR
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

                if (Physics.Raycast(ray, out hit, 80, mascaraRompible))
                {
                    selectedBarricade = hit.collider.gameObject;
                    selectedBarricade.GetComponentInParent<BarricadaScript>().ShowCircle(true);

                }
                else {
                    if (selectedBarricade != null)
                    {
                        selectedBarricade.GetComponentInParent<BarricadaScript>().ShowCircle(false);
                        selectedBarricade = null;
                    }
                }
            }

            if ((Input.GetMouseButtonDown(1)))
            {
                //Se declara una variable del struct RayCastHit
                RaycastHit hit;

                //Se crea la variable de rayo
                Ray ray;

                //Al ser el editor de unity se utiliza esta funcion para el Rayo

#if UNITY_EDITOR
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

                if (Physics.Raycast(ray, out hit, 80, mascaraRompible)) {
                    GameObject laBarricada = hit.collider.gameObject;
                    foreach (GameObject z in _keptSelectedZombies)
                    {
                        z.GetComponent<ZombieScript>().attackBarricade(laBarricada);
                    }

                }

                //Se comprueba si choca con algun collider, teniendo en cuenta solo los objetos que pertenecen a la mascara mask1 "Ground"
                else if (Physics.Raycast(ray, out hit, 80, mascaraSuelo))
                {
                    //Se guarda la posicion clicada 
                    endPoint = hit.point;

                    //Como no nos interesa que cambie la Y del zombie que se esta moviendo la restauramos a la original
                    endPoint.y = yAxis;

                    //Aqui se intenta que el zombie mire a la posicion a la que se esta moviendo
                    this.gameObject.transform.LookAt(hit.point);

                    //Esta i se utiliza de contador para repartir a los zombies alrededor del punto elegido como destino
                    int i = 0;

                    //Por cada zombie en la lista de zombies seleccionados, se establece el movimiento final en funcion de la 
                    //Cantidad de zombies que se han movido ya hacia el punto y van rotando en un angulo de 45 grados alrededor del punto
                    foreach (GameObject zombie in _keptSelectedZombies)
                    {
                        if (zombie != null)
                        {
                            Vector3 desplazamientoFinal = Vector3.zero;
                            if (i >= 1)
                            {
                                float angle = 45 * i;
                                Quaternion rotacion = Quaternion.AngleAxis(angle, Vector3.up);
                                Vector3 distancia = Vector3.right * (1f * (1 + ((i - 1) / 8)));
                                desplazamientoFinal = rotacion * distancia;
                                if (zombie.GetComponent<ZombieScript>().barricada != null)
                                {
                                    if (zombie.GetComponent<ZombieScript>().barricada._atacantes.Contains(zombie))
                                        zombie.GetComponent<ZombieScript>().barricada.VaciarSitio(zombie.GetComponent<ZombieScript>().barricadaSpot);
                                    zombie.GetComponent<ZombieScript>().barricada._atacantes.Remove(zombie);
                                }
                            }
                            //Esta funcion hace a los zombies moverse hacia el punto deseado pero teniendo en cuenta el desplazamiento final 
                            //Para cada zombie
                            MoveZombies(zombie, endPoint + desplazamientoFinal);
                            i++;
                        }
                    }
                }

            }
        }
        }
        else
        {

        }

    }

    //Método que cambia el booleano de pausado
    public void changePause() {
            isPaused = !isPaused;  
    }

    //Un método que devuelve un booleano de forma ineficiente para no tener que escribir todo el codigo de negación
    bool IsNotAlive(GameObject z)
    {
        if (z.GetComponent<ZombieScript>() != null)
        {
            return !z.GetComponent<ZombieScript>().isAlive;
        }
        else
        {
            return !z.GetComponent<VillagerScript>().isAlive;
        }
    }

    //Método que calcula la distancia entre dos GameObjects
    public float CalcularDistancia(GameObject a,GameObject b) { 
        return (a.transform.position - b.transform.position).magnitude;
    }
    //El codigo adicional que modifica las listas de zombies y villagers en funcion de una funcion que comprueba si estan vivos o no
    void UpdateSelection2()
    {
        _zombies.RemoveAll(IsNotAlive);
        _selectedZombies.RemoveAll(IsNotAlive);
        _keptSelectedZombies.RemoveAll(IsNotAlive);
        foreach (GameObject v in _villagers) {
            if (!v.GetComponent<VillagerScript>().isAlive && !v.GetComponent<VillagerScript>().hasTransformed) {
                int que = Random.Range(0, 20);

                if (que < 10)
                {
                    SpawnWalker(v.transform.position);
                }
                else if (que < 16)
                {
                    SpawnWalker(v.transform.position);
                }
                else {
                    SpawnMutank(v.transform.position);
                }
            }
        }
        _villagers.RemoveAll(IsNotAlive);
    }

    //Funcion que dibuja la caja de seleccion
    void DrawSelectionBox()
    {
        if (!_selecting)
        {
            //Si no estamos seleccionando, comprobamos que si se ha pulsado la tecla de selección
            if (_input._selectingBegins)
            {
                RaycastHit hit;
                Ray ray;

                //Lanzamos un rayo desde la pantalla de nuestra cámara, tomando como punto la posición de nuestro puntero
                ray = Camera.main.ScreenPointToRay(_input._mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    //Guardamos el punto en el que colisiona nuestro rayo.
                    _selectionOrigin = hit.point;

                    //Creamos el cuadro de selección instanciandolo a partir de un prefab
                    _selectionBox = GameObject.Instantiate(Resources.Load("SelectionBox")) as GameObject;
                    _selectionBox.GetComponent<GUITexture>().pixelInset = new Rect(_input._mousePosition.x, _input._mousePosition.y, 1, 1);
                }
            }
        }
        else
        {
            //Si ya hemos comenzado una selección, comprobamos que ésta no ha acabado
            if (_input._selectingEnds)
            {
                //Destruimos el cuadro de selección en caso de que esta haya acabado
                Destroy(_selectionBox);
            }
            else
            {
                //Estos son los límites de nuestro cuadro de selección
                Rect bound = _selectionBox.GetComponent<GUITexture>().pixelInset;

                //Con esta sencilla función pasamos el origen de la selección a coordenadas de pantalla
                Vector3 selectionOriginBox = Camera.main.WorldToScreenPoint(_selectionOrigin);

                //Recogemos los límites de nuestro cuadro en función del punto de origen y la posición actual del ratón
                bound.xMin = Mathf.Min(selectionOriginBox.x, _input._mousePosition.x);
                bound.yMin = Mathf.Min(selectionOriginBox.y, _input._mousePosition.y);
                bound.xMax = Mathf.Max(selectionOriginBox.x, _input._mousePosition.x);
                bound.yMax = Mathf.Max(selectionOriginBox.y, _input._mousePosition.y);

                //Cambiamos el pixelInset de nuestro cuadro de selección
                _selectionBox.GetComponent<GUITexture>().pixelInset = bound;
            }
        }
    }

    //Método que actualiza la selección
    void UpdateSelection()
    {
        if (!_selecting)
        {
            if (_input._selectingBegins)
            {
                //Si no mantenemos la selección ni la invertimos y comenzamos una nueva seleccion
                if (!_input._keepSelection && !_input._invertSelection)
                {
                    //Desmarcamos los zombies 

                    //AQUI FALTA CAMBIAR QUE DEJE DE APARECER EL CÍRCULO QUE AÚN ESTA POR INCLUÍR

                    foreach (GameObject zombie in _selectedZombies)
                    {
                        if (zombie != null)
                        {
                           /* Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
                            foreach (Renderer render in renders)
                                render.material.color -= Color.yellow;*/
                        }
                    }

                    //Limpiamos las listas de zombies seleccionados
                    _selectedZombies.Clear(); //Esta no es necesario limpiarla ya
                    foreach (GameObject z in _keptSelectedZombies) {
                        z.GetComponent<ZombieScript>().isSelected = false;
                    }
                    _keptSelectedZombies.Clear();
                    
                }

                //Indicamos que hemos empezado una selección
                _selecting = true;
            }
        }
        else
        {
            if (_input._selectingEnds)
            {
                //Guardamos la lista actual de zombies seleccionados
                foreach (GameObject zombie in _selectedZombies)
                    if (zombie != null)
                    {
                        _keptSelectedZombies.Add(zombie);
                        zombie.GetComponent<ZombieScript>().isSelected = true;
                    }
                //Indicamos que hemos finalizado nuestra selección
                _selecting = false;
            }
            else
            {
                RaycastHit hit;
                Ray ray;

                //Buscamos los zombies se encuentren dentro de la caja de selección
                List<GameObject> zombiesInSelectionBox = new List<GameObject>();

                //Dado que no se puede modificar una lista mientras la estás recorriendo,
                //es mejor utilizar listas alternaticas para agregar y remover

                //Lista de zombies que añadiremos a la selección
                List<GameObject> zombiesToAdd = new List<GameObject>();

                //Lista de zombies que removeremos de la selección
                List<GameObject> zombiesToRemove = new List<GameObject>();

                List<GameObject> zombiesToMove = new List<GameObject>();

                //Primero lanzamos un rayo para guardar el punto de finalización de la selección
                ray = Camera.main.ScreenPointToRay(_input._mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //Este es el plano tridimensional de selección
                    Rect selectionPlane = new Rect();

                    //Se hace al selectionPlane ser igual que la _selectionbox ya que ambos estan en función de la pantalla
                    selectionPlane = _selectionBox.GetComponent<GUITexture>().pixelInset;

                    //Comprobamos que el rayo no golpea directamente en una unidad
                    if (_zombies.Contains(hit.collider.gameObject))
                    {

                        //Como el collider forma parte del propio objeto se añaden el zombie en contacto con el propio rayo
                        //en funcion de su propio collider

                        zombiesInSelectionBox.Add(hit.collider.gameObject);

                    }

                    //Agregamos a la lista los zombies que se encuentran dentro del cuadro de selección
                    //Iteración realizada una vez por zombie en la lista de zombies
                    foreach (GameObject zombie in this._zombies)
                    {
                        if (!zombiesInSelectionBox.Contains(zombie) && (Camera.main.WorldToScreenPoint(zombie.transform.position).x >= selectionPlane.xMin && Camera.main.WorldToScreenPoint(zombie.transform.position).x <= selectionPlane.xMax && Camera.main.WorldToScreenPoint(zombie.transform.position).y >= selectionPlane.yMin && Camera.main.WorldToScreenPoint(zombie.transform.position).y <= selectionPlane.yMax))
                        {
                            zombiesInSelectionBox.Add(zombie);
                            zombie.GetComponent<ZombieScript>().isSelected = true;
                        }
                        else
                        {
                                _selectedZombies.Remove(zombie);
                                zombie.GetComponent<ZombieScript>().isSelected = false;
                        }
                    }
                }

                foreach (GameObject zombies in zombiesInSelectionBox)
                {
                    if (zombies != null)
                    {
                        if (!_input._invertSelection)
                        {
                            //Si no está pulsada la tecla de invertSelection seleccionamos los zombies del cuadro
                            if (!_selectedZombies.Contains(zombies))
                            {
                                {
                                    zombiesToAdd.Add(zombies);
                                    zombiesToMove.Add(zombies);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Si está pulsada la tecla de invertSelection removemos los zombies del cuadro
                        if (_selectedZombies.Contains(zombies))
                        {
                            zombiesToRemove.Add(zombies);
                        }
                    }
                }

                if (!_input._keepSelection)
                {
                    foreach (GameObject zombie in _keptSelectedZombies)
                    {
                        if (zombie != null)
                        {
                            if (!_input._invertSelection)
                            {
                                if (!zombiesInSelectionBox.Contains(zombie) && _selectedZombies.Contains(zombie))
                                {
                                    zombiesToRemove.Add(zombie);
                                }
                            }
                        }
                        else
                        {
                            if (!zombiesInSelectionBox.Contains(zombie) && !_selectedZombies.Contains(zombie))
                            {
                                zombiesToAdd.Add(zombie);
                                zombiesToMove.Add(zombie);

                            }
                        }
                    }
                }

                foreach (GameObject zombie in zombiesToAdd)
                {
                    if (zombie != null)
                    {
                        SelectZombie(zombie);
                    }
                }

                foreach (GameObject zombie in zombiesToRemove)
                {
                    if (zombie != null)
                    {
                        DeselectZombie(zombie);
                    }
                }

            }
        }
    }

    void SelectZombie(GameObject zombie)
    {
        //Comprobamos que el zombie no esté ya seleccionado
        if (!_selectedZombies.Contains(zombie))
        {
            //Agregamos el zombie a la lista
            _selectedZombies.Add(zombie);

            foreach (GameObject z in _selectedZombies) {

                //Se pone en true el booleano de selección
                z.GetComponent<ZombieScript>().isSelected = true;

            }
        }
    }


    //Función para deseleccionar zombies de la lista
    void DeselectZombie(GameObject zombie)
    {
        if (_selectedZombies.Contains(zombie))
        {
            //Quitamos al zombie de la lista
            _selectedZombies.Remove(zombie);
        }
    }
    //Función del movimiento
    #region movimiento
    void MoveZombies(GameObject zombie, Vector3 desiredPosition)
    {
        //En caso de que existan zombies en la lista de _keptSelectedZombies
        if (_keptSelectedZombies.Contains(zombie))
        {
            //Agregamos el zombie a la lista
            //_selectedZombies.Add(zombie);

            //Se hace una referencia al script de movimiento de cada zombie a cada iteración
            ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();

            //El booleano wasCommanded se pone en true, ya que se les ha ordenador moverse
            zombieMovement.wasCommanded = true;
            //Función que mueve a los zombies
            zombie.GetComponent<ZombieScript>().goBarricade = false;
            
            zombieMovement.MoveTo(desiredPosition);
        }

    }
    #endregion
}
