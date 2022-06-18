using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerWeapon;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.Audio;
using KMJ.UI;

namespace K.Monster
{
    public class M_Shooter : MonoBehaviour
    {
        NormalMonster data;

        [SerializeField]
        GameObject niddle;
        float coolTime;
        [SerializeField]
        float coolRate;

        GameObject player;
        bool attackState;
        bool deadState;
        Transform startTransform;
        public ShakeData MyShake;
        [SerializeField]
        GameObject deadEffect;
        [SerializeField]
        AudioClip hit;
        [SerializeField]
        AudioClip dead;
        [SerializeField]
        GameObject[] Item;

        public NormalMonster DATA
        {
            get{
                return data;
            }
            set{
                data = value;
            }
        }

        void Awake()
        {
            attackState = false;
            deadState = false;
            data = new NormalMonster(0.5f, 20f, MonsterBase.type.normal);
            startTransform = this.transform;
        }

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        IEnumerator CallEffect()
        {
            yield return new WaitForSeconds(2.5f);
            this.GetComponent<AudioSource>().clip = dead;
            this.GetComponent<AudioSource>().Play();
            Instantiate(deadEffect, this.transform);
            Instantiate(SpawnItem(), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        }

        void Update()
        {
            if(data.FullHp <= 0f && !deadState)
            {
                this.GetComponent<Animator>().SetTrigger("dead");
                StartCoroutine(CallEffect());
                this.GetComponent<BoxCollider>().enabled = false;
                GameManager.Instance().monsterCount++;
                //this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(this.gameObject, 3f);
                deadState = true;
                return;
            }

            if(coolTime < coolRate && !deadState)
            {
                coolTime += Time.deltaTime;
            }
            if(coolTime >= coolRate && !deadState)
            {
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (180/2));
                Vector3 dis = (player.transform.position + new Vector3(0f, 1.5f, 0f)) - this.transform.position;

                if(dis.magnitude < 15f)
                {
                    if(Vector3.Dot(dis.normalized, this.transform.forward) > dotValue)
                    {
                        //Debug.Log("안 if");
                        attackState = true;
                        this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.red;
                        attack(dis);
                        //Debug.Log(this.gameObject.name + "col");
                        coolTime = 0f;
                        return;
                    }
                    else
                    {
                        //Debug.Log("안 if else");
                        this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.white;
                        attackState = false;
                    }

                }
                else
                {
                    //Debug.Log("밖 if else");
                    this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.white;
                    attackState = false;
                }
                    coolTime = 0f;
            }
        }

        void attack(Vector3 dis)
        {
            StartCoroutine(AttackToPlayer(dis));
            this.transform.rotation = Quaternion.LookRotation(dis.normalized);
        }

        IEnumerator AttackToPlayer(Vector3 mtop)
        {
            yield return new WaitForSeconds(0.3f);
            Instantiate(niddle, this.transform.GetChild(5).transform);
        }

        void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.tag == "hitBox")
            {
                CameraShakerHandler.Shake(MyShake);
                this.GetComponent<AudioSource>().clip = hit;
                this.GetComponent<AudioSource>().Play();
                Debug.Log(collision.gameObject.transform.parent.GetComponent<weaponController>().plDemage.demage);
                data.FullHp -= collision.gameObject.transform.parent.GetComponent<weaponController>().plDemage.demage;
                if (data.FullHp < 0)
                {
                    data.FullHp = 0;
                }
                Debug.Log(collision.gameObject.transform.parent.GetComponent<weaponController>().plDemage.demage);
                this.transform.GetChild(4).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(4).gameObject.transform.localScale.y,
                this.transform.GetChild(4).gameObject.transform.localScale.z);
                //Attacked();
            }

            if (collision.gameObject.tag == "fire")
            {
                CameraShakerHandler.Shake(MyShake);
                this.GetComponent<AudioSource>().clip = hit;
                this.GetComponent<AudioSource>().Play();
                Debug.Log(player.GetComponent<weaponController>().plDemage.demage);
                data.FullHp -= player.GetComponent<weaponController>().plDemage.demage;
                if (data.FullHp < 0)
                {
                    data.FullHp = 0;
                }
                Debug.Log(player.GetComponent<weaponController>().plDemage.demage);
                this.transform.GetChild(4).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(4).gameObject.transform.localScale.y,
                this.transform.GetChild(4).gameObject.transform.localScale.z);
                //Attacked();
            }
        }

        GameObject SpawnItem()
        {
            //프리팹 넣는 순 하트->공업->이속
            //슈팅->공업 90%
            //똥->하트 90%
            //모아이-> 이속 90%
            int per = Random.Range(0, 100); //0~100
            if (90 <= per && per < 95) //90~94
            {
                return Item[0];
            }
            if(0 <= per && per < 90) //0~89
            {
                return Item[1];
            }
            if(95 <= per && per < 100) //95~99
            {
                return Item[2];
            }
            return Item[0];
        }
    }
}

