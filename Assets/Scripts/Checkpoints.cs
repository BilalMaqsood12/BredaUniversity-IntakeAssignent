using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform startPos;
    bool CheckpointChecked;

    // Start is called before the first frame update
    void Start()
    {
        CheckpointChecked = PlayerPrefs.GetInt("Checkpoint") == 1 ? true : false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (!CheckpointChecked) {
                GetComponent<Animator>().SetBool("Checkpoint", true);
                PlayerPrefs.SetInt("Checkpoint", 1);
                CheckpointChecked = true;
            }
        }
    }
}
