using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}

    public Transform player;
    public CinemachineVirtualCamera followCamera;
    
    [Space]

    public Checkpoints[] checkpoints;

    [Header ("PLAYER STATS")]
    public int stonesCount;
    [Space]
    public int currentHearts;
    public int maxHearts;


    int tempStonesCount;

    private void Awake() {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
        PlayerPrefs.DeleteKey("Checkpoint");

        tempStonesCount = PlayerPrefs.GetInt("StonesCount");

        if (tempStonesCount == 0) {
            PlayerPrefs.SetInt("StonesCount", stonesCount);
        }else {
            stonesCount = PlayerPrefs.GetInt("StonesCount");
        }
        
    
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddHeart(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveHeart(1);
        }

        if (currentHearts <= 0) {
            if (PlayerPrefs.GetInt("Checkpoint") == 1) {
                GameManager.instance.player.position = GameManager.instance.checkpoints[0].startPos.position;
                AddHeart(maxHearts);
            }else if (PlayerPrefs.GetInt("Checkpoint") == 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }

    }

    public void RemoveHeart (int val)
    {
        if (currentHearts > 0) 
        {
            currentHearts -= val;
            UIManager.instance.UpdateHearts();
        }

        BlinkObject();
    }

    public void AddHeart (int val)
    {
        if (currentHearts < maxHearts) 
        {
            currentHearts += val;
            UIManager.instance.UpdateHearts();

        }
    }

    public void AddStones (int val) {
        stonesCount += val;
    }

    public void RemoveCameraTargets ()
    {
        followCamera.m_Follow = null;
        followCamera.m_LookAt = null;
    }

    public void CancelMovement () {
        player.gameObject.GetComponent<PlayerController>().enabled = false;
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteKey("StonesCount");
    }


    private void BlinkObject()
    {
        Invoke("EnableBlink", 0f);
        Invoke("DisableBlink", 0.06f);
    }

    private void EnableBlink ()
    {
        player.localScale = Vector3.zero;
    }
    private void DisableBlink ()
    {
        if (player.GetComponent<PlayerController>().facingDirection == 1) {
            player.localScale = new Vector3(1, 1, -1);
        }else {
            player.localScale = new Vector3(1, 1, 1);
        }
    }

}
