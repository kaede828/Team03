using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [SerializeField] Text KillNum;
    int resultKillEnemy = 0;
    float Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        resultKillEnemy = Player.getKillEnemy();
        Timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        KillNum.text = resultKillEnemy.ToString();

        if (Input.GetKeyDown("joystick button 1") && 3.0f<Timer)
        {
            SceneManager.LoadScene("Title");
        }

    }
}
