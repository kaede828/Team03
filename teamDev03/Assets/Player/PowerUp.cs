using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] GameObject PowerUpMenu;
    [SerializeField] GameObject Cursor1;
    [SerializeField] GameObject Cursor2;
    [SerializeField] GameObject Cursor3;
    [SerializeField] GameObject Cursor4;
    public float SelectNum=0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PowerUpMenu.activeSelf == true)
        {
            //D-Pad　なぜか逆
            float dph = Input.GetAxis("D_PAD_H");//左+1 右-1
            float dpv = Input.GetAxis("D_PAD_V");//上-1 下+1
            if (dpv == -1)
            {
                SelectNum = 1;
            }
            if (dph == -1)
            {
                SelectNum = 2;
            }
            if (dph == 1)
            {
                SelectNum = 3;
            }
            if (dpv == 1)
            {
                SelectNum = 4;
            }
            
            
            
            
            switch (SelectNum)
            {
                case 0:
                    Cursor1.SetActive(false);
                    Cursor2.SetActive(false);
                    Cursor3.SetActive(false);
                    Cursor4.SetActive(false);
                    break;
                case 1:
                    //プレイヤー側でSelectNumが1の時Bを押すと選択
                    Cursor1.SetActive(true);
                    SelectNum = 0;
                    break;

                case 2:
                    Cursor2.SetActive(true);
                    SelectNum = 0;
                    break;

                case 3:
                    Cursor3.SetActive(true);
                    SelectNum = 0;
                    break;

                case 4:
                    Cursor4.SetActive(true);
                    SelectNum = 0;
                    break;
            }
            Debug.Log(SelectNum);
        }
    }
}
