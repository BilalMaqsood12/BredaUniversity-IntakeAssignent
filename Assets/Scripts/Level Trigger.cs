using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public bool loadNextScene = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (loadNextScene) {
                GameManager.instance.RemoveCameraTargets();
                PlayerPrefs.DeleteKey("Checkpoint");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else 
            {
                GameManager.instance.currentHearts = 0;
                GameManager.instance.RemoveHeart(0);
            }
        }

        if (other.CompareTag("Enemy")) {
            other.GetComponent<Health>().TakeDamage(99);
        }
    }
}
