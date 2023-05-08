using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform startPos;
    bool CheckpointChecked;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CheckpointChecked = PlayerPrefs.GetInt("Checkpoint") == 1 ? true : false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (!CheckpointChecked) {
                PlayerPrefs.SetInt("Checkpoint", 1);
                animator.Play("Checkpoint_Checked");
                CheckpointChecked = true;
            }
        }
    }
}
