using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; set;}

    [Header ("HUD")]
    public TextMeshProUGUI stonesText;
    
    [Space]

    public Sprite HeartImage;
    public RectTransform HeartsGrid;
    public Color HeartColor;
    public Color EmptyHeartColor;
    public List<Image> HeartsGFX;


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
        stonesText.text = "x" + GameManager.instance.stonesCount.ToString();
    }

    public void CreateHeartsGFX ()
    {
        for (int i = 0; i < GameManager.instance.maxHearts; i++)
        {
            GameObject newHeart = new GameObject("Heart " + i, typeof(RectTransform));
            newHeart.transform.parent = HeartsGrid;
            newHeart.AddComponent<Image>();
            newHeart.GetComponent<Image>().sprite = HeartImage;
            newHeart.GetComponent<Image>().color = HeartColor;
            newHeart.transform.localScale = new Vector3(1, 1, 1);
            HeartsGFX.Add(newHeart.GetComponent<Image>());
        }
    }

    public void UpdateHeartsGFX ()
    {
        for (int i = 0; i < GameManager.instance.maxHearts - GameManager.instance.tempCuurrentHearts; i++)
        {
            GameObject newHeart = new GameObject("Heart " + i, typeof(RectTransform));
            newHeart.transform.parent = HeartsGrid;
            newHeart.AddComponent<Image>();
            newHeart.GetComponent<Image>().sprite = HeartImage;
            newHeart.GetComponent<Image>().color = HeartColor;
            newHeart.transform.localScale = new Vector3(1, 1, 1);
            HeartsGFX.Add(newHeart.GetComponent<Image>());
        }
    }

    public void UpdateHearts ()
    {
        for (int i = 0; i < HeartsGFX.Count; i++)
        {
            HeartsGFX[i].color = EmptyHeartColor;
        }

        for (int i = 0; i < GameManager.instance.currentHearts; i++)
        {
            HeartsGFX[i].color = HeartColor;
        }
    }
}
