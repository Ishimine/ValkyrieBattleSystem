using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlladorDeBatalla : MonoBehaviour {

    public static ControlladorDeBatalla instance;
    public Escuadron[] escuadrones;

    [SerializeField] private int idEscuadronAct;
    [SerializeField] private int idEscuadronObj;
    [SerializeField] private int idUnidadAct;
    [SerializeField] private int idUnidadObj;


    public GameObject flechaObj;
    public GameObject flechaAli;



    public delegate void trigger();
    public trigger ActCambioTurno;
    public delegate void unidadSel(Transform g);
    public event unidadSel selObj;
    public event unidadSel desObj;

    public delegate void actEscuadron(Escuadron e);
    public event actEscuadron actEsc;
    public delegate void actUnidad(UnidadBatalla u);
    public actUnidad actU;
    public actUnidad actAtacado;


    void Awake()
    {
        if (instance == null)
        {
            InicializacionTest();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void InicializacionTest()
    {
        idEscuadronAct = 0;
        idEscuadronObj = 1;
        escuadrones[0].SetId(0);
        escuadrones[0].SetMirandoDer(true);
        escuadrones[0].InicializarTest();
        escuadrones[1].SetId(1);
        escuadrones[1].SetMirandoDer(false);
        escuadrones[1].InicializarTest();

        escuadrones[0].SetLayer("Escuadron_1/Unidad", "Escuadron_1/Arma", "Escuadron_2/Unidad");
        escuadrones[1].SetLayer("Escuadron_2/Unidad", "Escuadron_2/Arma", "Escuadron_1/Unidad");
        if (actEsc != null) actEsc(escuadrones[idEscuadronAct]);


        foreach(Escuadron e in escuadrones)
        {
            e.sinAcciones = CambioDeTurno;
        }
        escuadrones[idEscuadronAct].InicioTurno();
    }

    public void InicializarUI()
    {
        if (actEsc != null) actEsc(escuadrones[idEscuadronAct]);        
    }


    public void CambioDeTurno()
    {
        if (ActCambioTurno != null) ActCambioTurno();
        int aux = idEscuadronAct;
        idEscuadronAct = idEscuadronObj;
        idEscuadronObj = aux;
        idUnidadAct = 0;
        idUnidadObj = 0;
        ActFlechaAli(0);
        ActFlechaObj(0);
        if (actEsc != null)
        {
            actEsc(escuadrones[idEscuadronAct]);
        }
        if (actU != null) actU(escuadrones[idEscuadronObj].unidades[idUnidadObj]);
        escuadrones[idEscuadronAct].InicioTurno();
    }

    public void Atacar(int x)
    {
        SetUnidadAtk(x);
        Atacar(idEscuadronAct,idUnidadAct,idEscuadronObj,idUnidadObj);

        
        if (selObj != null) selObj(escuadrones[idEscuadronAct].unidades[idUnidadAct].transform);
    }

    public void SetUnidadAtk(int x)
    {
        if(!escuadrones[idEscuadronAct].GetMirandoDer())
        {
            if (x == 3) x = 0;
            else if (x == 0) x = 3;
        }
        ActFlechaAli(x);
        idUnidadAct = x;
    }



    private void Atacar(int escAtk, int uAtk,int escDef,int uDef)
    {
        UnidadBatalla atacante = escuadrones[escAtk].GetUnidad(uAtk);
        UnidadBatalla defensor = escuadrones[escDef].GetUnidad(uDef);
        if(atacante == null || defensor == null)
        {
            Debug.Log("Una de las unidades involucradas se encuentra muerta");
            return;
        }

        if (actU != null) actU(defensor);

        defensor.PjAtacado = UI_Batalla.instance.ActPjAtacado;
        Atacar(atacante, defensor);
    }

    private void Atacar(UnidadBatalla atacante, UnidadBatalla defensor)
    {
        //Preparar UI para ataque2
        atacante.Atacar(defensor);
    }

    /// <summary>
    /// Usar esto para trabajar con el sistema de formaciones de batalla
    /// </summary>
    /// <param name="dir"> 1=Adelante, 2=Abajo, 3=Arriba, 4=Atras</param>

        
    public void CambiarUnidadObjetivo(int dir)
    {
        if (desObj != null) desObj(escuadrones[idEscuadronObj].unidades[idUnidadObj].transform);

        if (!escuadrones[idEscuadronObj].GetMirandoDer())
        {
            if (dir == 0) dir = 3;
            else if (dir == 3) dir = 0;
        }
        SetUnidadObjetivo(dir);

        if (selObj != null) selObj(escuadrones[idEscuadronObj].unidades[idUnidadObj].transform); // Se envia la unidad seleccionada

        if (actU != null) actU(escuadrones[idEscuadronObj].unidades[idUnidadObj]);
        //escuadrones[idEscuadronObj].unidades[idUnidadObj].EnviarVida();
    }


    public void SetUnidadObjetivo(int x)
    {
        ActFlechaObj(x);
        idUnidadObj = x;
    }

    void ActFlechaObj(int x)
    {
        flechaObj.transform.position = escuadrones[idEscuadronObj].posiciones[x].position + Vector3.up * 9;
    }

    void ActFlechaAli(int x)
    {
        flechaAli.transform.position = escuadrones[idEscuadronAct].posiciones[x].position + Vector3.up * 9;
    }

    public void UnidadAtaqueTerminado(UnidadBatalla unidad)
    {
        if (desObj != null) desObj(unidad.transform);
    }
    
}
