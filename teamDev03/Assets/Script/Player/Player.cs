﻿using System.Collections;
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

    private Vector3 LastPos;//最後の場所

    //回転の速さ
    [SerializeField] GameObject Head;
    [SerializeField] float AngleSpeed = 6.0f;

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
        Move = Head.transform.rotation * new Vector3(moveX, 0, moveZ);
        characterController.SimpleMove(Move);

        //進行方向の回転
        Vector3 angle = new Vector3(0, Input.GetAxis("Mouse X") * AngleSpeed, 0);
        Head.transform.Rotate(angle);

        //どこからどこに進んだか
        Vector3 diff = transform.position - LastPos;   
        //最後の場所を更新
        LastPos = transform.position;  

        //ベクトルの大きさが0.01以上の時に向きを変える
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff);
        }



        //ジャンプ
        if (characterController.isGrounded)//地面についているか
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //ジャンプ
                Jump.y = JumpPower;
            }
        }
        //重力
        Jump.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(Jump * Time.deltaTime);

        //回避
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown(KeyCode.Q))
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
