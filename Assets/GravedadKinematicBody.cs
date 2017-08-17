using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class GravedadKinematicBody : MonoBehaviour {

    Vector2 gravedad;
    Rigidbody rb;
    Vector3 desplazamiento;
    public bool encendido;
    public bool EnPiso;
    public Collider col;
    Animator anim;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        gravedad = Physics.gravity;	
	}



   

    private void FixedUpdate()
    {
        if ((transform.position.y <= .5f) || !encendido )
        {
            return;
        }
        else
        {
            CalcularVelocidad();
            AplicarVelocidad();
        }
    }

    void CalcularVelocidad()
    {
        desplazamiento = new Vector3(0,gravedad.y) * Time.fixedDeltaTime;
        desplazamiento += Vector3.up * anim.velocity.y * Time.fixedDeltaTime;

    }

    void AplicarVelocidad()
    {
        transform.position = transform.position + desplazamiento;
    }

    /*
    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Terreno")
        {
            //Debug.Log("En piso");
            EnPiso = true;
        }
    }*/

}
