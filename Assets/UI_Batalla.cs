using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Batalla : MonoBehaviour
{
    public static UI_Batalla instance;
    public UI_Escuadron escuadronUI;
    public UI_PJatacado pjAtacadoUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ControlladorDeBatalla.instance.actEsc += AplicarEscuadron;
        ControlladorDeBatalla.instance.actU += ActPjAtacado;
        ControlladorDeBatalla.instance.InicializarUI();       

    }

    public void AplicarEscuadron(Escuadron e)
    {
        escuadronUI.ActualizarEscuadron(e);
    }

    public void ActPjAtacado(UnidadBatalla unidad)
    {
        pjAtacadoUI.Unidad = unidad;
    }

}
