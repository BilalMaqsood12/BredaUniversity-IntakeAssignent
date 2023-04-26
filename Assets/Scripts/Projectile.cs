using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
    
        if (other.CompareTag("Crate")) {
            other.gameObject.GetComponent<Crate>().OpenCrate();
        }

        if (other.CompareTag("Enemy")) {
            if (GameManager.instance.player.position.x < transform.position.x) {
                other.GetComponent<Enemy>().movingRight = false;
            }else if (GameManager.instance.player.position.x > transform.position.x) {
                other.GetComponent<Enemy>().movingRight = true;
            }
            other.GetComponent<Health>().TakeDamage(1);
        }

        if (other.CompareTag("Player")) {
            GameManager.instance.RemoveHeart(1);
        }

        if (other.gameObject.tag == "Cage Controller") {
            other.transform.parent.GetComponent<Cage>().OpenGate();
            Destroy(this.gameObject);
        
        }

        if (!other.CompareTag("Item")) {
            Destroy(this.gameObject);
        }
        
    }

}
