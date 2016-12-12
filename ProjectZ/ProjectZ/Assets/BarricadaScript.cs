using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadaScript : MonoBehaviour {
    public int health;
    public AstarPath elPathfinder;
    public List<GameObject> _atacantes;
    public float contador;
    public float tiempoAContar;

	// Use this for initialization
	void Start () {
        elPathfinder = GameObject.FindObjectOfType<AstarPath>();
        health = 100;
        tiempoAContar = 0.5f;
	}

    void loseHp() {
        health -= 5;
    }
	// Update is called once per frame
	void Update () {

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
	}
    private void OnDestroy()
    {
        elPathfinder.Scan();
    }
}
