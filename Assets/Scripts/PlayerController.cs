using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 6f;
    public float jumpForce = 4f;
    public float gravity = -9.81f;
    public float groundDrag = 4f;
    public float airDrag = 0.1f;
    public bool isGrounded;

    [Header ("PROJECTILES AND STUFF")]
    public GameObject projectilePrefb;
    public Transform projectileSpawnTransform;
    public float throwForce = 1000f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float movementInput = Input.GetAxis("Horizontal");  
        Vector3 movementDirection = new Vector3(movementInput, 0, 0);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(transform.up * jumpForce);
        }

        
        rb.AddForce(movementDirection * movementSpeed, ForceMode.Acceleration);


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
        //Ground Detection
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f);

        //Apply Extra Gravity
        if (!isGrounded) {
            rb.AddForce(transform.up * gravity);
        }

        
    }

    private void ShootProjectile ()
    {
        var newProjectile = Instantiate(projectilePrefb, projectileSpawnTransform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.right * throwForce);
    }

}
