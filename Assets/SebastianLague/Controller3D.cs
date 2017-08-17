using UnityEngine;
using System.Collections;

public class Controller3D : RaycastController {
    /// <summary>
    /// Ejemplo un MoveAmount de 10,0,0
    /// En Estricto movera 10 unidades a la Derecha al objeto en relacion a los ejes globales
    /// en Dependiente movera 10 Unidades en direccion derecha tomando en cuenta la rotacion del objeto.
    /// </summary>
    public enum TipoMovimiento { Estricto,Dependiente}
    //public float maxSlopeAngle = 80;

	public CollisionInfo collisions;
	[HideInInspector]
	public Vector2 playerInput;
    public Vector3 lastVelocity;
    

    public delegate void Golpe(float fuerza);
    public Golpe avisoGolpe;

    public delegate void Velocity(Vector3 vel);
    public Velocity velocityAplicada;



    public Vector3 fuerzaAplicadaObjetivo;
    public Vector3 fuerzaAplicada;
    private Vector3 fApliVel;
    private Vector3 fApliVel2;

    public new void Start() {
		base.Start ();
		collisions.faceDir = 1;
	}


   public void AplicarFuerza(Vector3 f)
    {
        fuerzaAplicadaObjetivo += f;
    }

    void CalcularFuerzaAplicada()
    {        
        fuerzaAplicada = Vector3.SmoothDamp(fuerzaAplicada, fuerzaAplicadaObjetivo, ref fApliVel, .1f);
        fuerzaAplicadaObjetivo = Vector3.SmoothDamp(fuerzaAplicadaObjetivo, Vector3.zero, ref fApliVel2, .3f) * Time.deltaTime;
    }


	public void Move(Vector3 moveAmount, bool standingOnPlatform, TipoMovimiento tipo)
    {       
        Move (moveAmount, Vector3.zero,tipo, standingOnPlatform);
	}

	public void Move(Vector3 moveAmount, Vector3 input, TipoMovimiento tipo, bool standingOnPlatform = false) {


     /*   if (tipo == TipoMovimiento.Dependiente && transform.rotation == Quaternion.Euler(0, 180, 0))
            collisions.faceDir = -1;
        else
            collisions.faceDir = 1;*/

        //Debug.Log(collisions.faceDir);

        UpdateRaycastOrigins ();
       // CalcularFuerzaAplicada();

        collisions.Reset ();
        collisions.moveAmountOld = moveAmount;

        playerInput = input;        

		XCollisions (ref moveAmount);
		if (moveAmount.y != 0) {
			YCollisions (ref moveAmount);
		}
        if(moveAmount.z != 0)
        {
            ZCollisions(ref moveAmount);
        }

		transform.Translate (moveAmount);
        if (velocityAplicada != null) velocityAplicada((moveAmount));



		if (standingOnPlatform) {
			collisions.below = true;
		}

        
        if (moveAmount.x > -.05 && moveAmount.x < .05)
            moveAmount.x = 0;

        if (collisions.moveAmountOld.x > -.05 && collisions.moveAmountOld.x < .05)
            collisions.moveAmountOld.x = 0;

       


    }
    
