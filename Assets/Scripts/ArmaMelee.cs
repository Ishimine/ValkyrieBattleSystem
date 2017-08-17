using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaMelee : Arma {
    
    public UnidadEstadisticas.tipoDaño tipo = UnidadEstadisticas.tipoDaño.Fisico;




    public TrailRenderer trail;



   

    public override void AnimDeAtaque()
    {
       // throw new NotImplementedException();
    }

    public override void AnimPostAtaque()
    {
        DesactivarTrail();
    }

    public override void AnimPreAtaque()
    {
        ActivarTrail();
    }


    public void ActivarTrail()
    {
        if(trail != null)
            trail.enabled = true;
    }


    public void DesactivarTrail()
    {
        if (trail != null)
            trail.enabled = false;
    }

    public override void AtaqueIniciado()
    {
       
        AnimPreAtaque();
        atacando = true;
    }


    public override void AtaqueTerminado()
    {
        AnimPostAtaque();
        atacando = false;
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject == objetivo && atacando)
        {
            if (unidad.transform.rotation == Quaternion.Euler(0, 180, 0))
            {
                fuerzaAtaqueAct = new Vector2(fuerzaAtaqueAct.x * -1, fuerzaAtaqueAct.y);
            }
            AtaqueTerminado();
            col.GetComponent<UnidadBatalla>().RecibirDaño(daño, tipo, (fuerzaAtaqueAct.x<0), fuerzaAtaqueAct);
            //Debug.Log(fuerzaAtaqueAct);
        }
    }    
    
    


}
