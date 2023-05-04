using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}

    public int level = 1;
    public Transform player;
    public CinemachineVirtualCamera followCamera;
    
    [Space]

    public Checkpoints[] checkpoints;

    [Header ("PLAYER STATS")]
    public int stonesCount;
    [Space]
    public int currentHearts;
    public int maxHearts;

    [Header ("CAMERA SHAKES")]
    public Vector3 hitShake;


    [HideInInspector] public int tempCuurrentHearts;

    private void Awake() {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
        PlayerPrefs.DeleteKey("Checkpoint");

        if (level == 1) {
            PlayerPrefs.DeleteKey("Hearts");
            PlayerPrefs.DeleteKey("StonesCount");
        }else {
            maxHearts = PlayerPrefs.GetInt("Hearts");
            currentHearts = PlayerPrefs.GetInt("Hearts");
            stonesCount = PlayerPrefs.GetInt("StonesCount");
        }

        if (stonesCount <= 0) {
            PlayerPrefs.SetInt("StonesCount", 14);
            stonesCount = PlayerPrefs.GetInt("StonesCount");
        }

        tempCuurrentHearts = currentHearts;

        UIManager.instance.CreateHeartsGFX();

    
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void RestoreLives ()
    {
        if (PlayerPrefs.GetInt("Checkpoint") == 1) {
            GameManager.instance.player.position = GameManager.instance.checkpoints[0].startPos.position;
            AddHeart(maxHearts);
        }else if (PlayerPrefs.GetInt("Checkpoint") == 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RemoveHeart (int val)
    {
        currentHearts -= val;
        if (currentHearts > 0) 
        {
            UIManager.instance.UpdateHearts();
            StartCoroutine(GameManager.instance.CameraShake(hitShake.x, hitShake.y, hitShake.z));
        }else {
            RestoreLives();
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
        player.GetComponent<Animator>().SetFloat("Movement", 0);
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteKey("StonesCount");
    }


    private void BlinkObject()
    {
        Invoke("EnableBlink", 0f);
        Invoke("DisableBlink", 0.1f);
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



    public void RewardForCompletingFirstLevel ()
    {
        UIManager.instance.UpdateHeartsGFX();
        AddHeart(maxHearts - currentHearts);
    }


    public IEnumerator CameraShake(float amplitude, float frequency, float time)
    {
        followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;

        yield return new WaitForSeconds(time);

        followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;

        yield return null; 
        
    }

}
