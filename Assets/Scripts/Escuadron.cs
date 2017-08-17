using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Escuadron : MonoBehaviour {
    
    private bool mirandoDer;

    [SerializeField] private string nombre;
    [SerializeField] private int id;
    int cantUnidadesActivas;            //Cant de unidades existentes y que no an terminado sus acciones
    int cantUnidadesSinAcciones;         //

    public delegate void Trigger();
    public Trigger sinAcciones;


    public UnidadBatalla[] unidades;
    public Transform[] posiciones;

    

    public void SetId(int id)
    {
        this.id = id;
    }

    public void InicializarTest()
    {
        InicializarUnidadesTest();
        ResetPos();
    }

    public void InicioTurno()
    {
        cantUnidadesActivas = 0;
        cantUnidadesSinAcciones = 0;
        foreach (UnidadBatalla u in unidades)
        {
            if (u != null)
            {
                u.InicializarUnidad();
                u.sinAcciones = CheckUnidadesActivas;
                cantUnidadesActivas++;
            }
        }

       

    }

    void CheckUnidadesActivas()
    {
        cantUnidadesSinAcciones++;

        if(cantUnidadesSinAcciones == cantUnidadesActivas)
        {
            if (sinAcciones != null) sinAcciones();
        }

        //print("Activos: " + cantUnidadesActivas);
        //print("Inactivos: " + cantUnidadesSinAcciones);
    }
    

    public void ResetPos()
    {
        Vector2 distAlCentro = Vector3.one * 6;
        if (!mirandoDer) distAlCentro.x *= -1;

        posiciones[0].position = transform.position + new Vector3(distAlCentro.x,0) ;
        posiciones[1].position = transform.position - new Vector3(0, 0,distAlCentro.y);
        posiciones[2].position = transform.position + new Vector3(0, 0,distAlCentro.y);
        posiciones[3].position = transform.position - new Vector3(distAlCentro.x,0);

        for(int i =0; i<unidades.Length;i++)
        {
            UnidadBatalla u = unidades[i];
            if (u !=null)
            {
                u.SetNewPosition(posiciones[i]);
                u.PosicionarEn(posiciones[i].position);
                //Debug.Log(gameObject.name + " MD: " + mirandoDer);
                u.MirandoDer = mirandoDer;
            }
        }
    }

    public void SetLayer(string x, string y, string z)
    {
        foreach (UnidadBatalla item in unidades)
        {
            item.SetLayer(x,y, z);
        }
    }

    public UnidadBatalla GetUnidad(int id)
    {
        if (unidades[id].EstaViva())
            return unidades[id];
        else
            return null;
    }

    

    public void CambiarPosicion(int idUnidad, int idPos)
    {
        UnidadBatalla aux = unidades[idPos];
        if (aux != null)
        {
            aux.SetNewPosition(posiciones[idUnidad]);
            aux.PosicionarEn(posiciones[idUnidad].position);
        }

        unidades[idUnidad].SetNewPosition(posiciones[idPos]);
        unidades[idUnidad].PosicionarEn(posiciones[idPos].position);
        unidades[idPos] = unidades[idUnidad];
        unidades[idPos] = aux;
    }


    public void SetMirandoDer(bool x)
    {
        mirandoDer = x;
    }

    public bool GetMirandoDer()
    {
        return mirandoDer;
    }


    public void CargarUnidades()
    {
        ///this.unidades = AdministradorDeUnidades(id,nombre);      //Deberia cargar el escuadron desde disco usando el id y el nombre
    }
    public void GuardarUnidades(UnidadBatalla[] unidades)
    {
        ///AdministradorDeEscuadrones(unidades, id, nombre);        //Deberia guardar el escuadron en base del id y el nombre
    }


    public void InicializarUnidadesTest()
    {
        unidades = new UnidadBatalla[4];
        for (int i =0; i<unidades.Length;i++)
        {
            GameObject n = Instantiate<Object>(Resources.Load("Unidades/uRar_V2"), posiciones[i]) as GameObject;
            n.name = this.name + "unidad: " + i;
            UnidadBatalla ub = n.GetComponent<UnidadBatalla>();
            unidades[i] = ub;
            unidades[i].estadisticas.nombre = "Roboto " + i;
            ub.SetEscuadron(this);
            ub.combo.ReRollCombo();
        }
    }

    public void ReportarUnidadMuerta(UnidadBatalla unidad)
    {
        id = unidad.GetId();
    }


   
}
