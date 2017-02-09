﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadaScript : MonoBehaviour {
    public int health;
    public AstarPath elPathfinder;
    public GameObject circulo;
    private SpriteRenderer circuloSprite;
    public List<GameObject> _atacantes;
    public float contador;
    public float tiempoAContar;
    GameLogicScript gameLogic;

	// Use this for initialization
	void Start () {
        gameLogic = FindObjectOfType<GameLogicScript>();
            circuloSprite = GetComponentInChildren<SpriteRenderer>();
        circulo = circuloSprite.gameObject;
        circulo.SetActive(false);
        circulo.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        elPathfinder = GameObject.FindObjectOfType<AstarPath>();
        health = 100;
        tiempoAContar = 0.5f;
	}

    void HideCircle() {
        circulo.SetActive(false);
    }

    void loseHp() {
        health -= 5;
    }
    // Update is called once per frame
    void Update() {
        if (!gameLogic.isPaused){

            foreach (GameObject p in _atacantes) {
                contador += Time.deltaTime;
            }

            if (contador > tiempoAContar) {
                contador = 0;
                loseHp();
            }

            if (health <= 0) {
                Destroy(gameObject);
            }
        } }
    private void OnDestroy()
    {
        elPathfinder.Scan();
    }
}
