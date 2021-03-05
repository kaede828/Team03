using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //移動
    private Vector3 Move;//移動
    [SerializeField] float Speed;//移動量
    private float DefaultSpeed;//初期移動量
    float moveX = 0f;
    float moveZ = 0f;
    
    //ジャンプ
    private Vector3 Jump;//ジャンプ
    [SerializeField] float JumpPower;//ジャンプ力

    //回避
    private bool Avert;
    [SerializeField] float AvertTime;//回避時間
    [SerializeField] float AvertSpeed;
    float Timer;

    //体力
    static public int HP;


    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        DefaultSpeed = Speed;
    }
    void FixedUpdate()
    {
        //移動
        moveX = Input.GetAxis("Horizontal") * Speed;
        moveZ = Input.GetAxis("Vertical") * Speed;
        Move = new Vector3(moveX, 0, moveZ);
        characterController.SimpleMove(Move);

        //ジャンプ
        if (characterController.isGrounded)//地面についているか
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump.y = JumpPower;//ジャンプ
            }
        }
        Jump.y += Physics.gravity.y * Time.deltaTime;//重力
        characterController.Move(Jump * Time.deltaTime); //move方向に動かす

        //回避
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Avert = true;
            }
        }

        //回避したら
        if(Avert==true)
        {
            if (Timer<= AvertTime)
            {
                //スピードアップ
                Speed = AvertSpeed;
                Timer += Time.deltaTime;

                //無敵処理
            }
            else
            {
                //通常状態へ
                Speed = DefaultSpeed;
                Timer = 0;
                Avert = false;
            }
            
        }


       
    }
}
