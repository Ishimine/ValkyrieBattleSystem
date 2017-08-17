using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contiene los eventos que deben ser llamados por las animaciones
/// </summary>
public class ContactoAnimEstados : MonoBehaviour {

    public enum Fuerza {Debil,Medio,Fuerte};
    

    public UnidadBatalla ub;
    public ControladorDeEstados estados;

    public Arma arma;

    public Vector2 fDirAtaqueActual = Vector2.zero;


  

    private void Awake()
    {
        estados = ub.estados;
    }


    public void SetFuerzaX(float x)
    {
        fDirAtaqueActual.x = x;
    }
    public void SetFuerzaY(float y)
    {
        fDirAtaqueActual.y = y;
    }

    public void AtaqueTerminado()
    {
        estados.AtaqueTerminado();
        AnimPostAtaque();
    }

    public void AtaqueIniciado()
    {
        arma.SetFuerza(fDirAtaqueActual);
        arma.AtaqueIniciado();
        AnimPreAtaque();
    }

    public void AnimPreAtaque()
    {
        if (arma != null)
            arma.AnimPreAtaque();
    }

    public void AnimDeAtaque()
    {
        if (arma != null)
            arma.AnimDeAtaque();
    }

    public void AnimPostAtaque()
    {
        if (arma != null)
            arma.AnimPostAtaque();
    }
    
}
