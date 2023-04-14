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
    
    GameObject newObject;

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
            SpawnObject(HeartItem, false, null);
        }

        if (crateType.Equals(CrateType.Stones)) {
            SpawnObject(null, true, StonesItem);
        }

        if (crateType.Equals(CrateType.Enemy)) {
            Debug.Log("Enemy");
        }

        Destroy(this.gameObject);
    }

    private void SpawnObject (GameObject GO, bool fromArray, GameObject[] GOArray) {
        if (!fromArray) 
        {
            newObject = Instantiate(GO, transform.position, Quaternion.identity);
            newObject.GetComponent<Rigidbody>().AddForce(forceDirection * force * Time.deltaTime, ForceMode.Impulse);
        } 
        else if (fromArray) 
        {
            int ObjectToSpawnNumber = Random.Range(0, GOArray.Length);
            newObject = Instantiate(GOArray[ObjectToSpawnNumber], transform.position, Quaternion.identity);
            newObject.GetComponent<Rigidbody>().AddForce(forceDirection * force * Time.deltaTime, ForceMode.Impulse);
           
        }        
    }
}
