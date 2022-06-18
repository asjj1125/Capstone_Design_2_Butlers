using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerWeapon;
using FirstGearGames.SmoothCameraShaker;
using KMJ.UI;

namespace K.Monster
{
    public class M_Cheese : MonoBehaviour
    {
        NormalMonster data;
        Rigidbody rigid;
        Transform pinPos;

        [SerializeField]
        GameObject[] CurvePoint;
        int CurvePointer = 0;
        bool ArrivedAtPointer;
        GameObject player;
        Vector3 playerPos;
        //적시 상태
        bool attackState;
        bool delay;
        bool motiondelay;
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

        //특정 위치를 왔다갔다 함
        //플레이어 발견 시 플레이어를 향해 빠르게 돌진
        //플레이어 바로 앞에서 날아올라 내려찍기
        float springForce;
        GameObject player;

        void Awake()
        {
            //파라미터 부분은 파서 완성되는 대로 수정
            //data = new NormalMonster(1f, 400f, MonsterBase.type.normal);
            ArrivedAtPointer = false;
            attackState = false;
            data = new NormalMonster(0.5f, 20f, MonsterBase.type.normal);
            delay = false;
            playerPos = new Vector3();
            motiondelay = false;
            deadState = false;
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
                deadState = true;
                this.GetComponent<Animator>().SetTrigger("Dead");
                StartCoroutine(CallEffect());
                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                GameManager.Instance().monsterCount++;
                //this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(this.gameObject, 3f);
                return;
            }
            if(deadState == false)
            {
                //플레이어가 시야에 들어오면 쫒음
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (160/2));
                Vector3 dic = player.transform.position - this.transform.position;
                //Debug.Log("호출");
                rigid.AddForce(new Vector3(0f, springForce, 0f), ForceMode.Impulse);
                //플레이어가 시야에 들어오면 쫒음
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (180/2));
                Vector3 dic = player.transform.position - this.transform.position;

                if(dic.magnitude < 30)
                {
                    if(Vector3.Dot(dic.normalized, this.transform.forward) > dotValue)
                    {
                        attack();
                        Debug.Log(this.gameObject.name + "col");
                        springTime = 0f;
                        if(springRate >= 1.5f)
                            springRate += Random.Range(0f, 0.5f);
                        if(springRate > 2f)
                            springRate = 1.5f;
                        return;
                    }
                    else
                        wander();
                }
                else
                {
                    wander();
                }

                springTime = 0f;
                if(springRate >= 1.5f)
                    springRate += Random.Range(0f, 0.4f);
                if(springRate > 3f)
                    springRate = 2f;
            }

                if(dic.magnitude < 15f)
                {
                    if(Vector3.Dot(dic.normalized, this.transform.forward) > dotValue || attackState)
                    {
                        attackState = true;
                        this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.red;
                        attack(2f);
                        //Debug.Log(this.gameObject.name + "col");
                        return;
                    }
                    else
                        wander(1f);
                }
                else
                {
                    wander(1f);
                }
            }
            
        }

        //void FixedUpdate(){}
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
                this.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(2).gameObject.transform.localScale.y,
                this.transform.GetChild(2).gameObject.transform.localScale.z);
                //Attacked();
            }
        }

        void attack(float speedRate)
        void attack()
        {
            Vector3 mtop = player.transform.position - this.transform.position;
            Vector3 dis = pinPos.position - this.transform.position;
            //경계 영역
            if(dis.magnitude >= 30f)
            {
                rigid.AddForce(new Vector3(dis.x/3, 0f, dis.z/3), ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
            }
            if(dis.magnitude < 30f)
            {
                rigid.AddForce((mtop.normalized)*5f, ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                
            }
        }
        void wander()
        {
            Vector3 mtop = player.transform.position - this.transform.position;
            Vector3 dis = pinPos.position - this.transform.position;
            //경계 영역 밖
            if(dis.magnitude >= 20f)
            {
                this.transform.position += dis.normalized * (speedRate * 3.0f) * Time.deltaTime;
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
                this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.white;
                attackState = false;
            //Debug.Log("ch: "+this.transform.position);
            //Debug.Log("pin: "+pinPos.position);
            //Debug.Log(dis.magnitude);
            if(dis.magnitude >= 15f-4f)
            {
                rigid.AddForce(new Vector3(dis.x/2, 0f, dis.z/2), ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
            }
            //경계 영역 안
            if(dis.magnitude < 20f)
            {
                if(mtop.magnitude <= 5f)
                {
                    //점프
                    //플레이어의 머리 위 좌표
                    if(delay == false)
                    {
                        gameObject.GetComponent<Animator>().SetTrigger("Attack");
                        playerPos = player.transform.position;
                        this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                        delay = true;
                        StartCoroutine(JumpAttack(mtop));
                    }
                    if(motiondelay == false)
                    {
                        rigid.useGravity = false;
                        Vector3 toplayerhead = playerPos + new Vector3(0f, 10f, 0f);
                        this.transform.position = Vector3.MoveTowards(this.transform.position, toplayerhead, 0.8f);
                        //this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                    }
                }
                else
                {
                    //접근
                    if(delay == false)
                    {
                        gameObject.GetComponent<Animator>().SetFloat("walkSpeed", speedRate);
                        this.transform.position += mtop.normalized * (speedRate * 3.0f) * Time.deltaTime;
                        this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                    }
                }
                //StartCoroutine(AttackToPlayer(mtop));
                Vector3 v = new Vector3(rand(), 0f, rand());
                rigid.AddForce(v, ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(v.normalized);
                
            }
        }

        IEnumerator JumpAttack(Vector3 mtop)
        {
            yield return new WaitForSeconds(0.6f);
            rigid.useGravity = true;
            motiondelay = true;
            rigid.mass = 10f;
            yield return new WaitForSeconds(2f);
            delay = false;
            motiondelay = false;
            rigid.mass = 1f;
        }

        void wander(float speedRate)
        {
            Vector3 target = CurvePoint[CurvePointer].transform.position;
            if(ArrivedAtPointer == true)
            {
                ++CurvePointer;
                if(CurvePointer >= CurvePoint.Length)
                {
                    CurvePointer = 0;
                }
                target = CurvePoint[CurvePointer].transform.position;
                pinPos = CurvePoint[CurvePointer].transform;
                ArrivedAtPointer = false;
            }
            Vector3 dis = target - this.transform.position;
            this.transform.rotation = Quaternion.LookRotation(dis.normalized);
            gameObject.GetComponent<Animator>().SetFloat("walkSpeed", speedRate);
            this.transform.position += dis.normalized * (speedRate * 3.0f) * Time.deltaTime;
            //this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
            if(dis.magnitude < 1f)
            {
                ArrivedAtPointer = true;
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
            if (95 <= per && per < 100) //95~99
            {
                return Item[1];
            }
            if (0 <= per && per < 90) //0~89
            {
                return Item[2];
            }
            return Item[0];
        }
    }
}

