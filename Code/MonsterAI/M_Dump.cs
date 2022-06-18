using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerWeapon;
using FirstGearGames.SmoothCameraShaker;
using KMJ.UI;

namespace K.Monster
{
    public class M_Dump : MonoBehaviour
    {
        NormalMonster data;
        Rigidbody rigid;
        Transform pinPos;
        float wanderTime;
        [SerializeField]
        float wanderRate;
        [SerializeField]
        float wanderForce;
        GameObject player;
        //적시 상태
        bool attackState;
        bool deadState;
        public NormalMonster DATA
        {
            get{
                return data;
            }
            set{
                data = value;
            }
        }
        public ShakeData MyShake;
        [SerializeField]
        GameObject deadEffect;
        [SerializeField]
        AudioClip hit;
        [SerializeField]
        AudioClip dead;
        [SerializeField]
        GameObject[] Item;

        void Awake()
        {
            //파라미터 부분은 파서 완성되는 대로 수정
            //data = new NormalMonster(1f, 400f, MonsterBase.type.normal);
            attackState = false;
            deadState = false;
            data = new NormalMonster(0.5f, 20f, MonsterBase.type.normal);
        }

        void Start()
        {
            rigid = this.GetComponent<Rigidbody>();
            pinPos = this.transform.parent.transform;
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
                //this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                GameManager.Instance().monsterCount++;
                Destroy(this.gameObject, 3f);
                deadState = true;
                return;
            }
            
            if(wanderTime < wanderRate && !deadState)
            {
                wanderTime += Time.deltaTime;
            }
            if(wanderTime >= wanderRate && !deadState)
            {
                //Debug.Log("똥:호출");
                rigid.AddForce(new Vector3(0f, 5f, 0f), ForceMode.VelocityChange);
                this.GetComponent<Animator>().SetTrigger("jumpping");

                //플레이어가 시야에 들어오면 쫒음

                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (120/2));
                Vector3 dic = player.transform.position - this.transform.position;

                if(dic.magnitude < 10f)
                {
                    if(Vector3.Dot(dic.normalized, this.transform.forward) > dotValue || attackState)
                    {
                        attackState = true;
                        this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.red;
                        attack();
                        //Debug.Log(this.gameObject.name + "col");
                        wanderTime = 0f;
                        if(wanderRate >= 1.5f)
                            wanderRate += Random.Range(0f, 0.5f);
                        if(wanderRate > 2f)
                            wanderRate = 1.5f;
                        return;
                    }
                    else
                        wander();
                }
                else
                {
                    wander();
                }

                wanderTime = 0f;
                if(wanderRate >= 1.5f)
                    wanderRate += Random.Range(0f, 0.4f);
                if(wanderRate > 3f)
                    wanderRate = 2f;
            }
        }

        void attack()
        {
            Vector3 mtop = player.transform.position - this.transform.position;
            Vector3 dis = pinPos.position - this.transform.position;
            //경계 영역
            if(dis.magnitude >= 15f-1f)
            {
                rigid.AddForce(new Vector3(dis.normalized.x*5, 0f, dis.normalized.z*5), ForceMode.VelocityChange);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
                this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
                attackState = false;
            }
            if(dis.magnitude < 15f-1f)
            {
                //rigid.AddForce((mtop.normalized)*40f, ForceMode.Impulse);
                StartCoroutine(AttackToPlayer(mtop));
                this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                
            }
        }
        IEnumerator AttackToPlayer(Vector3 mtop)
        {
            yield return new WaitForSeconds(0.2f);
            rigid.AddForce((mtop.normalized)*8f, ForceMode.VelocityChange);
        }
        void wander()
        {
            //배회할 중심 포지션 지정(빈오브젝트 등으로 핀 지정)
            //핀>몬스터로 계층구조로 관리
            //몬스터에서 핀까지의 벡터
            Vector3 dis = pinPos.position - this.transform.position;
            //Debug.Log("ch: "+this.transform.position);
            //Debug.Log("pin: "+pinPos.position);
            //Debug.Log(dis.magnitude);
            if(dis.magnitude >= 10f)
            {
                rigid.AddForce(new Vector3(dis.normalized.x*5, 0f, dis.normalized.z*5), ForceMode.VelocityChange);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
                this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
            }
            if(dis.magnitude < 10f)
            {
                Vector3 v = new Vector3(rand(1f), 0f, rand(1f));
                rigid.AddForce(v, ForceMode.VelocityChange);
                this.transform.rotation = Quaternion.LookRotation(v.normalized);
                this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;

            }
        }

        float rand(float weight)
        {
            int MorP = Random.Range(0, 2);
            if(MorP  == 0)
                return Random.Range(-5f *weight, -3f*weight);
            if(MorP == 1)
                return Random.Range(3f*weight, 5f*weight);

            Debug.Log("M_Cheese/rand(): MorP is not 0 or 1");
            return 0f;
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
                if(data.FullHp < 0)
                {
                    data.FullHp = 0;
                }
                Debug.Log(collision.gameObject.transform.parent.GetComponent<weaponController>().plDemage.demage);
                this.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(2).gameObject.transform.localScale.y,
                this.transform.GetChild(2).gameObject.transform.localScale.z);
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
                this.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(2).gameObject.transform.localScale.y,
                this.transform.GetChild(2).gameObject.transform.localScale.z);
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
            if (0 <= per && per < 90) //0~89
            {
                return Item[0];
            }
            if (90 <= per && per < 95) //90~94
            {
                return Item[1];
            }
            if (95 <= per && per < 100) //95~99
            {
                return Item[2];
            }
            return Item[0];
        }
    }
}

