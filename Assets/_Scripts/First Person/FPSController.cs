using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FPSController: MonoBehaviour {
	public static bool disabled = false;
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;

    private bool limitDiagonalSpeed = true;
 
    public bool enableRunning = false;
 
    public float jumpSpeed = 4.0f;
    public float gravity = 10.0f;
 
    private float fallingDamageThreshold = 5; 

    public bool slideWhenOverSlopeLimit = false;
    public bool slideOnTaggedObjects = false; 
    public float slideSpeed = 5.0f;

    public bool airControl = true; 

    public float antiBumpFactor = .75f;
    public int antiBunnyHopFactor = 1;
 
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;
    private CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
	public bool isRunning = false;
	public bool isCrouching;
	public float crouchHeight = 1.5f;
	float centerHeight;

    void Start() {
		disabled = false;
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;
        jumpTimer = antiBunnyHopFactor;
		centerHeight = crouchHeight / -6;
    }

	public bool CanGetUp() {
		if (Physics.Raycast(transform.position, Vector3.up, 2, LayerMaskHelper.EverythingBut(31)))//, QueryTriggerInteraction.Ignore))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    void Update() {
		if (Time.timeScale == 0 || disabled)
			return;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;
 
        if (grounded) 
		{
			if (Input.GetButtonDown("Crouch"))
			{
				isCrouching = !isCrouching;
			}

			if (isCrouching)
			{
				speed = walkSpeed / 2f;
				controller.height = Mathf.Lerp(controller.height, crouchHeight, 5 * Time.deltaTime);
				controller.center = new Vector3(0, Mathf.Lerp(controller.center.y, centerHeight, 5 * Time.deltaTime), 0);
			}
			else
			{
				if (CanGetUp())
				{
					controller.height = 2.5f;
					controller.center = new Vector3(0, 0.25f, 0);
				}
			}

			bool sliding = false;

            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
			{
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit) sliding = true;
            }

            else
			{
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit) sliding = true;
            }
 
            if (falling) 
			{
                falling = false;
                if (myTransform.position.y < fallStartLevel - fallingDamageThreshold) FallingDamageAlert (fallStartLevel - myTransform.position.y);
            }
 
            if (enableRunning && !isCrouching)
            {
            	speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
				if (Input.GetButton("Zoom")) speed = walkSpeed;
            }

			if (speed == runSpeed)
				isRunning = true;
			else
				isRunning = false;
				
            if ((sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide")) 
			{
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            else
			{
                moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }
 
            if (!Input.GetButton("Jump"))
			{
                jumpTimer++;
            }
			else if (jumpTimer >= antiBunnyHopFactor && !isCrouching)
			{				
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
            }
        }
        else 
		{
            if (!falling)
			{
                falling = true;
                fallStartLevel = myTransform.position.y;
            }
 
            if (airControl && playerControl)
			{
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputY * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }
 
        moveDirection.y -= gravity * Time.deltaTime;

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }
 
    void OnControllerColliderHit(ControllerColliderHit hit) {
        contactPoint = hit.point;
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body != null && !body.isKinematic) body.velocity += controller.velocity * 0.1f;
    }

    void FallingDamageAlert(float fallDistance) {
        print ("Fell " + fallDistance + " units");   
    }
}