using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public CrateType crateType;
    [Space]
    public float force;
    public Vector3 forceDirection;
    [Space]
    public GameObject HeartItem;
    public GameObject[] StonesItem;

    public enum CrateType
    {
        Empty,
        Invincibility,
        Hearts,
        Stones,
        Enemy
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenCrate ()
    {
        if (crateType.Equals(CrateType.Empty)) {
            return;
        }

        if (crateType.Equals(CrateType.Invincibility)) {
            Debug.Log("Invincibility");
        }

        if (crateType.Equals(CrateType.Hearts)) {
            var heart = Instantiate(HeartItem, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody>().AddForce(forceDirection * force * Time.deltaTime, ForceMode.Impulse);
        }

        if (crateType.Equals(CrateType.Stones)) {
            int randStonesAmount = Random.Range(0, StonesItem.Length);
            var stones = Instantiate(StonesItem[randStonesAmount], transform.position, Quaternion.identity);
            stones.GetComponent<Rigidbody>().AddForce(forceDirection * force * Time.deltaTime, ForceMode.Impulse);
        }

        if (crateType.Equals(CrateType.Enemy)) {
            Debug.Log("Enemy");
        }
    }
}
