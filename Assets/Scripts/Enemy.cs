using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float speed;
    public float chasingSpeed;
    public float partolSpeed;
    [Space]
    public Transform playerDetectorTransform;
    public float playerSpottableDistance = 12f;
    public float stoppingDistance = 4f;
    public LayerMask playerMask;
    public float coolDownTime = 2f;
    [Space]
    [ReadOnly]public bool movingRight = true;
    [ReadOnly]public bool hasSpottedPlayer;
    [Space]
    public Transform obstacleDetector;
    public float detectionDistance;
    public bool detectGround;
    public bool detectWall;

    [Header ("PROJECTILES AND STUFF")]
    public GameObject projectilePrefb;
    public Transform projectileSpawnTransform;
    public float throwForce = 35f;

    bool startShooting;
    bool facePlayerDirection;

    float _coolDownTime;

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _coolDownTime = coolDownTime;
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

        if (!startShooting) {
            if (movingRight) {rb.AddForce(Vector3.right * speed * Time.deltaTime);}
            else if (!movingRight) {rb.AddForce(Vector3.left * speed * Time.deltaTime);}
        }

        animator.SetFloat("Movement", rb.velocity.x);

        //Ground Detection
        RaycastHit groundInfo;
        detectGround = Physics.Raycast(obstacleDetector.position, -obstacleDetector.up, out groundInfo, detectionDistance);
        if (groundInfo.collider == null) 
        {
            if (movingRight == true) {
                transform.localScale = new Vector3(1, 1, -1);
                movingRight = false;
            }else if (movingRight == false) {
                transform.localScale = new Vector3(1, 1, 1);
                movingRight = true;
            }
        }

        //Wall Detection
        RaycastHit wallInfo;
        detectWall = Physics.Raycast(obstacleDetector.position, dir, out wallInfo, detectionDistance);
        if (wallInfo.collider != null) 
        {
            if (movingRight == true) {
                transform.localScale = new Vector3(1, 1, -1);
                movingRight = false;
            }else if (movingRight == false) {
                transform.localScale = new Vector3(1, 1, 1);
                movingRight = true;
            }
        }


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
        else if (!hasSpottedPlayer) 
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
        GameManager.instance.stonesCount -= 1;
        
        if (movingRight) {
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }else if (!movingRight) {
            newProjectile.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
        }
        
    }
}
