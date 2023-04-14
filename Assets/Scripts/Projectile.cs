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
        if (!other.CompareTag("Player")) {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Crate")) {
            other.gameObject.GetComponent<Crate>().OpenCrate();
        }
    }
}
