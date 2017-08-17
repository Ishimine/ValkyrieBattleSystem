using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControladorDeEstados {

    [SerializeField] public Animator anim;
    public delegate void Trigger();
    public Trigger atkTerminado;



    private bool atacando;
    private bool muerto;

    public bool Atacando()
    {
        return atacando;
    }

    public void AtaqueTerminado()
    {
        Debug.Log("ControladorEstados: ataque terminado");
        if (atkTerminado != null) atkTerminado();
        atacando = false;

    }

    public void ActualizarVelocity(Vector3 vel)
    {
        anim.SetFloat("VelX",vel.x);
        anim.SetFloat("VelY", vel.y);
        anim.SetFloat("VelZ",vel.z);
    }

    public void Atacar(int x)
    {   
        anim.SetInteger("idAtkAct", x);
        anim.SetTrigger("Atacar");
    }

    public void AtaqueIniciado()
    {
        atacando = true;
    }

    public void KnockBack()
    {
        anim.SetTrigger("KnockBack");
    }

    public void SetEnMovimiento(bool x)
    {
        anim.SetBool("EnMovimiento",x);
    }

    public void SetVel(float vel)
    {
        anim.SetFloat("Vel",vel);
    }
    public void Muerto()
    {
        muerto = true;
        anim.SetBool("Muerto", true);
        SetEnMovimiento(false);        
    }

    public void Revivido()
    {
        muerto = false;
        anim.SetTrigger("Revivido");
    }
}
