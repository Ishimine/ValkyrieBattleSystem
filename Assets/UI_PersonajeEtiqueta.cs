using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PersonajeEtiqueta : MonoBehaviour {

    public Text nombre;
    public Text tVida;
    public Slider slider;
    public float vida;
    public float valorActual;
    public float valorObjetivo;

    private UnidadBatalla unidad;
    public UnidadBatalla Unidad
    {
        set {
            unidad = value;
            Inicializar();
        }
        get { return unidad; }
    }


    float vel;
    public float suavizado;
    
    public void Inicializar()
    {
        ActualizarNombre(unidad.estadisticas.nombre);
        unidad.PjAtacado = ActualizarVida;
        ActualizarVida(unidad.estadisticas.GetVidaActual(), unidad.estadisticas.GetVidaBase());        
    }    

    void ActualizarNombre(string n)
    {
        nombre.text = n;   
    }

    void ActualizarVida(UnidadBatalla unidad)
    {
        ActualizarVida(unidad.estadisticas.GetVidaActual(), unidad.estadisticas.GetVidaBase());
    }

    void ActualizarVida(float act, float total)
    {
        vida = act;
        valorObjetivo = act/total;
        
        StartCoroutine(Actualizar());
    }

    IEnumerator Actualizar()
    {
        while (valorObjetivo != valorActual)
        {
            valorActual = Mathf.SmoothDamp(valorActual, valorObjetivo, ref vel, suavizado);
            tVida.text = vida.ToString();
            slider.value = valorActual;
            yield return null;
        }
    }
}
