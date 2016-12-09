using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuckerfuckingfuck : MonoBehaviour {
    AstarPath elPath;
    public bool algoPasa;
	// Use this for initialization
	void Start () {
        algoPasa = false;
        elPath = gameObject.GetComponent<AstarPath>();
	}
	
	// Update is called once per frame
	void Update () {
        if (algoPasa) {

            elPath.Scan();
            algoPasa = false;
        }
	}
}
