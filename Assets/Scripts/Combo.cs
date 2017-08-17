using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Combo
{

    public int[] ataquesId = {0,1,2,3}; 
    public int[] orden = {0,1,2};
    public int ataqueAct;
    public delegate void Trigger();
    public Trigger comboTerminado;
    public bool terminado = false;

    

    public void ReRollCombo()
    {
        orden[0] = Random.Range(0,6);
        orden[1] = Random.Range(0,6);
        orden[2] = Random.Range(0,6);
    }



    public void ResetearCombo()
    {
        for (int i=0; i < orden.Length; i++)
        {
            orden[i] = i;
        }
    }
	
    public void InicializarCombo()
    {
        terminado = false;
        ataqueAct = 0;
    }

    public int GetAtaqueAct()
    {
        return ataqueAct;
    }

    public int SigAtaque()
    {
        int x = ataqueAct;
        ataqueAct++;
        if(ataqueAct >= ataquesId.Length)
        {
            ataqueAct = 0;
            if (comboTerminado != null) comboTerminado();
            terminado = true;
        }
        return orden[x];
    }


    public void IntercambiarAtaques(int a, int b)
    {
        int c = orden[a];
        a = b;
        b = c;
    }



}
