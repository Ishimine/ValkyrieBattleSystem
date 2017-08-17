using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estadistiscas De unidades
/// Arreglo Estadisticas*
///         0 = Vida
///         1 = Ataque
///         2 = Armadura Fisica
///         3 = Resistencia Magica
///         4 = Resistencia Agua
///         5 = Resistencia Tierra
///         6 = Resistencia Fuego
///         7 = Resistencia Aire        
/// </summary>
[System.Serializable]
public class UnidadEstadisticas
{
    public string nombre;
    public delegate void trigger();
    public trigger UnidadMuerta;

    public enum tipoDaño {Fisico, Magico, Agua, Tierra, Fuego, Aire };




    [SerializeField] int[] estadisticasOriginales = new int[8];
    [SerializeField] int[] estadisticasBase = new int[8];
    [SerializeField] int[] estadisticasActuales = new int[8];



   

    
    public void RestaurarEstadisticas()
    {
        for (int i = 0; i<estadisticasActuales.Length;i++)
        {
            estadisticasActuales[i] = estadisticasBase[i];
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="daño"></param>
    /// <param name="tipo">
    ///         2 = Armadura Fisica
    ///         3 = Resistencia Magica
    ///         4 = Resistencia Agua
    ///         5 = Resistencia Tierra
    ///         6 = Resistencia Fuego
    ///         7 = Resistencia Aire
    /// </param>
    public void RecibirDaño(int daño, tipoDaño tipo)
    {
        float d = daño;
        float def = (float)estadisticasActuales[(int)tipo+2];
        int vida = estadisticasActuales[0];
        float dañoFinal;          
    
        if (def >= 0)
            dañoFinal = ((100 / (100 + def)) * daño);
        else
            dañoFinal = (2 - 100 / (100 - def)) * daño;

        estadisticasActuales[0] -= (int)dañoFinal;        
        if(estadisticasActuales[0] <= 0)
        {
            estadisticasActuales[0] = 0;
            if (UnidadMuerta != null) UnidadMuerta();
        }

    }

    /// <summary>
    /// </summary>
    /// <param name="tipo">    
    ///         1 = Armadura Fisica
    ///         2 = Resistencia Magica
    ///         3 = Resistencia Agua
    ///         4 = Resistencia Tierra
    ///         5 = Resistencia Fuego
    ///         6 = Resistencia Aire</param>
    /// <returns></returns>
    public int GetDef(int tipo)
    {
        return estadisticasActuales[tipo];
    }

    public int GetVidaActual()
    {
        return estadisticasActuales[0];
    }

    public int GetVidaBase()
    {
        return estadisticasBase[0];
    }

    public int GetAtaque()
    {
        return estadisticasActuales[1];
    }
}
