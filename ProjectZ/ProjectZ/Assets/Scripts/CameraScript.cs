using UnityEngine;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    //CONSTANTES DE CÁMARA
    //Velocidad de movimiento de la cámara
    const float CAMERA_SPEED = 30.0f;
    //Margen de pantalla donde se podrá mover la cámara situando allí el ratón
    const int CAMERA_MOVE_MARGIN = 60;
    //Manejador de input
    InputHandlerScript _input;

    // Use this for initialization
    void Start()
    {
        //Guardamos la referencia al input en nuestra clase
        _input = GameObject.Find("GameLogic").GetComponent<InputHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Declaramos un vector velocidad de la Cámara
        Vector3 cameraVector;

        //Comprobamos si el ratón se encuentra en los margenes de movimiento
        CheckMousePosition(out cameraVector);

        //Y ahora comprobamos las entradas del teclado
        if (_input._cameraUp)
            cameraVector.z = CAMERA_SPEED;
        else if (_input._cameraDown)
            cameraVector.z = -CAMERA_SPEED;
        if (_input._cameraRight)
            cameraVector.x = CAMERA_SPEED;
        else if (_input._cameraLeft)
            cameraVector.x = -CAMERA_SPEED;

        //Movemos la cámara en el vector que hemos especificado
        transform.Translate(cameraVector * Time.deltaTime, Space.World);
    }

    void CheckMousePosition(out Vector3 cameraVector)
    {
        cameraVector = new Vector3();

        if (_input._mousePosition.x < CAMERA_MOVE_MARGIN)
        {
            cameraVector.x = -CAMERA_SPEED;
        }
        else if (_input._mousePosition.x > (Screen.width - CAMERA_MOVE_MARGIN))
        {
            cameraVector.x = CAMERA_SPEED;
        }

        if (_input._mousePosition.y < CAMERA_MOVE_MARGIN)
        {
            cameraVector.z = -CAMERA_SPEED;
        }
        else if (_input._mousePosition.y > (Screen.height - CAMERA_MOVE_MARGIN))
        {
            cameraVector.z = CAMERA_SPEED;
        }
    }
}