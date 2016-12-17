using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaDestruidaScript : MonoBehaviour {

    public bool[] sitiosBool = new bool[12];
    public Vector3[] sitiosVec = new Vector3[12];
	// Use this for initialization
	void Start () {
        sitiosVec[0] = gameObject.transform.position + new Vector3(1.44f, 0, -1.29f);
        sitiosVec[1] = gameObject.transform.position + new Vector3(-0.05f, 0, -1.29f);
        sitiosVec[3] = gameObject.transform.position + new Vector3(1.44f, 0, -1.60f);

        /*  sitios[1].posicion = new Vector3(1.44f, 0, 1.16f);
          sitios[2].posicion = new Vector3(-0.05f, 0, -1.29f);
          sitios[3].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[4].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[5].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[6].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[7].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[8].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[9].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[10].posicion = new Vector3(1.44f, 0, -1.29f);
          sitios[11].posicion = new Vector3(1.44f, 0, -1.29f);
          */
        sitiosBool[0] = sitiosBool[1] = sitiosBool[2] = sitiosBool[3] = sitiosBool[4] = sitiosBool[5] = sitiosBool[6] = sitiosBool[7] = sitiosBool[8] = sitiosBool[9] = sitiosBool[10] = sitiosBool[11] = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
