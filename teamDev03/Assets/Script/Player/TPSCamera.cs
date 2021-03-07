using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    public GameObject Player;//プレイヤー
    public Vector3 Default = new Vector3(0, 3f, -9f);//プレヤーとの距離

    void Update()
    {

        //カメラを移動
        transform.position = Player.transform.position;

        //プレイヤーの向いている方向にカメラを向ける
        transform.rotation = Player.transform.rotation;

        //位置を調整
        transform.Translate(Default);

        //プレイヤー方向にカメラを向ける
        transform.LookAt(Player.transform);
    }

}
