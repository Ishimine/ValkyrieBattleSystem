using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTest : MonoBehaviour
{

    [SerializeField] public List<Transform> objetivos = new List<Transform>();

    public float altura = 10;
    public float margen = 10;
   [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    public Transform Centro;

    public Vector3 objetivo;
    public Vector3 posCamara;

    public float distX;

    public float distCamara;

    private Vector3 vel;
    public float suavisado;
    public float velMax;
    private ControlladorDeBatalla cBat;

    private void Start()
    {
        cBat = ControlladorDeBatalla.instance;
        cBat.ActCambioTurno += VaciarObjetivos;
        cBat.desObj += EliminarObjetivo;
        cBat.selObj += AgregarObjetivo;
    }

    public void VaciarObjetivos()
    {
        objetivos.Clear();
    }

    private void Update()
    {

        if(objetivos.Count < 2)
        {
            posCamara = new Vector3(0, altura, -50);
        }
        /*else if (objetivos.Count == 1)
        {
            posCamara = new Vector3(objetivos[0].position.x - margen/2, Centro.position.y + altura,-50);
        }*/
        else
        {
            CalcularObjetivosLimitrofes();
            PosicionarCentro();
            CalcularAB();
        }
        MoverCamara();
    }

    
    public void PosicionarCentro()
    {
        Centro.position = (A.position + B.position)/2;
    }

    public void CalcularDistanciaAB()
    {
        distX = Vector3.Distance(A.position, B.position);
    }


    public void CalcularPosicionCamaraAB()
    {
        float rad = 75f * Mathf.Deg2Rad;
        distCamara = (distX / 2) / (1 / rad - 1) / 2;
        posCamara = new Vector3(Centro.position.x, altura, Centro.position.z + distCamara);
    }

    void MoverCamara()
    {
        transform.position = Vector3.SmoothDamp(transform.position, posCamara, ref vel, suavisado, velMax);
    }
    
    void Reestablecer()
    {
        A.transform.position = new Vector3(-25, 0, 0);
        B.transform.position = new Vector3(25, 0, 0);
    }

    void CalcularAB()
    {
        PosicionarCentro();
        CalcularDistanciaAB();
        CalcularPosicionCamaraAB();
    }

    void CalcularObjetivosLimitrofes()
    {
            Vector3 auxA;
            Vector3 auxB;
            auxA = Vector3.right * 9999;
            auxB = Vector3.left * 9999;




            for (int i = 0; i < objetivos.Count; i++)
            {
                if (objetivos[i].position.x < auxA.x)
                {
                    auxA = objetivos[i].position;
                }
                if (objetivos[i].position.x > auxB.x)
                {
                    auxB = objetivos[i].position;
                }
            }
            A.position = auxA + Vector3.left * margen;
            B.position = auxB + Vector3.right * margen;
        
    }

    public void AgregarObjetivo(Transform t)
    {
       // Debug.Log("Agregado Objetivo: " + t.name);

        if (!objetivos.Exists(x => x == t))
            objetivos.Add(t);
    }
    public void EliminarObjetivo(Transform t)
    {
       // Debug.Log("Eliminado Objetivo: " + t.name);

        objetivos.Remove(t);
    }

}