using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}

    public Transform player;


    [Header ("PLAYER STATS")]
    public int stonesCount;
    [Space]
    public int currentHearts;
    public int maxHearts;


    private void Awake() {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
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

    }

    public void RemoveHeart (int val)
    {
        if (currentHearts > 0) 
        {
            currentHearts -= val;
            UIManager.instance.UpdateHearts();
        }
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

}
