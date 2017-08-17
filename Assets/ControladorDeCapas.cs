using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;



public class ControladorDeCapas : MonoBehaviour {

    UnidadBatalla unidad;
    public SpriteMeshInstance manoDeArma;
    [SerializeField] public SpriteMeshInstance[] renders;
    [SerializeField] public CapaZ[] capaZ;
    float capa;


    private void Awake()
    {
        unidad = GetComponent<UnidadBatalla>();
        CargarRenders();
    }


    private void CargarRenders()
    {
        renders = transform.GetComponentsInChildren<SpriteMeshInstance>();
        capaZ = transform.GetComponentsInChildren<CapaZ>();
    }
    void Update()
    {
        ActualizarCapa();
        AplicarCapas();
    }

    void ActualizarCapa()
    {
        capa = transform.position.z * -1 * 10;
    }


    void AplicarCapas()
    {
        for(int i = 0; i < renders.Length; i++)
        {
            renders[i].sortingOrder = (int)(capaZ[i].capaZ + capa);
            if(renders[i] == manoDeArma && unidad.arma != null)
            {
                unidad.arma.render.sortingOrder = renders[i].sortingOrder - 1;                
            }
        }
    }


}