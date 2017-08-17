using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller3D))]
public class PlayerMovement : MonoBehaviour
{

    [Header("NormalJump")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .05f;
	public float moveSpeed = 20;
    float movExtraRoot = 1;
    //public float moveSpeedWalking = 13;


    [Header("AirJump")]
    public bool canAirJump = true;
    public float fAirJump = 4;
    public int maxAirJump = 1;
    public int actAirJump = 0;

    [Header("Wall Jump & Slide")]
    public bool canWallSlide = true;
	public Vector3 wallJumpClimb;
	public Vector3 wallJumpOff;
	public Vector3 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
    float velocityZSmoothing;

	public Controller3D controller;

	Vector3 directionalInput;
	bool wallSliding = false;
	int wallDirX;

    bool enKnockBack = false;

    float knockBackdirX;
    int intensidadKB;

    public bool usingRootMotion = false;

    public bool corriendo = false;

    Vector3 rootMotionVel;
    Vector3 fuerzaAplicada;

    Controller3D.TipoMovimiento tipo;

    public bool movePorAnimacion;
    public Transform tObjetivo;
    public float velMoveAnim = 10f;
    public Vector3 velAnim;
    Vector3 velCurrent;
    public bool moveTobjetivo = false;
    public bool movePorAnimacionRec = false;

    Animator anim;
    Collider col;
    void Start()
    {
        anim = GetComponent<Animator>();
		controller = GetComponent<Controller3D> ();
        col = GetComponent<Collider>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

    void Update()
    {
        if (usingRootMotion)
        {
            return;
        }

        CalculateVelocity();
        HandleWallSliding();

        Vector3 velFinal = new Vector3();

        
        velFinal = velocity  * Time.deltaTime;

        controller.Move(velFinal, directionalInput, tipo);
        //   if (anim != null) anim.SetFloat("VelY", velFinal.y);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;            
        }
    }

    public void SnapPosicion(Vector3 pos)
    {
        transform.position = pos;
    }


    public void SetDirectionalInput(Vector3 input, Controller3D.TipoMovimiento tipo)
    {
        directionalInput = input;
        this.tipo = tipo;
    }
    

    public void SaltoNormalInputDown()
    {      
        if(enKnockBack)
        {            
            return;
        }
		if (wallSliding)
        {         
            actAirJump = 0;
            WallSlidingJump();
        }
        else if (controller.collisions.below)                                                           //Tocando Piso
        {
            actAirJump = 0;
            velocity.y = maxJumpVelocity;
			
		}
        else if(canAirJump && actAirJump < maxAirJump)                                                  //Sin tocar piso
        {
            actAirJump++;
            velocity.y = maxJumpVelocity;
        }
	}
    
    void WallSlidingJump()
    {
        if(!canWallSlide)
        {
            return;
        }
        if (wallDirX == directionalInput.x)
        {
            velocity.x = -wallDirX * wallJumpClimb.x;
            velocity.y = wallJumpClimb.y;
        }
        else if (directionalInput.x == 0)
        {
            velocity.x = -wallDirX * wallJumpOff.x;
            velocity.y = wallJumpOff.y;
        }
        else
        {
            velocity.x = -wallDirX * wallLeap.x;
            velocity.y = wallLeap.y;
        }
    }

	public void SaltoNormalInputUp()
    {
		if (velocity.y > minJumpVelocity)
        {
			velocity.y = minJumpVelocity;
		}
	}		

	void HandleWallSliding()
    {
        if(!canWallSlide)
        {
            return;
        }
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

    public void SetRootMotionVelocity(Vector3 root)
    {
       /* if(transform.rotation == Quaternion.Euler(0,180,0))
        {
            rootMotionVel = new Vector3(root.x *-1, root.y, root.z *-1);
        }
        else
        {
            rootMotionVel = root;
        }*/

        rootMotionVel = root;

    }

    void CalculateVelocity()
    { 
        float targetVelocityX = directionalInput.x * moveSpeed + rootMotionVel.x + fuerzaAplicada.x;
        float targetVelocityZ = directionalInput.z * moveSpeed + rootMotionVel.z + fuerzaAplicada.z;

    
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.z = Mathf.SmoothDamp(velocity.z, targetVelocityZ, ref velocityZSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);



        velocity.y += gravity * Time.deltaTime + rootMotionVel.y + fuerzaAplicada.y;

        fuerzaAplicada = Vector3.zero;


    }       
    
    public void SetVelocidad(float vel)
    {
        moveSpeed = vel;
    }


    public void AplicarFuerza(Vector3 fuerza)
    {
        fuerzaAplicada  = fuerza;
      //  controller.AplicarFuerza(fuerza);
    }
}
