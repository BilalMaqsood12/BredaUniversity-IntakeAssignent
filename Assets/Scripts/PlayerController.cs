using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("MOVEMENT")]
    public float movementSpeed = 6f;
    public float gravity = -9.81f;
    public float groundDrag = 4f;
    public float airDrag = 0.1f;
    public bool isGrounded;
    
    [Header ("JUMP")]
    public float jumpForce = 4f;
    public float jumpTime = 0.4f;
    public float jumpMultiplier = 3f;
    bool isJumping;
    float jumpCounter;
    Vector3 vecGravity;
    int numberOfJumps;


    [Header ("PROJECTILES AND STUFF")]
    public GameObject projectilePrefb;
    public Transform projectileSpawnTransform;
    public float throwForce = 1000f;

    Rigidbody rb;

    Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector3(0, -Physics.gravity.y, 0);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         
        movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        //Face player towards input direction
        if (movementDirection.x < 0 ) {
            transform.eulerAngles = new Vector3(0, -90f, 0);
        }
        if (movementDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 90f, 0);
        }

        //Jump
        if (Input.GetButtonDown("Jump") && numberOfJumps < 1) {
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


        if (isGrounded) {
            numberOfJumps = 0;
        }

        //Shoot Projectile
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootProjectile();
        }

        //Drag
        rb.drag = isGrounded ? groundDrag : airDrag;

    }

    private void FixedUpdate() 
    {
        //Movement
        rb.AddForce(movementDirection * movementSpeed * Time.deltaTime, ForceMode.Impulse);


        //Ground Detection
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 2f);

        //Apply Extra Gravity
        if (!isGrounded) {
            rb.AddForce(transform.up * gravity, ForceMode.Acceleration);
        }

        
    }

    private void ShootProjectile ()
    {
        var newProjectile = Instantiate(projectilePrefb, projectileSpawnTransform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
    }

}
