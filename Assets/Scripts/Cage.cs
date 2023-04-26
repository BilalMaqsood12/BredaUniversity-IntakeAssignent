using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public ParticleSystem sparkles;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenGate () 
    {
        Debug.Log("Open Gate");
        sparkles.Play();
        GetComponent<Animator>().Play("OpenGate");
    }

}
