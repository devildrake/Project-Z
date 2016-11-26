using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogicScript : MonoBehaviour
{

    //flag to check if the user has tapped / clicked. 
    //Set to true on click. Reset to false on reaching destination
    //destination point
    private Vector3 endPoint;
	//alter this to change the speed of the movement of player / gameobject
	public float duration = 50.0f;
	//vertical position of the gameobject
	private float yAxis;

    public LayerMask mask = 8;
    public LayerMask mask2 = 9;
    InputHandlerScript _input;
   // public GameObject zombie;
    //Listas de zombies
    public List<GameObject> _zombies;
    public List<GameObject> _selectedZombies;
    public List<GameObject> _keptSelectedZombies;

    //Cuadro de selección
    public GameObject _selectionBox;

    //Origen de la selección actual
    public Vector3 _selectionOrigin;

    //Con esta variable sabemos si hemos comenzado una selección
    public bool _selecting;

    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;

    // Use this for initialization
    void Start()
    {
        
		yAxis = gameObject.transform.position.y;
        //Guardamos la referencia al input en nuestra clase
        _input = this.GetComponent<InputHandlerScript>();

        //Inicializamos las listas
        _zombies = new List<GameObject>();
        _selectedZombies = new List<GameObject>();
        _keptSelectedZombies = new List<GameObject>();

        //Vamos a crear 3 zombies
        GameObject zombie = Resources.Load("ZombieObject") as GameObject;
     
        GameObject zombie1 = GameObject.Instantiate(zombie, /*new Vector3(245, 0.5f, 61)*/position1, Quaternion.identity) as GameObject;
        GameObject zombie2 = GameObject.Instantiate(zombie,/* new Vector3(250, 0.5f, 61)*/position2, Quaternion.identity) as GameObject;
        GameObject zombie3 = GameObject.Instantiate(zombie,/* new Vector3(240, 0.5f, 61)*/position3, Quaternion.identity) as GameObject;



        //Añadimos los zombies a la lista
        _zombies.Add(zombie1);
        _zombies.Add(zombie2);
        _zombies.Add(zombie3);
        Debug.Log(Camera.main.WorldToScreenPoint(zombie1.transform.position));
    }

    // Update is called once per frame
    void Update()
    {

       
        DrawSelectionBox();
        UpdateSelection();
        UpdateSelection2();
		if ((Input.GetMouseButtonDown (1))) {
			//declare a variable of RaycastHit struct
			RaycastHit hit;
			//Create a Ray on the tapped / clicked position
			Ray ray;
			//for unity editor
			#if UNITY_EDITOR
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            
            //for touch device
            /*		#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);*/
#endif

            //Check if the ray hits any collider
            if (Physics.Raycast (ray, out hit,80,mask)) {
                //set a flag to indicate to move the gameobject
               
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
                //save the click / tap position
                endPoint = hit.point;
				//this.gameObject.transform.LookAt(hit.point);
				//as we do not want to change the y axis value based on touch position, reset it to original y axis value
				endPoint.y = yAxis;

               

                int i = 0;
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
                            //  Debug.Log(endPoint);
                        }
                        MoveZombies(zombie, endPoint + desplazamientoFinal);

                        i++;
                    }
                }
			}

		}


    }
    bool IsNotAlive(GameObject z)
    {
        return !z.GetComponent<ZombieScript>().isAlive;
    }
    void UpdateSelection2() {
        _selectedZombies.RemoveAll(IsNotAlive);
        _keptSelectedZombies.RemoveAll(IsNotAlive);
    }

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
                
                
                
                //AQUI ESTA EL PROBLEMA DEL RAYO DE SELECCION
                if (Physics.Raycast(ray, out hit))
                {
                    //Guardamos el punto tridimensional en el que colisiona nuestro rayo.
                    _selectionOrigin = hit.point;

                    //Creamos el cuadro de selección
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
                //Destruimos el cuadro de selección
                Destroy(_selectionBox);
            }
            else
            {
                //Estos son los límites de nuestro cuadro de selección
                Rect bound = _selectionBox.GetComponent<GUITexture>().pixelInset;

                //Con esta sencilla función pasamos el origen de la selección a coordenadas de pantalla
                Vector3 selectionOriginBox = Camera.main.WorldToScreenPoint(_selectionOrigin);

                //Recogemos los límites de nuestro cuadro en función del punto de origen y la posición actual del puntero
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
                //Si no mantenemos la selección
                if (!_input._keepSelection && !_input._invertSelection)
                {
                    //Desmarcamos los cazas
                    foreach (GameObject zombie in _selectedZombies)
                    {
                        if (zombie != null)
                        {
                            Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
                            foreach (Renderer render in renders)
                                render.material.color -= Color.yellow;
                        }
                    }

                    //Limpiamos las listas de zombies seleccionados
                    _selectedZombies.Clear(); //Esta no es necesario limpiarla ya
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
                    }
                //Indicamos que hemos finalizado nuestra selección
                _selecting = false;
            }
            else
            {
                RaycastHit hit;
                Ray ray;

                //Buscamos las unidades y edificios que se encuentren dentro de la caja de selección
                List<GameObject> zombiesInSelectionBox = new List<GameObject>();

                //Dado que no se puede modificar una lista mientras la estás recorriendo,
                //es mejor utilizar listas alternaticas para agregar y remover

                //Lista de zombies que añadiremos a la selección
                List<GameObject> zombiesToAdd = new List<GameObject>();

                //Lista de zombies que removeremos de la selección
                List<GameObject> zombiesToRemove = new List<GameObject>();

				List<GameObject> zombiesToMove = new List<GameObject> ();

                //Primero lanzamos un rayo para guardar el punto de finalización de la selección
                ray = Camera.main.ScreenPointToRay(_input._mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //Este es el plano tridimensional de selección
                    Rect selectionPlane = new Rect();
                    
               /*     selectionPlane.xMin = Mathf.Min(_selectionOrigin.x, _input._mousePosition.x);
                    selectionPlane.yMin = Mathf.Min(_selectionOrigin.z, _input._mousePosition.z);
                    selectionPlane.xMax = Mathf.Max(_selectionOrigin.x, _input._mousePosition.x);
                    selectionPlane.yMax = Mathf.Max(_selectionOrigin.z, _input._mousePosition.z);
                    */
                    selectionPlane = _selectionBox.GetComponent<GUITexture>().pixelInset;

                    Debug.Log(selectionPlane.xMax);
                    Debug.Log(selectionPlane.yMax);
                    Debug.Log(selectionPlane.xMin);
                    Debug.Log(selectionPlane.yMin);

                   Debug.Log("Z" + Camera.main.WorldToScreenPoint( _zombies[1].transform.position));



                    //Comprobamos que el rayo no golpea directamente en una unidad

                    if (_zombies.Contains(hit.collider.gameObject))
                    {//Si el collider estuviera en el propio objeto

                        //En nuestro caso los colider están en los componentes hijos del FighterObject, por lo que debemos acceder al padre
                        // if (hit.collider.gameObject.transform.parent != null && this._zombies.Contains(hit.collider.gameObject.transform.parent.gameObject))
                        {
                            //Esta comprobación es necesaria, ya que al coger un único punto de referencia de los zombies, si éste punto no está dentro del cuadro, no lo seleccionaría
                            zombiesInSelectionBox.Add(hit.collider.gameObject);
                        }
                    }

                    //Agregamos a la lista los zombies que se encuentran dentro del cuadro de selección
                    foreach (GameObject zombie in this._zombies)
                    {
                        if (!zombiesInSelectionBox.Contains(zombie) && (Camera.main.WorldToScreenPoint(zombie.transform.position).x >= selectionPlane.xMin && Camera.main.WorldToScreenPoint(zombie.transform.position).x <= selectionPlane.xMax && Camera.main.WorldToScreenPoint(zombie.transform.position).y >= selectionPlane.yMin && Camera.main.WorldToScreenPoint(zombie.transform.position).y <= selectionPlane.yMax))
                        {
                            zombiesInSelectionBox.Add(zombie);
                        }
                    }
                }

                foreach (GameObject zombies in zombiesInSelectionBox)
                {
                    if (zombies != null)
                    {
                        if (!_input._invertSelection)
                        {
                            //Si no está pulsada la tecla de invertSelection seleccionamos los cazas del cuadro
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
                        //Si está pulsada la tecla de invertSelection removemos los cazas del cuadro
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
                        if (zombie != null) {
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

				/*foreach (GameObject zombie in zombiesToMove) 
				{
					MoveZombies(zombie);
				}*/
            }
        }
    }

    void SelectZombie(GameObject zombie)
    {
        //Comprobamos que el caza no esté ya seleccionado
        if (!_selectedZombies.Contains(zombie))
        {	
            //Agregamos el zombie a la lista
            _selectedZombies.Add(zombie);
            //Marcamos el zombie de color amarillo
            Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
			foreach (Renderer render in renders) {
				render.material.color += Color.yellow;
			}
        }
    }

    void DeselectZombie(GameObject zombie)
    {
        if (_selectedZombies.Contains(zombie))
        {
            //Removemos el zombie de la lista
            _selectedZombies.Remove(zombie);
            //Desmarcamos el zombie
            Component[] renders = zombie.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer render in renders)
                render.material.color -= Color.yellow;
			
        }
    }

	void MoveZombies(GameObject zombie, Vector3 desiredPosition)
	{
        Debug.Log("Hi");

        if (_keptSelectedZombies.Contains (zombie)) {

            //Agregamos el zombie a la lista
            _selectedZombies.Add (zombie);

            ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();


            zombieMovement.MoveTo(desiredPosition);


        }

    }
}