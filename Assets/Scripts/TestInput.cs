using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {



    ControlladorDeBatalla cb;

	// Use this for initialization
	void Start () {

        cb = ControlladorDeBatalla.instance;

		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            cb.CambioDeTurno();

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            cb.CambiarUnidadObjetivo(2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cb.CambiarUnidadObjetivo(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cb.CambiarUnidadObjetivo(0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cb.CambiarUnidadObjetivo(3);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            cb.Atacar(3);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            cb.Atacar(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            cb.Atacar(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            cb.Atacar(2);
        }



    }
}
