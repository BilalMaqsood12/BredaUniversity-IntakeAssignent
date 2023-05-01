using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("MOVEMENT")]
    public float movementSpeed = 6f;
    public float flyingStrength = -30f;
    public float gravity = -9.81f;
    public float groundDrag = 4f;
    public float airDrag = 0.1f;
    public bool isGrounded;
    public bool canFly;
    public LayerMask SqueezableObjects;
    public float SqueezeForce = 1200f;
    
    [Header ("JUMP")]
    public float jumpForce = 4f;
    public float jumpTime = 0.4f;
    public float jumpMultiplier = 3f;
    public float coyoteTime = 0.25f;
    float _coyotiTime;
    bool isJumping;
    float jumpCounter;
    Vector3 vecGravity;
    int numberOfJumps;

    [Header ("GROUND CHECK")]
    public Transform groundChecker;
    public float groundCheckDistance = 0.25f;


    [Header ("PROJECTILES AND STUFF")]
    public GameObject projectilePrefb;
    public Transform projectileSpawnTransform;
    public float throwForce = 1000f;


    //
    [HideInInspector] public int facingDirection;
    float _gravity;

    //Components
    Rigidbody rb;
    Animator animator;
    Vector3 movementDirection;


    //Animation related stuff
    int upperBodyLayerIndex;

    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector3(0, -Physics.gravity.y, 0);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _coyotiTime = coyoteTime;

        //Animation related stuff
        upperBodyLayerIndex = animator.GetLayerIndex("UpperBody");
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Detection
        RaycastHit hit;
        isGrounded = Physics.Raycast(groundChecker.position, -groundChecker.up, out hit, groundCheckDistance);
        animator.SetBool("OnGround", isGrounded);
         
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        animator.SetFloat("Movement", movementDirection.x);
        animator.SetBool("CanFly", canFly);

        //Face player towards input direction
        if (movementDirection.x < 0 ) {
            transform.localScale = new Vector3(1, 1, -1);
            facingDirection = 1;
        }
        if (movementDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingDirection = 0;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && _coyotiTime != 0 && numberOfJumps < 1) {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            isJumping = true;
            numberOfJumps += 1;
            jumpCounter = 0;
        }

        if (rb.velocity.y > 0 && isJumping) {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f) {
                currentJumpM = jumpCounter * (1 - t);
            }
            rb.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump")) {
            isJumping = false;
            jumpCounter = 0;

            if (rb.velocity.y > 0) {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.6f, rb.velocity.z);
            }
        }

        if (!isGrounded) {
            animator.SetFloat("Jump", rb.velocity.y);
            
        }


        if (isGrounded) {
            numberOfJumps = 0;
            _coyotiTime = coyoteTime;
        }

        //Shoot Projectile
        if (Input.GetKeyDown(KeyCode.F) && GameManager.instance.stonesCount > 0 && !canFly)
        {
            float upperBodyLayerWeight = 1f;
            animator.SetLayerWeight(upperBodyLayerIndex, upperBodyLayerWeight);
            animator.Play("Mr_Bunny_ThrowCarrots");
        }

        //Drag
        rb.drag = isGrounded ? groundDrag : airDrag;
        
        if (isGrounded) {
            _gravity = gravity;
        }else if (!isGrounded && rb.velocity.y < -15 && canFly) {
            _gravity = flyingStrength;
        }

    }

    private void FixedUpdate() 
    {
        //Movement
        rb.AddForce(movementDirection * movementSpeed * Time.deltaTime, ForceMode.Impulse);

        //Apply Extra Gravity
        if (!isGrounded) {
            rb.AddForce(transform.up * _gravity, ForceMode.Acceleration);
            _coyotiTime -= Time.deltaTime;
        }


        //Squeeze Over Objects
        RaycastHit SqueezableObjectsHit;
        if (Physics.Raycast(groundChecker.position, -groundChecker.up, out SqueezableObjectsHit, groundCheckDistance + 0.2f, SqueezableObjects))
        {
            AddForce(SqueezeForce, transform.up, ForceMode.Impulse);

            //If it's a crate, open it.
            if (SqueezableObjectsHit.collider.gameObject.GetComponent<Crate>() != null) {
                SqueezableObjectsHit.collider.gameObject.GetComponent<Crate>().OpenCrate();
            }
        }

        
    }

    private void ShootProjectile ()
    {
        var newProjectile = Instantiate(projectilePrefb, projectileSpawnTransform.position, Quaternion.identity);
        GameManager.instance.stonesCount -= 1;
        
        if (facingDirection == 0) {
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }else if (facingDirection == 1) {
            newProjectile.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
        }

        PlayerPrefs.SetInt("StonesCount", GameManager.instance.stonesCount);
        
    }

    private void EndThrowAnimation ()
    {
        animator.SetLayerWeight(upperBodyLayerIndex, 0f);
    }

    private void AddForce(float force, Vector3 direction, ForceMode forceMode)
    {
        rb.AddForce(direction * force * Time.deltaTime, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Obstacle") {
            GameManager.instance.RemoveHeart(1);
            other.collider.enabled = false;
        }
    }

}
