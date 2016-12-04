using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogicScript : MonoBehaviour
{
    /*Este código está pensado para manejar la lógica de selección y movimiento de los zombies, así como el listado de estos 
     y de los villagers en partida*/
    /*Se hace una referencia general al InputHandler
     * Referencias Temporales-> ZombieScript: Cambiar el tipo de zombie que se esta creando
                               *VillagerScript: Cambiar el tipo de villager que se esta creando
                               *ZombieMovement: Triggerear el booleano de que se le ha dado orden de movimiento y el boolean 
                               *Que genera el movimiento
                               */


    //Vector que en su momento representara el punto destino de los zombies que se mueven
    private Vector3 endPoint;

    //vertical position of the gameobject
    private float yAxis;

    //Mascara para el suelo
    public LayerMask mask1 = 8;

    //Mascara para los zombies
    public LayerMask mask2 = 9;

    //Referenca al script de inputs

    InputHandlerScript _input;

    //Listas de zombies: Existentes, seleccionados en un momento y los que se quedan en la lista tras terminar la selección
    public List<GameObject> _zombies;
    public List<GameObject> _selectedZombies;
    public List<GameObject> _keptSelectedZombies;

    //Lista de villagers

    public List<GameObject> _villagers;



    //Objeto de cuadro de selección
    public GameObject _selectionBox;

    //Origen de la selección actual
    public Vector3 _selectionOrigin;

    //Con esta variable sabemos si hemos comenzado una selección
    public bool _selecting;

    //Estas variables son las posiciones de los 3 zombies que se crean por codigo más abajo
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;
    GameObject zombie;

    void Start()
    {

        yAxis = gameObject.transform.position.y;
        //Guardamos la referencia al input en nuestra clase
        _input = this.GetComponent<InputHandlerScript>();

        //Inicializamos las listas
        _zombies = new List<GameObject>();
        _selectedZombies = new List<GameObject>();
        _keptSelectedZombies = new List<GameObject>();
        _villagers = new List<GameObject>();


        //Se crean 3 zombies y un villager

        //Cargando los prefabs
        zombie = Resources.Load("ZombieObject") as GameObject;
        GameObject villager = Resources.Load("VillagerObject") as GameObject;

        //Se crea  el primer zombie y se establece el tipo de zombie del que se trata para que luego el zombie haga lo que tenga que hacer
        GameObject zombie1 = GameObject.Instantiate(zombie, position1, Quaternion.identity) as GameObject;
        zombie1.GetComponent<ZombieScript>().tipo = ZombieScript.zombieClass.runner;

        //Se crea  el segundo zombie y se establece el tipo de zombie del que se trata para que luego el zombie haga lo que tenga que hacer
        GameObject zombie2 = GameObject.Instantiate(zombie, position2, Quaternion.identity) as GameObject;
        zombie2.GetComponent<ZombieScript>().tipo = ZombieScript.zombieClass.mutank;

        //Se crea  el tercer zombie y se establece el tipo de zombie del que se trata para que luego el zombie haga lo que tenga que hacer
        GameObject zombie3 = GameObject.Instantiate(zombie, position3, Quaternion.identity) as GameObject;
        zombie3.GetComponent<ZombieScript>().tipo = ZombieScript.zombieClass.walker;

        //Se crea un villager y se establece el tipo de villager del que se trata para que haga lo que deba hacer
        GameObject villager1 = GameObject.Instantiate(villager, new Vector3(2, 1, 10), Quaternion.identity) as GameObject;
        villager1.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
		//soldier
		GameObject villager2 = GameObject.Instantiate(villager, new Vector3(4, 1, 10), Quaternion.identity) as GameObject;
		villager2.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;

        //Añadimos los zombies a la lista
        _zombies.Add(zombie1);
        _zombies.Add(zombie2);
        _zombies.Add(zombie3);
        _villagers.Add(villager1);
		_villagers.Add(villager2);
    }


    void Update()
    {
        //Se hace true el booleano canAtack en cada zombie seleccionado al pulsar la tecla A
        if (_input._mustAttack && _keptSelectedZombies.Count > 0)
        {
            foreach (GameObject t in _keptSelectedZombies)
            {
                t.GetComponent<ZombieScript>().canAttack = true;
            }
        }

        //O Se hace false en los zombies seleccionados al pulsar la tecla S
        else if (_input._mustNotAttack && _keptSelectedZombies.Count > 0)
        {
            foreach (GameObject t in _keptSelectedZombies)
            {
                t.GetComponent<ZombieScript>().canAttack = false;
            }
        }
        //Funcion que dibuja la caja de seleccion
        DrawSelectionBox();

        //Funcion que actualiza la seleccion
        UpdateSelection();

        //Funcion de apoyo para actualizar la seleccion cuando los zombies mueren
        UpdateSelection2();

        //Al pulsar el boton derecho del ratón, se genera un rayo en el mundo

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

            //Esto sería en caso de que estuveiramos hablando de un aparato tactil (smartphone)
            /*		#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    */


            //Se comprueba si choca con algun collider, teniendo en cuenta solo los objetos que pertenecen a la mascara mask1 "Ground"
            if (Physics.Raycast(ray, out hit, 80, mask1))
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

    //Una funcion booleana ineficiente para no tener que escribir todo el codigo de negación
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

    //El codigo adicional que modifica las listas de zombies y villagers en funcion de una funcion que comprueba si estan vivos o no
    void UpdateSelection2()
    {
        _zombies.RemoveAll(IsNotAlive);
        _selectedZombies.RemoveAll(IsNotAlive);
        _keptSelectedZombies.RemoveAll(IsNotAlive);
        foreach (GameObject v in _villagers) {
            if (!v.GetComponent<VillagerScript>().isAlive && !v.GetComponent<VillagerScript>().hasTransformed) {
                GameObject aZombie = GameObject.Instantiate(zombie, v.transform.position, Quaternion.identity) as GameObject;
                aZombie.GetComponent<ZombieScript>().tipo = ZombieScript.zombieClass.runner;
                _zombies.Add(aZombie);
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
                            Debug.Log("OnBox");
                        }
                        else
                        {
                                _selectedZombies.Remove(zombie);
                                zombie.GetComponent<ZombieScript>().isSelected = false;
                                Debug.Log("BURH");                    
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
                z.GetComponent<ZombieScript>().isSelected = true;

            }
            //Marcamos el zombie de color amarillo
            /*Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer render in renders)
            {
                render.material.color += Color.yellow;
            }*/
        }
    }


    //Función para deseleccionar zombies de la lista
    void DeselectZombie(GameObject zombie)
    {
        if (_selectedZombies.Contains(zombie))
        {
            //Removemos el zombie de la lista
            _selectedZombies.Remove(zombie);
            //Desmarcamos el zombie
            /*Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer render in renders)
                render.material.color -= Color.yellow;
                */
        }
    }
    //Función del movimiento
    #region movimiento
    void MoveZombies(GameObject zombie, Vector3 desiredPosition)
    {
        Debug.Log("Hi");

        //En caso de que existan zombies en la lista de _keptSelectedZombies
        if (_keptSelectedZombies.Contains(zombie))
        {
            //Agregamos el zombie a la lista
            _selectedZombies.Add(zombie);

            //Se hace una referencia al script de movimiento de cada zombie a cada iteración
            ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();

            //El booleano wasCommanded se pone en true, ya que se les ha ordenador moverse
            zombieMovement.wasCommanded = true;

            //Función que mueve a los zombies
            zombieMovement.MoveTo(desiredPosition);

        }

    }
    #endregion
}
