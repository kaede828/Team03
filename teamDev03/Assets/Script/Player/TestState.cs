using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestState : MonoBehaviour
{
    public Image GapImage;
    public Image DreamImage;
    public Image RealImage;
    //世界の状態
    [SerializeField] GameObject REAL;
    [SerializeField] GameObject DREAM;
    [SerializeField] GameObject GAP;
    float gameTime;
    public float countTime = 5.0f;
    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        
        if (gameTime >= 0 && gameTime <= 10)
        {
            RealImage.fillAmount -= 1.0f / 10 * Time.deltaTime;
            REAL.SetActive(true);
            DREAM.SetActive(false);
            GAP.SetActive(false);
        }
        if (gameTime >= 10 && gameTime <= 20)
        {
            DreamImage.fillAmount -= 1.0f / 10 * Time.deltaTime;
            REAL.SetActive(false);
            DREAM.SetActive(true);
            GAP.SetActive(false);
        }
        if (gameTime >= 20 && gameTime <= 30)
        {
            
            GapImage.fillAmount -= 1.0f / 10 * Time.deltaTime;
            REAL.SetActive(false);
            DREAM.SetActive(false);
            GAP.SetActive(true);
        }
        if (gameTime > 30)
        {
            gameTime = 0;
            RealImage.fillAmount = 1;
            DreamImage.fillAmount = 1;
            GapImage.fillAmount = 1;
        }
    }
}
