using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHealth = 3;
    [Space]
    public float blinkTime = 0.1f;
    public Material blinkMaterial;
    public Material originalMaterial;
    [Space]
    public GameObject KillParticles;
    public UnityEvent OnDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int val) {
        if (currentHealth > 0) {
            currentHealth -= val;
        }
        
        BlinkObject();
    
        if (currentHealth <= 0) {
            spawnKillParticle();
            OnDie.Invoke();
            Destroy(this.gameObject);
        }
    }

    private void BlinkObject()
    {
        Invoke("EnableBlink", 0f);
        Invoke("DisableBlink", blinkTime);
    }

    private void EnableBlink ()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = blinkMaterial;
    }
    private void DisableBlink ()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = originalMaterial;
    }

    public void spawnKillParticle ()
    {
        Vector3 particlePos = new Vector3 (transform.position.x, transform.position.y + 1.15f, transform.position.z);
        var killparticle = Instantiate(KillParticles, particlePos, Quaternion.identity);
        Destroy(killparticle, 3f);
    }
}
