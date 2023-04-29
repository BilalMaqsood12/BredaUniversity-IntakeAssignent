using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{
    public static FinalCutscene instance;

    public Transform mrBunny;
    public Transform mrsBunny;
    public Transform mrBunnyPos;
    public Transform mrsBunnyPos;
    public Transform Heart;

    public Animator BlackScreen;


    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartCutscene () 
    {
        BlackScreen.Play("BlackScreen");

        yield return new WaitForSecondsRealtime(0.5f);
       
        mrsBunny.position = mrsBunnyPos.position;
        mrBunny.position = mrBunnyPos.position;
        Heart.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        Application.LoadLevel("Start");
    }
}