using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ParticleSystem rewardGrantedParticles;
    public GameObject grantedUI;
    public Collider blocker;
    
    bool grandted;


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
        if (!grandted) {
            
            GetComponent<Animator>().Play("OpenGate");
            grantedUI.SetActive(true);
            grandted = true;
        }
    }

    public void Continue ()
    {

        grantedUI.SetActive(false);
        blocker.enabled = false;
        GameManager.instance.maxHearts = 5;
        GameManager.instance.RewardForCompletingFirstLevel();
        PlayerPrefs.SetInt("Hearts", GameManager.instance.maxHearts);
        rewardGrantedParticles.transform.position = GameManager.instance.player.position;
        rewardGrantedParticles.Play();
    }

}
