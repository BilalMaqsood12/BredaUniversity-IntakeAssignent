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

    Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         
        movementDirection = new Vector2(movementInput, 0);

        //Face player towards input direction
        if (movementDirection.x < 0 ) {
            transform.eulerAngles = new Vector3(0, -90f, 0);
        }
        if (movementDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 90f, 0);
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(transform.up * jumpForce * Time.deltaTime);
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

        rb.velocity = new Vector2(movementDirection * movementSpeed);


        //Ground Detection
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 2f);

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
