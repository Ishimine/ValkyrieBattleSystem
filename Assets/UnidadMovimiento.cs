using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimiento : MonoBehaviour {

    public float velMov = 10;

    private float gravedad;


    private Vector3 velAnim;
    public Vector3 VelAnim { get { return velAnim; } set { velAnim = value; } }
    
    private Vector3 direccionMovimiento;
    public Vector3 DireccionMovimiento { get { return direccionMovimiento; }  set { direccionMovimiento = value; } }

    private Vector3 velFinal;

    private void Awake()
    {
        gravedad = Physics.gravity.y;
    }

    public void FixedUpdate()
    {
        CalcularVelocidad();
        transform.Translate(velFinal);
    }


    public void CalcularVelocidad()
    {
        Vector3 movPotencial = (direccionMovimiento * velMov * Time.deltaTime) + velAnim + (new Vector3(0,gravedad) * Time.deltaTime);


        //CalcularColisionX(ref movPotencial)
        //CalcularColisionY(ref movPotencial)
        //CalcularColisionZ(ref movPotencial)
        velFinal = movPotencial;
    }


    private void CalcularColisionY(ref Vector3 movPotencial)
    {
        if(movPotencial.y != 0)
        {
        }
    }




}
