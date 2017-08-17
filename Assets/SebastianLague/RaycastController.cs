using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    public const float grosorPiel = .015f;
    //const float distEntreRayos = .25f;

    public int cantRayosX;
    public int cantRayosY;
    public int cantRayosZ;

    public float espaciadoRayosX;
    public float espaciadoRayosY;
    public float espaciadoRayosZ;

    [HideInInspector]
	public BoxCollider col;
	public RaycastOrigins raycastOrigins;

	public virtual void Awake() {
		col = GetComponent<BoxCollider> ();
	}

	public virtual void Start() {
		CalculateRaySpacing ();
	}

	public void UpdateRaycastOrigins() {
		Bounds bounds = col.bounds;
		bounds.Expand (grosorPiel * -2);

        raycastOrigins.bottomLeftForward = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.bottomRightForward = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.topLeftForward = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.topRightForward = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

        raycastOrigins.bottomLeftBack = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.bottomRightBack = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.topLeftBack = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.topRightBack = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
    }
	
	public void CalculateRaySpacing() {
		Bounds bounds = col.bounds;
		bounds.Expand (grosorPiel * -2);

        /*
		float boundsLargo = bounds.size.y;
		float boundsAlto = bounds.size.x;
		float boundsAncho = bounds.size.z;

        cantRayosX = Mathf.RoundToInt (boundsAlto / distEntreRayos);
		cantRayosY = Mathf.RoundToInt (boundsLargo / distEntreRayos);
		cantRayosZ = Mathf.RoundToInt (boundsAncho / distEntreRayos);

        espaciadoRayosX = bounds.size.y / (cantRayosX - 1);
		espaciadoRayosY = bounds.size.x / (cantRayosY - 1);
		espaciadoRayosZ = bounds.size.z / (cantRayosZ - 1);*/

        float boundsLargo = bounds.size.x;
        float boundsAlto = bounds.size.y;
        float boundsAncho = bounds.size.z;    

        espaciadoRayosX = boundsLargo / (cantRayosX - 1);
        espaciadoRayosY = boundsAlto / (cantRayosY - 1);
        espaciadoRayosZ = boundsAncho / (cantRayosZ - 1);


        Debug.Log("espaciadoRayosX = boundsLargo / (cantRayosX - 1)");
        Debug.Log(espaciadoRayosX + "=" + boundsLargo + "/" + "(" + cantRayosX + "- 1)");

        Debug.Log("espaciadoRayosY = boundsAlto / (cantRayosY - 1)");
        Debug.Log(espaciadoRayosY + "=" + boundsAlto + "/" + "(" + cantRayosY + "- 1)");

        Debug.Log("espaciadoRayosZ = boundsAncho / (cantRayosZ - 1)");
        Debug.Log(espaciadoRayosZ + "=" + boundsAncho + "/" + "(" + cantRayosZ + "- 1)");

    }
	
	public struct RaycastOrigins {
		public Vector3 topLeftForward, topRightForward, topLeftBack, topRightBack;
		public Vector3 bottomLeftForward, bottomRightForward, bottomLeftBack, bottomRightBack;
    }

    private void OnValidate()
    {
        CalculateRaySpacing();
    }
}
