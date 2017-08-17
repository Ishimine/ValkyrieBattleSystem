using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnidadBatalla : MonoBehaviour {
    UnidadMovimiento mov;
    private bool vivo = true;       
    private Escuadron escuadron;    //Escuadron al que pertenece
    private int idDePosicion;       //Id de la posicion en el escuadron

    public GameObject cuerpo;       //GameObject que contiene la visual del personaje

    private Transform posOrigen;
    public float distAtaque = 3;
    private Vector3 vel;
    public float esperaPostAtk = 1.5f;
    public float velSuavizado;
    public float velMax;
    public float velMin;
    public float radioAreaLlegada = 1;
    private UnidadBatalla objetivo;

    public PlayerMovement pMove;
    Controller3D control;

    private IEnumerator esperaPostAtaque;
    /// <summary>
    /// Tiempo que tarda la unidad en trasladarse 
    /// </summary>
    public float tiempoDeLlegadaAtaque = 0.75f;
    public float tiempoDeLlegadaRetorno = 2f;
    private bool movimientoDeAtaque = false;

    [SerializeField] public UnidadEstadisticas estadisticas;
    [SerializeField] public ControladorDeEstados estados;
    public Combo combo;


    public GameObject objeto_1;
    public GameObject objeto_2;

    public ContactoAnimEstados animEstados;

    public Arma arma;

    public float fGolpeado = 5;


   [SerializeField] private bool mirandoDer;
    public bool MirandoDer
    {
        get
        {
            return mirandoDer;
        }
        set
        {
            if (!value)
            {
                pMove.controller.collisions.faceDir = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                pMove.controller.collisions.faceDir = 1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            mirandoDer = value;
        }        
    }
    
    public delegate void ActPj(UnidadBatalla u);
    public ActPj PjAtacado;

    public delegate void Trigger();
    public Trigger sinAcciones;



    private void Awake()
    {


        pMove = GetComponent<PlayerMovement>();
        estadisticas.UnidadMuerta = UnidadMuerta;
        CargarArma();

        estados.atkTerminado += AtaqueTerminado2;
        control = GetComponent<Controller3D>();
        control.velocityAplicada += estados.ActualizarVelocity;


        //estadisticas.ReiniciarEstadisticas();
    }

    public bool EstaViva()
    {
        return vivo;
    }
    public int GetId()
    {
        return idDePosicion;
    }
    public void SetId(int x)
    {
        idDePosicion = x;
    }
    internal void PosicionarEn(Vector3 pos)
    {
        transform.position = pos;
    }


    public void InicializarUnidad() //Inicializa la unidad al iniciar el turno
    {
        combo.InicializarCombo();
    }  

    public void CargarArma(/*GameObject n*/)
    {
        string t = "Armas/Espada";
       /* int i = UnityEngine.Random.Range(0,2);
        if (i == 0) t = "Armas/Espada";
        else t = "Armas/EspadaGrande";
        */

        GameObject clone = Instantiate<UnityEngine.Object> (Resources.Load(t),objeto_1.transform) as GameObject;
        arma = clone.GetComponent<Arma>();
        animEstados.arma = arma;
        arma.SetDaño(estadisticas.GetAtaque());
        arma.unidad = this;
        

    }
    
    float CalcularVelocidad(float dist, float tiempo)
    {
        float vel = dist / tiempo;
        if (vel < velMin) vel = velMin;
        else if (vel > velMax) vel = velMax;

        //Debug.Log("Veclocidad calculada: " + vel);  
        return vel;
    }

    IEnumerator Movimiento(Transform obj, bool atacar)
    {
        Vector3 dist = Vector3.zero;
        Vector3 posObj = new Vector3(obj.position.x,transform.position.y, obj.position.z);
        Vector3 dir = posObj - transform.position;
        estados.SetEnMovimiento(true);

        float a = 0;
        if (atacar)
        {
            estados.AtaqueIniciado();
            a = CalcularVelocidad(dir.magnitude, tiempoDeLlegadaAtaque);
            pMove.SetVelocidad(a);
            dist = distAtaque * Vector3.right;
            if (dir.x < 0)
                dist *= -1;
            posObj -=  dist;
        }        
        else
        {
            a = CalcularVelocidad(dir.magnitude, tiempoDeLlegadaRetorno);
            pMove.SetVelocidad(a);
        }
   /*     print(gameObject.name+"  Movimiento Iniciado Atacar: " + atacar);
        print(gameObject.name+"  Velocidad: " + a);*/

        while (Mathf.Abs(dir.magnitude) > radioAreaLlegada)
        {



     /*       Debug.Log("Mathf.Abs(dir.magnitude)" + Mathf.Abs(dir.magnitude));
            Debug.Log("radioAreaLlegada" + radioAreaLlegada);*/

            MirarObjetivo(posObj);
            pMove.SetDirectionalInput(dir.normalized,Controller3D.TipoMovimiento.Dependiente);              //Envia al playerMovementeLaDireccionDelMovimiento
            yield return null;
            dir = posObj - transform.position;
           // Debug.Log(" dir = posObj - transform.position" +  posObj + " - "+ transform.position);
        }

     //   print(gameObject.name + "  Movimiento Terminado");

        if (atacar)//Una ves trasladado si el movimiento requeria un ataque lo ejecuta
        {
            movimientoDeAtaque = false;
            MirarObjetivo(objetivo.transform.position);
            Atacar();
        }
        else // Una vez trasladado si el movimiento NO requeria un ataque chequea si es un movimiento de retorno
        {
            MirarAdelante();

            if (combo.terminado)
            {
                if (sinAcciones != null) sinAcciones();
            }            
        }
        pMove.SetDirectionalInput(Vector3.zero, Controller3D.TipoMovimiento.Dependiente);
       
        estados.SetEnMovimiento(false);
    }

    private void AtaqueTerminado2()
    {
        if(arma.atacando)            arma.AtaqueTerminado();
        ControlladorDeBatalla.instance.UnidadAtaqueTerminado(this);
        if (!movimientoDeAtaque)
        {
            esperaPostAtaque = EsperaPostAtaque(esperaPostAtk);
            StartCoroutine(esperaPostAtaque);
        }
    }



    IEnumerator EsperaPostAtaque(float x)
    {
        //print(gameObject.name + "Espera Post ataque iniciada. " + Time.time);

        yield return new WaitForSeconds(x);
        //print(gameObject.name + "Espera Post ataque Terminada" + Time.time);

        MoverHasta(posOrigen, false);

    }

    private void MirarObjetivo(Vector3 pos)
    {
        if (pos.x - transform.position.x > 0)
            Mirar(true);
        else
            Mirar(false);
    }

    private void MirarAdelante()
    {
        if (escuadron.GetMirandoDer()) Mirar(true) ;
        else Mirar(false);
    }

    public void MoverHasta(Transform obj, bool x)
    {
        estados.SetEnMovimiento(true);

        //if(esperaPostAtaque != null) StopCoroutine(esperaPostAtaque);
       // StopAllCoroutines();
        StartCoroutine(Movimiento(obj, x));
    }        

    public void SetNewPosition(Transform x)
    {
        posOrigen = x;
    }

    public void Atacar(UnidadBatalla obj)
    {
        if(estados.Atacando() || combo.terminado || movimientoDeAtaque)
        {
           // Debug.Log("Ataque denegado. A:"+estados.Atacando()+ " ComboT: "+combo.terminado+ "Mov"+ movimientoDeAtaque);
            return;
        }



        /*Debug.Log("Ataque Aceptado. A:" + estados.Atacando() + " ComboT: " + combo.terminado + "Mov" + movimientoDeAtaque);
        Debug.Log("//////////////////////////////////////////");*/

        objetivo = obj;
        arma.SetObjetivo(obj.gameObject);
        arma.SetDaño( estadisticas.GetAtaque());
        movimientoDeAtaque = true;

        //StopCoroutine(EsperaPostAtaque());
        //estados.AtaqueIniciado();
        if (esperaPostAtaque != null) StopCoroutine(esperaPostAtaque);
        MoverHasta(obj.transform, true);


    }

    private void Atacar()
    {        
        estados.Atacar(combo.SigAtaque());
    }   



    /// <summary>
    /// Aplica el layer designado al escuadron
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void SetLayer(string x,string y, string z)
    {
        gameObject.layer = LayerMask.NameToLayer(x);
        arma.gameObject.layer = LayerMask.NameToLayer(y);
        arma.layerObjetivo = LayerMask.NameToLayer(z);
    }

    /// <summary>
    /// Guarda el escuadron al que pertenece la unidad
    /// </summary>
    /// <param name="e"></param>
    public void SetEscuadron(Escuadron e)
    {
        escuadron = e;
    }


    /// <summary>
    /// Setea unidad objetivo durante una rutina de ataque
    /// </summary>
    /// <param name="unidad"></param>
    public void SetObjetivo(UnidadBatalla unidad)
    {
        objetivo = unidad;
    }

    public void UnidadMuerta()
    {
        ControlladorDeBatalla.instance.UnidadAtaqueTerminado(this);
        vivo = false;
        escuadron.ReportarUnidadMuerta(this);
        estados.Muerto();
        StopAllCoroutines();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="daño">Cantidad de daño en bruto del ataque</param>
    /// <param name="tipo">Tipo de daño de la unidad</param>
    /// <param name="pos"></param>
    /// <param name="fuerza"></param>
    public void RecibirDaño(int daño, UnidadEstadisticas.tipoDaño tipo, bool desdeDer, Vector2 fuerza)
    {
        estadisticas.RecibirDaño(daño, tipo);
        estados.KnockBack();


        Mirar(desdeDer);

        /*
        if (mirandoDer)
            pMove.AplicarFuerza(new Vector2(fuerza.x*-1,fuerza.y));
        else*/
        pMove.AplicarFuerza(fuerza);


   

        if (vivo)
        {
            StopAllCoroutines();
            StartCoroutine(EsperaPostAtaque(3f));
        }
        if (PjAtacado != null)      //Informa que el pj fue atacado y envia una referencia del mismo
        {
            PjAtacado(this);
        }
    }


    private void MirarDir(float x)
    {
        if (x - transform.position.x < 0)
        {
            Mirar(false);
        }
        else
        {
            Mirar(true);
        }
    }

    void Mirar(bool der)
    {
        if (der)
        {
            MirandoDer = true;    
        }
        else
        {
            MirandoDer = false;
        }

    }

    public void AplicarCapaEnArma(int capa)
    {
        if (arma != null)
            arma.render.sortingOrder = capa;
    }


    public void ActualizarVida()
    {
        print("kkkkk");
    }

}
