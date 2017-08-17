using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(UnidadBatalla))]
public class RigidbodyAnimator : MonoBehaviour {

    Rigidbody rb;
    Animator anim;
    UnidadBatalla unidad;
    Vector3 velActual;
    Vector3 velFinal;
    Vector3 rootMotionVel;

    private void Awake()
    {
        unidad = GetComponent<UnidadBatalla>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    public void FixedUpdate()
    {
        CalcularVelocityFinal();
        AplicarVelocidadFinal();
    }

    public void AplicarVelocidadFinal()
    {
    }      

    public void CalcularVelocityFinal()
    {
        velFinal = rootMotionVel + rb.velocity;
    }

    public void InRootMotionMovement(Vector2 x)
    {
        if (unidad.MirandoDer) rootMotionVel = x;
        else rootMotionVel = new Vector2(-x.x, x.y);
    }



}
