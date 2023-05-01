using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crate : MonoBehaviour
{
    public CrateType crateType;
    [Space]
    public float force;
    public Vector3 forceDirection;
    [Space]
    public GameObject HeartItem;
    public GameObject[] StonesItem;
    
    [Header ("ENEMY")]
    public GameObject enemy;
    public UnityEvent OnDie;

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
        if (crateType.Equals(CrateType.Invincibility)) {
            Debug.Log("Invincibility");
        }

        if (crateType.Equals(CrateType.Hearts)) {
            SpawnObject(HeartItem, false, null, Quaternion.identity);
        }

        if (crateType.Equals(CrateType.Stones)) {
            SpawnObject(null, true, StonesItem, Quaternion.identity);
        }

        if (crateType.Equals(CrateType.Enemy)) {
            var newEnemy = Instantiate(enemy, transform.position, Quaternion.Euler(0, 90, 0));
            if (GameManager.instance.player.position.x < transform.position.x) {
                newEnemy.GetComponent<Enemy>().movingRight = true;
            }else if (GameManager.instance.player.position.x > transform.position.x) {
                newEnemy.GetComponent<Enemy>().movingRight = false;
            }
            newEnemy.GetComponent<Health>().OnDie = OnDie;
        }

        Destroy(this.gameObject);
    }

    private void SpawnObject (GameObject GO, bool fromArray, GameObject[] GOArray, Quaternion rotation) {
        if (!fromArray) 
        {
            newObject = Instantiate(GO, transform.position, rotation);
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
