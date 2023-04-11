using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ITEM item;
    public int hearts = 1;
    public int stones = 5;
    [Space]
    public Collider triggerCollider;

    public enum ITEM
    {
        Invincibility,
        Hearts,
        Stones,
    }

    void OpenCrate ()
    {
        Invoke("DisableTriggerCollider", 0f);
        Invoke ("EnableTriggerCollider", 3f);

        if (item.Equals(ITEM.Invincibility)) {
            
        }
        if (item.Equals(ITEM.Hearts)) {
            GameManager.instance.AddHeart(hearts);
        }
        if (item.Equals(ITEM.Stones)) {
            GameManager.instance.AddStones(stones);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OpenCrate();
        }
    }

    private void DisableTriggerCollider () {triggerCollider.isTrigger = false;}
    private void EnableTriggerCollider () {triggerCollider.isTrigger = true;}
}
