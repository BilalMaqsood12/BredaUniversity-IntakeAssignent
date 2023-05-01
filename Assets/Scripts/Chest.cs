using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Collider blocker;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest () 
    {
        blocker.enabled = false;
        GetComponent<Animator>().Play("OpenGate");
        GameManager.instance.RewardForCompletingFirstLevel();
    }

}