    void XCollisions(ref Vector3 moveAmount) {
        bool seguir = true;
        moveAmount.x *= collisions.faceDir;
      //  moveAmount.x += fuerzaAplicada.x;
        lastVelocity.x = moveAmount.x;
        
        float directionX = Mathf.Sign( moveAmount.x) ;
		float longitudRay = Mathf.Abs (moveAmount.x) + grosorPiel;

		if (Mathf.Abs(moveAmount.x) < grosorPiel) {
			longitudRay = 2*grosorPiel;
		}


        for (int i = 0; i < cantRayosY; i++)
        {
                if (!seguir) break;
            Vector3 origenRayo = (directionX == -1 * collisions.faceDir) ? raycastOrigins.bottomLeftBack : raycastOrigins.bottomRightForward;
            origenRayo += Vector3.up * (espaciadoRayosY * i);
            for (int j = 0; j < cantRayosZ; j++)
            {
                if (!seguir) break;

                Vector3 origenSegundoRayo = origenRayo + Vector3.forward * (espaciadoRayosZ * j * directionX);

                            
                RaycastHit hit;

                if (Physics.Raycast(origenSegundoRayo, Vector3.right * directionX, out hit, longitudRay, collisionMask))
                {
                    seguir = false;
                    if (hit.distance == 0)
                    {
                        continue;
                    }





                    Debug.DrawRay(origenSegundoRayo, Vector2.right * hit.distance, Color.yellow);


                    moveAmount.x = (hit.distance - grosorPiel) * directionX;
                    longitudRay = hit.distance;
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
                else if (hit.transform == null)
                {
                    if ((j == 0 && i == 0))
                        Debug.DrawRay(origenSegundoRayo, Vector2.right * directionX * longitudRay, Color.magenta);
                    else
                        Debug.DrawRay(origenSegundoRayo, Vector2.right * directionX * longitudRay, Color.red);
                }
            }
        }
    }

	void YCollisions(ref Vector3 moveAmount) {

        bool seguir = true;
        lastVelocity.y = moveAmount.y;

    //    moveAmount.y += fuerzaAplicada.y;

        float directionY = Mathf.Sign (moveAmount.y);
		float longitudRay = Mathf.Abs (moveAmount.y) + grosorPiel;

        for (int i = 0; i < cantRayosX; i++)
        {
            if (!seguir) break;
            Vector3 origenRayo = (directionY == -1) ? raycastOrigins.bottomLeftForward : raycastOrigins.topLeftForward;
            origenRayo += Vector3.right * (espaciadoRayosX * i);
            for (int j = 0; j < cantRayosZ; j++)
            {               
                if (!seguir) break;
                Vector3 origenSegundoRayo = origenRayo + Vector3.forward * (espaciadoRayosZ * j);

                RaycastHit hit;

                if (Physics.Raycast(origenSegundoRayo, Vector2.up * directionY, out hit, longitudRay, collisionMask))
                {
                    seguir = true;
                    if (hit.collider.tag == "Through")
                    {
                        if (directionY == 1 || hit.distance == 0)
                        {
                            continue;
                        }
                        if (collisions.fallingThroughPlatform)
                        {
                            continue;
                        }
                        if (playerInput.y == -1)
                        {
                            collisions.fallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatform", .5f);
                            continue;
                        }
                    }
                    Debug.DrawRay(origenSegundoRayo, Vector2.up * directionY * hit.distance, Color.yellow);
                                        
                    moveAmount.y = (hit.distance - grosorPiel) * directionY;
                    longitudRay = hit.distance;
                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                    
                }
                else
                {
                    if ((j == 0 && i == 0))
                        Debug.DrawRay(origenRayo, Vector2.up * directionY, Color.magenta);
                    else
                        Debug.DrawRay(origenRayo, Vector2.up * directionY, Color.green);

                }

            }
        }
    }

    void ZCollisions(ref Vector3 moveAmount)
    {
        bool seguir = true;
        moveAmount.z *= collisions.faceDir;

      //  moveAmount.z += fuerzaAplicada.z;

        lastVelocity.z = moveAmount.z ;


        float directionZ = Mathf.Sign(moveAmount.z) ;
        float longitudRay = Mathf.Abs(moveAmount.z) + grosorPiel;

        for (int i = 0; i < cantRayosX; i++)
        {
                if (!seguir) break;
            Vector3 origenRayo = (directionZ == -1) ? raycastOrigins.bottomLeftForward : raycastOrigins.bottomLeftBack;
            origenRayo += Vector3.right * (espaciadoRayosY * i);
            


            for (int j = 0; j < cantRayosY; j++)
            {

                if (!seguir) break;

                Vector3 origenSegundoRayo = origenRayo + Vector3.up * (espaciadoRayosX * j) + moveAmount.y * Vector3.up;

                RaycastHit hit;
                

                if (Physics.Raycast(origenSegundoRayo, Vector3.forward * directionZ, out hit, longitudRay, collisionMask))
                {
                    Debug.DrawRay(origenSegundoRayo, Vector3.forward * directionZ * hit.distance, Color.yellow);
                    moveAmount.z = (hit.distance - grosorPiel) * directionZ;
                    longitudRay = hit.distance;
                    collisions.below = directionZ == -1;
                    collisions.above = directionZ == 1;
                }
                else
                {
                    if ((j == 0 && i == 0))
                        Debug.DrawRay(origenSegundoRayo, Vector3.forward * directionZ * hit.distance, Color.magenta);
                    else
                        Debug.DrawRay(origenSegundoRayo, Vector3.forward * directionZ * hit.distance, Color.yellow);

                }
            }
        }
    }


   




    /*
	void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal) {
		float moveDistance = Mathf.Abs (moveAmount.x);
		float climbmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (moveAmount.y <= climbmoveAmountY) {
			moveAmount.y = climbmoveAmountY;
			moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
			collisions.slopeNormal = slopeNormal;
		}
	}
    
	void DescendSlope(ref Vector2 moveAmount) {

		RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast (raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs (moveAmount.y) + grosorPiel, collisionMask);
		RaycastHit2D maxSlopeHitRight = Physics2D.Raycast (raycastOrigins.bottomRight, Vector2.down, Mathf.Abs (moveAmount.y) + grosorPiel, collisionMask);
		if (maxSlopeHitLeft ^ maxSlopeHitRight) {
			SlideDownMaxSlope (maxSlopeHitLeft, ref moveAmount);
			SlideDownMaxSlope (maxSlopeHitRight, ref moveAmount);
		}

		if (!collisions.slidingDownMaxSlope) {
			float directionX = Mathf.Sign (moveAmount.x);
			Vector2 origenRayo = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			RaycastHit2D hit = Physics2D.Raycast (origenRayo, -Vector2.up, Mathf.Infinity, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
					if (Mathf.Sign (hit.normal.x) == directionX) {
						if (hit.distance - grosorPiel <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (moveAmount.x)) {
							float moveDistance = Mathf.Abs (moveAmount.x);
							float descendmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
							moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);
							moveAmount.y -= descendmoveAmountY;

							collisions.slopeAngle = slopeAngle;
							collisions.descendingSlope = true;
							collisions.below = true;
							collisions.slopeNormal = hit.normal;
						}
					}
				}
			}
		}
	}

	void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount) {

		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle > maxSlopeAngle) {
				moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs (moveAmount.y) - hit.distance) / Mathf.Tan (slopeAngle * Mathf.Deg2Rad);

				collisions.slopeAngle = slopeAngle;
				collisions.slidingDownMaxSlope = true;
				collisions.slopeNormal = hit.normal;
			}
		}

	}*/

    void ResetFallingThroughPlatform() {
		collisions.fallingThroughPlatform = false;
	}

	public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool forward, back;

        /*	public bool climbingSlope;
            public bool descendingSlope;
            public bool slidingDownMaxSlope;

            public float slopeAngle, slopeAngleOld;
            public Vector2 slopeNormal;*/
        public Vector2 moveAmountOld;
		public int faceDir;
		public bool fallingThroughPlatform;

       

		public void Reset() {
			above = below = false;
			left = right = false;
			forward = back = false;
            /*
			climbingSlope = false;
			descendingSlope = false;
			slidingDownMaxSlope = false;
			slopeNormal = Vector2.zero;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;*/
        }
	}

 
}
