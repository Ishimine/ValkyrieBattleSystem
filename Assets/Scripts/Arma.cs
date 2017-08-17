using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arma : MonoBehaviour {

    public UnidadBatalla unidad;
    public int daño;
    public GameObject objetivo;
    public bool atacando;
    public SpriteRenderer render;

    public Animator anim;
  [HideInInspector]  public Vector2 fuerzaAtaqueAct;
    public int layerObjetivo;

    abstract public void AnimPreAtaque();

    abstract public void AnimDeAtaque();

    abstract public void AnimPostAtaque();

    abstract public void AtaqueTerminado();

    abstract public void AtaqueIniciado();

    public void SetFuerza(Vector2 dir)
    {
        fuerzaAtaqueAct = dir;
    }

    public void SetDaño(int n)
    {
        daño = n;
    }

    public void SetObjetivo(GameObject objetivo)
    {
        this.objetivo = objetivo;
    }

    private void OnValidate()
    {
        AplicarLayer();
    }

    public void AplicarLayer()
    {
        render.sortingOrder = layerObjetivo;
    }

}
