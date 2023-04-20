using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool canMove = true;
    float speed;
    public float chasingSpeed;
    public float partolSpeed;
    [Space]
    public Transform playerDetectorTransform;
    public float playerSpottableDistance = 12f;
    public float stoppingDistance = 4f;
    public LayerMask playerMask;
    public float coolDownTime = 2f;
    public float extraGravity = -80f;
    [Space]
    [ReadOnly]public bool movingRight = true;
    [ReadOnly]public bool hasSpottedPlayer;
    [Space]
    public Transform obstacleDetector;
    public float detectionDistance;
   
    [Space]

    [ReadOnly]public bool detectGround;
    [ReadOnly]public bool detectWall;
    

    [Header ("PROJECTILES AND STUFF")]
    public GameObject projectilePrefb;
    public Transform projectileSpawnTransform;
    public float throwForce = 35f;

    bool startShooting;
    bool facePlayerDirection;

    float _coolDownTime;
    float _directionCoolDownTime = 0.25f;

    Rigidbody rb;
    Animator animator;

    RaycastHit[] hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _coolDownTime = coolDownTime;

        ChangeDirection();

        hit = new RaycastHit[3];
    }

    // Update is called once per frame
    void Update()
    {
        //Set Direction Vector
        Vector3 dir = new Vector3();
        if (movingRight) {dir = transform.forward;}
        else if (!movingRight) {dir = -transform.forward;}


        if (!hasSpottedPlayer) {
            speed = partolSpeed;
        } else if (hasSpottedPlayer) {
            speed = chasingSpeed;
        }

        if (!startShooting && canMove) {
            if (movingRight) {rb.AddForce(Vector3.right * speed * Time.deltaTime);}
            else if (!movingRight) {rb.AddForce(Vector3.left * speed * Time.deltaTime);}
        }

        animator.SetFloat("Movement", rb.velocity.x);

//      -----------------------------------------------------------------------------------------------


        _directionCoolDownTime -= Time.deltaTime;

        //Ground Detection
        Vector3[] directions = new Vector3[]{-Vector3.up, Vector3.left, Vector3.right};
    
        detectGround = Physics.Raycast(obstacleDetector.position, directions[0], out hit[0], detectionDistance);
        if (hit[0].collider == null && _directionCoolDownTime <= 0f) 
        {
            ChangeDirection();
        }

        //Wall Detection in Left Direction
        detectWall = Physics.Raycast(obstacleDetector.position, directions[1], out hit[1], detectionDistance);
        if (hit[1].collider != null && _directionCoolDownTime <= 0f) 
        {
            ChangeDirection();
        }

        //Wall Detection in Right Direction
        detectWall = Physics.Raycast(obstacleDetector.position, directions[2], out hit[2], detectionDistance);
        if (hit[2].collider != null && _directionCoolDownTime <= 0f) 
        {
            ChangeDirection();
        }


//      -----------------------------------------------------------------------------------------------

        
        //Spot Player
        hasSpottedPlayer = Physics.Raycast(playerDetectorTransform.position, dir, playerSpottableDistance, playerMask);
        if (Vector3.Distance(transform.position, GameManager.instance.player.position) < stoppingDistance && hasSpottedPlayer) {
            startShooting = true;
            animator.SetTrigger("Throw Stones");
            facePlayerDirection = true;
        }else {
            startShooting = false;
        }
        
        if (facePlayerDirection) 
        {
            if (GameManager.instance.player.position.x > transform.position.x) {
                movingRight = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (GameManager.instance.player.position.x < transform.position.x) {
                movingRight = false;
                transform.localScale = new Vector3(1, 1, -1);
            }
        }
        
        if (!hasSpottedPlayer) 
        {
            _coolDownTime -= Time.deltaTime;
            if (_coolDownTime < 0) {
                facePlayerDirection = false;
                _coolDownTime = coolDownTime;
            }
        }
        
    }

    private void ShootProjectile ()
    {
        var newProjectile = Instantiate(projectilePrefb, projectileSpawnTransform.position, Quaternion.identity);
        
        if (movingRight) {
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }else if (!movingRight) {
            newProjectile.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
        }
        
    }

    private void FixedUpdate() {
        if (!detectGround) {
            rb.AddForce(transform.up * extraGravity, ForceMode.Acceleration);
        }
    }

    public void ChangeDirection ()
    {
        if (movingRight == true) {
            transform.localScale = new Vector3(1, 1, -1);
            movingRight = false;
        }else if (movingRight == false) {
            transform.localScale = new Vector3(1, 1, 1);
            movingRight = true;
        }

        _directionCoolDownTime = 0.25f;
    }
}
