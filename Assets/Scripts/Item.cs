using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ITEM item;
    public int hearts = 1;
    public int stones = 5;

    public enum ITEM
    {
        Invincibility,
        Hearts,
        Stones,
    }


    float setTriggerTime = .3f;

    private void Update() 
    {
        setTriggerTime -= Time.deltaTime;
        if (setTriggerTime < 0) {
            GetComponent<Collider>().isTrigger = true;
        }    
    }

    void PickupItem ()
    {
        if (item.Equals(ITEM.Invincibility)) {
            
        }
        if (item.Equals(ITEM.Hearts)) {
            if (GameManager.instance.currentHearts < GameManager.instance.maxHearts) {
                GameManager.instance.AddHeart(hearts);
                Destroy(this.gameObject);
            }else {
                return;
            }
        }
        if (item.Equals(ITEM.Stones)) {
            GameManager.instance.AddStones(stones);
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PickupItem();
        }
    }
}
