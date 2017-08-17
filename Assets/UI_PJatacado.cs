using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PJatacado : MonoBehaviour {


    private UnidadBatalla unidad;
    public UnidadBatalla Unidad
    {
        set { unidad = value;
            text.text = unidad.estadisticas.nombre;
            unidad.PjAtacado = ActualizarValor;
            ActualizarValor(unidad);
        }
        get { return unidad; }
    }

    public Text vida;
    public Text text;
    public Slider slider;
    float valorActual = 1;
    float ValorActual
    {
        set {
            valorActual = value; }
        get { return valorActual; }
    }

    public float vel;
    public float suavisado;

    float valorObjetivo;

     
    void ActualizarValor(UnidadBatalla unidad)
    {
        ActualizarValor(unidad.estadisticas.GetVidaActual(),unidad.estadisticas.GetVidaBase());
    }

    void ActualizarValor(float act, float total)
    {
        vida.text = act.ToString();
        valorObjetivo = CalcularValorObjetivo(act, total);
        StartCoroutine(Actualizar());
    }

    float CalcularValorObjetivo(float act, float total)
    {
        float x = act/ total;
        return x;
    }

    IEnumerator Actualizar()
    {
        while (valorObjetivo != valorActual)
        {
   
            valorActual = Mathf.SmoothDamp(valorActual, valorObjetivo, ref vel, suavisado);
            slider.value = valorActual;
            yield return null;
        }
    }
}
