using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestPlayerMovement : MonoBehaviour {

    PlayerMovement pMove;

	// Use this for initialization
	void Start () {
        pMove = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            input.x = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            input.z = -1;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            pMove.SaltoNormalInputDown();
        }else if(Input.GetKeyUp(KeyCode.Space))
        {
            pMove.SaltoNormalInputUp();
        }
        pMove.SetDirectionalInput(input.normalized, Controller3D.TipoMovimiento.Estricto);
    }
}
