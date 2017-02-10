using System.Collections;
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
        elPathfinder = FindObjectOfType<AstarPath>();
        health = 100;
        tiempoAContar = 0.5f;
        gameLogic._barricadas.Add(gameObject);
	}

    public void ShowCircle(bool whot) {
        circulo.SetActive(whot);
    }

    public void loseHp() {
        health -= 5;
    }
    // Update is called once per frame
    void Update() {
        if (!gameLogic.isPaused){

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
