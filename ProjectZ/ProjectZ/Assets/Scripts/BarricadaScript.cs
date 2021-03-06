﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadaScript : MonoBehaviour {

    //Atributo que mantiene track de la vida de la barricada
    public float health;

    //Atributo que representa la vida máxima de la barricada
    public float maxHealth;

    //Instancia del objeto A* para poder reescanear
    public AstarPath elPathfinder;

    //Circulo de selección
    public GameObject circulo;

    //Sprite para el circulo
    private SpriteRenderer circuloSprite;

    //Listado de atacantes
    public List<GameObject> _atacantes;

    public sitios [] _posiciones;

    GameLogicScript gameLogic;

    public int aPlaceToAssign = 0;

    public struct sitios
    {
        public Vector3 posicion;
        public bool ocupado;
    }

	// Use this for initialization
	void Start () {
        gameLogic = GameLogicScript.gameLogic;
        gameLogic._barricadas.Add(gameObject);
        circuloSprite = GetComponentInChildren<SpriteRenderer>();
        circulo = circuloSprite.gameObject;
        circulo.SetActive(false);
        circulo.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        elPathfinder = FindObjectOfType<AstarPath>();
        maxHealth = health = 100;
        _posiciones = new sitios[10];
        _posiciones[0].posicion = (gameObject.transform.position) + new Vector3(0.716f, -0.2f, -0.591f);
        _posiciones[1].posicion = (gameObject.transform.position) + new Vector3(0.319f, -0.2f, -0.591f);
        _posiciones[2].posicion = (gameObject.transform.position) + new Vector3(-0.064f, -0.2f, -0.591f);
        _posiciones[3].posicion = (gameObject.transform.position) + new Vector3(-0.434f, -0.2f, -0.591f);
        _posiciones[4].posicion = (gameObject.transform.position) + new Vector3(-0.792f, -0.2f, -0.591f);
        _posiciones[5].posicion = (gameObject.transform.position) + new Vector3(0.716f, -0.2f, 0.428f);
        _posiciones[6].posicion = (gameObject.transform.position) + new Vector3(0.319f, -0.2f, 0.428f);
        _posiciones[7].posicion = (gameObject.transform.position) + new Vector3(-0.064f, -0.2f, 0.428f);
        _posiciones[8].posicion = (gameObject.transform.position) + new Vector3(-0.434f, -0.2f, 0.428f);
        _posiciones[9].posicion = (gameObject.transform.position) + new Vector3(-0.792f, -0.2f, 0.428f);

    }

    void ordenarArray(sitios[] arr, GameObject zombie)
    {
        int minPos;
        sitios tmp;
        for (int i = 0; i < 10; i++)
        {
            minPos = i;

            for (int j = i + 1; j < 10; j++)
            {
                if ((_posiciones[i].posicion-gameObject.transform.position).magnitude < (_posiciones[j].posicion-gameObject.transform.position).magnitude)
                {
                    minPos = j;
                }
            }
            tmp = arr[i];
            arr[i] = arr[minPos];
            arr[minPos] = tmp;
        }
    }

    public void VaciarSitio(int cual) {
        _posiciones[cual].ocupado = false;
    }

    public Vector3 AsignarSitio(GameObject zombie) {
        bool assigned = false;
        int contador = 0;
        Vector3 closestFreeSpot = new Vector3(0,0,0);
        ordenarArray(_posiciones, zombie);
        while (!assigned) {
            if (_posiciones[contador].ocupado)
            {
                contador++;
            }
            else {
                closestFreeSpot = _posiciones[contador].posicion;
                _posiciones[contador].ocupado = true;
                aPlaceToAssign = 0;

                assigned = true;
            }
        }
        return closestFreeSpot;
    }

    //Función que esconde o muestra el circulo en función del parametro whot
    public void ShowCircle(bool whot) {
        circulo.SetActive(whot);
    }

    public void loseHp() {
        health -= 5;
    }
    // Update is called once per frame
    void Update() {
        if (!gameLogic.isPaused&&!gameLogic.eventManager.onEvent){
            if (health / maxHealth * 100 <= 20)
            {
                circulo.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (health / maxHealth * 100 <= 50)
            {
                circulo.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            if (health <= 0) {
                Destroy(gameObject);
            }
        } }

    private void OnDestroy()
    {
        if(gameLogic._barricadas.Contains(gameObject))
        gameLogic._barricadas.Remove(gameObject);
        elPathfinder.Scan();
    }
}
