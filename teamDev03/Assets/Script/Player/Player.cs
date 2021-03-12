﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //移動
    private Vector3 Move;//移動
    bool MovePossible;//移動可能か
    [SerializeField] float Speed;//移動量
    private float DefaultSpeed;//初期移動量
    float moveX = 0f;
    float moveZ = 0f;

    private Vector3 LastPos;//最後の場所

    //回転の速さ
    [SerializeField] GameObject Camera;
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
    public int HP=100;
    [SerializeField] Image image;

    //攻撃
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Attack4;

    //強化ポイント
    public int Point;
    private string PointT;
    [SerializeField] Text PointText; // Textオブジェクト

    //アニメーション
    private Animator animator;
    private const string key_isRun = "isRun";//フラグの名前
    private const string key_isJump = "isJump";
    private const string key_isAvert = "isAvert";
    private const string key_isAttack = "isAttack";
    private const string key_isDamage = "isDamage";
    AnimatorClipInfo[] clipInfo;
    string clipName;
    int animatorNum;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        this.animator = GetComponent<Animator>();

        DefaultSpeed = Speed;

        
    }

    void FixedUpdate()
    {
        PointT = Point.ToString();
        PointText.text = PointT;

        // アニメーションの情報取得
        clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        // 再生中のクリップ名
        clipName = clipInfo[0].clip.name;
        //アニメーションの状態
        if (clipName == "WAIT00") animatorNum = 0;
        if (clipName == "RUN00_F") animatorNum = 1;
        if (clipName == "JUMP00") animatorNum = 2;
        if (clipName == "SLIDE00") animatorNum = 3;
        if (clipName == "POSE30") animatorNum = 4;
        if (clipName == "POSE29") animatorNum = 5;
        if (clipName == "POSE26") animatorNum = 6;
        if (clipName == "WAIT04") animatorNum = 7;

        //攻撃中でなければ
        if(animatorNum<4)
        {
            //移動
            moveX = Input.GetAxis("Horizontal") * Speed;
            moveZ = Input.GetAxis("Vertical") * Speed;
            Move = Camera.transform.rotation * new Vector3(moveX, 0, moveZ);
            characterController.SimpleMove(Move);

            //進行方向の回転
            Vector3 angle = new Vector3(0, Input.GetAxis("HorizontalRight") * AngleSpeed, 0);
            Camera.transform.Rotate(angle);

            //どこからどこに進んだか
            Vector3 diff = transform.position - LastPos;
            diff = new Vector3(diff.x, 0, diff.z);
            //最後の場所を更新
            LastPos = transform.position;


            //移動しているか
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                this.animator.SetBool(key_isRun, false);
            }
            else
            {
                //ベクトルの大きさが0.01以上の時に向きを変える
                if (diff.magnitude > 0.01f)
                {
                    transform.rotation = Quaternion.LookRotation(diff);
                }
                this.animator.SetBool(key_isRun, true);
            }
        }

     
        //ジャンプ
        if (characterController.isGrounded)//地面についているか
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                //ジャンプ
                //Jump.y = JumpPower;
                this.animator.SetTrigger(key_isJump);

            }
        }


        //重力
        Jump.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(Jump * Time.deltaTime);

        //回避
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown("joystick button 2"))
            {
                //Avert = true;
                this.animator.SetTrigger(key_isAvert);
            }
        }

        ////回避したら
        //if (Avert == true)
        //{
        //    if (Timer <= AvertTime)
        //    {
        //        //スピードアップ
        //        Speed = AvertSpeed;
        //        Timer += Time.deltaTime;

        //        //無敵処理
        //    }
        //    else
        //    {
        //        //通常状態へ
        //        Speed = DefaultSpeed;
        //        Timer = 0;
        //        Avert = false;
        //    }

        //}      

        //攻撃
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown("joystick button 1"))
            {
                this.animator.SetTrigger(key_isAttack);
                
            }
        }

        //雑な攻撃の当たり判定
        switch (animatorNum)
        {
            case 0:
                Attack1.SetActive(false);
                Attack2.SetActive(false);
                Attack3.SetActive(false);
                Attack4.SetActive(false);
                this.tag = ("Player");
                break;
            case 1:
                Attack1.SetActive(false);
                Attack2.SetActive(false);
                Attack3.SetActive(false);
                Attack4.SetActive(false);
                this.tag = ("Player");
                break;
            case 3:
                this.tag = ("PlayerAvert");
                break;
            case 4:
                Attack1.SetActive(true);
                break;
            case 5:
                Attack2.SetActive(true);
                Attack1.SetActive(false);
                break;
            case 6:
                Attack3.SetActive(true);
                Attack2.SetActive(false);
                break;
            case 7:
                Attack4.SetActive(true);
                Attack3.SetActive(false);
                break;
        }       

    }
    //攻撃の当たり判定を消す
    private void ColliderReset()
    {

        Attack4.SetActive(false);

    }

    //ダメージ
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "EnemyAttack")
        {
            HP -= 10;
            image.fillAmount = HP / 100.0f;
            this.animator.SetTrigger(key_isDamage);
        }

    }


    
}
