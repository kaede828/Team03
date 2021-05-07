using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSE : MonoBehaviour
{
    public AudioClip attackSE;
    public AudioClip missSE;
    AudioSource audioSource;
    bool isHit;//当たっている状態か
    int cooltime = 30;//seのクールタイム

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cooltime = 30;
    }

    // Update is called once per frame
    void Update()
    {
        //missSeが入ってなかったらなにもしない
        if (missSE != null)
        {
            if (!isHit && !audioSource.isPlaying)
            {
                cooltime = 0;
                audioSource.PlayOneShot(missSE);
            }
        }

        //当たっているかつこのSEが再生されていなかったら
        if (isHit&& !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(attackSE);
            isHit = false;
        }
        //当たらなかったら素振りの音
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            isHit = true;
        }
    }
}
