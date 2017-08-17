using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Escuadron : MonoBehaviour {

    public Escuadron escuadron;
    public UI_PersonajeEtiqueta[] etiquetas;

    public void ActualizarEscuadron(Escuadron e)
    {
        escuadron = e;
        EnlazarUI();
    }

    public void EnlazarUI()
    {
        //Debug.Log("escuadron: "+ escuadron.name);
        for(int i = 0; i<escuadron.unidades.Length; i++)
        {
            etiquetas[i].Unidad = escuadron.unidades[i];
        //Debug.Log("Unidad " +i+ ": " + escuadron.unidades[i].name);

        }
    }


}
