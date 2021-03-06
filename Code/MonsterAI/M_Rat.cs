using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using playerWeapon;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

namespace K.Monster
{
    public class M_Rat : MonoBehaviour
    {
        BossMonster data;
        GameObject player;
        public BossMonster DATA
        {
            get{
                return data;
            }
            set{
                data = value;
            }
        }
        bool BigAttack;
        float CloseAtkDistance = 10f;
        float FarAtkDistance = 20f;
        bool AttackEnd;
        bool isAnimating;
        float dashAttackTimer;
        float lookTimer;
        Vector3 lookVector;

        float testtimer;

        enum AttackState
        {
            none,
            dash,
            wave,
            bigattack,
            attacked,
            dead
        }

        AttackState nowState;
        public ShakeData MyShake;
        [SerializeField]
        GameObject deadEffect;
        [SerializeField]
        GameObject fish;
        [SerializeField]
        AudioClip hit;
        [SerializeField]
        AudioClip dead;

        void Awake()
        {
            data = new BossMonster(1f, 200f, MonsterBase.type.boss);
            BigAttack = false;
            AttackEnd = true;
            isAnimating = false;
            nowState = 0;
            dashAttackTimer = 0.3f;
            lookVector = new Vector3();
            testtimer = 0f;
        }

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }
        IEnumerator CallEffect()
        {
            yield return new WaitForSeconds(4.5f);
            this.GetComponent<AudioSource>().clip = dead;
            this.GetComponent<AudioSource>().Play();
            Vector3 pos = this.transform.position + new Vector3(0, 2, 0);
            Instantiate(deadEffect, this.transform) ;
            Instantiate(fish, pos, Quaternion.identity);
        }

        void Update()
        {
            Vector3 dis = player.transform.position - this.transform.position;
            if(dis.magnitude <100f)
            {
                this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.red;
                GameObject.Find("Canvas").transform.GetChild(9).gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.GetChild(10).gameObject.SetActive(true);
                if (data.FullHp <= 0f)
                {   
                    this.transform.GetChild(4).GetComponent<Renderer>().material.color = Color.white;
                    Dead();
                }
                //?????? ?????? ?????? ?????? ?????? ??? attack ??????
                //if(isAnimating == false)
                Attack();

                //?????? ??? ?????? ?????? ???????????? ?????? ??????
                //if(??? ??? ?????? ???)
                //Damaged();
            }

        }

        //hp??? ?????? ???????????? ??????, 70%, 40% 10%
        //?????????

        void Attacked()
        {
            CameraShakerHandler.Shake(MyShake);
            this.GetComponent<AudioSource>().clip = hit;
            this.GetComponent<AudioSource>().Play();
            nowState = AttackState.attacked;
             gameObject.GetComponent<Animator>().SetTrigger("attacked");
             StartCoroutine("AttackedDelayTimer", 1f);
        }

        void Dead()
        {
            if(nowState == AttackState.none)
            {
                nowState = AttackState.dead;
                gameObject.GetComponent<Animator>().SetTrigger("dead");
                StartCoroutine(CallEffect());
                //this.GetComponent<Rigidbody>().isKinematic = true;
                StartCoroutine("DeadDelayTimer", 5f);
            }
        }

        void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.tag == "hitBox")
            {
                data.FullHp -= collision.gameObject.transform.parent.GetComponent<weaponController>().plDemage.demage;
                if (data.FullHp < 0)
                {
                    data.FullHp = 0;
                }
                //Debug.Log(collision.gameObject.GetComponent<weaponController>().plDemage.demage);
                GameObject.Find("Canvas").transform.GetChild(10).gameObject.GetComponent<Image>().fillAmount = data.FullHp / data.Hp;
                this.transform.GetChild(4).gameObject.transform.localScale = new Vector3(2 * (data.FullHp / data.Hp),
                this.transform.GetChild(4).gameObject.transform.localScale.y,
                this.transform.GetChild(4).gameObject.transform.localScale.z);
                Attacked();
            }

            if (collision.gameObject.tag == "fire")
            {
                Debug.Log(player.GetComponent<weaponController>().plDemage.demage);
                data.FullHp -= player.GetComponent<weaponController>().plDemage.demage;
                if (data.FullHp < 0)
                {
                    data.FullHp = 0;
                }
                Debug.Log(player.GetComponent<weaponController>().plDemage.demage);
                GameObject.Find("Canvas").transform.GetChild(10).gameObject.GetComponent<Image>().fillAmount = data.FullHp / data.Hp;
                this.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1 * (data.FullHp / data.Hp),
                this.transform.GetChild(2).gameObject.transform.localScale.y,
                this.transform.GetChild(2).gameObject.transform.localScale.z);
                //Attacked();
            }
        }

        //?????? ??????
        void Attack()
        {
            /*
            //fullhp??? ???????????? hp
            if(data.Hp*0.4f < data.FullHp && data.FullHp <= data.Hp*0.7f)
            {
                BigAttack = true;
            }

            if(data.Hp*0.1f < data.FullHp && data.FullHp <= data.Hp*0.4f)
            {
                BigAttack = true;
            }

            if(data.Hp*0.0f < data.FullHp && data.FullHp <= data.Hp*0.1f)
            {
                BigAttack = true;
            }

            if(BigAttack == true)
            {
                Atk_BigAttack(3f);
                return;
            }
            */

            //?????? ?????? ??????, ????????? ?????? ???
            if(Vector3.Distance(this.transform.position, player.transform.position) <= CloseAtkDistance)
            {
                if(nowState == AttackState.none)
                {
                    nowState = AttackState.wave;
                }
            }
            
            if(nowState == AttackState.wave)
            {
                if(lookTimer < 1f)
                {
                    //????????????
                    Look();
                }
                lookTimer += Time.deltaTime;
                //??????
                Atk_Wave(2.0f);
                return;
            }
            
            //????????? ?????? ??????, ????????? ?????? ???
            if(Vector3.Distance(this.transform.position, player.transform.position) <= FarAtkDistance &&
            Vector3.Distance(this.transform.position, player.transform.position) > CloseAtkDistance)
            {
                if(nowState == AttackState.none)
                {
                    nowState = AttackState.dash;
                }
            }

            if(nowState == AttackState.dash)
            {
                if(lookTimer < 1f)
                {
                    //????????????
                    Look();
                }
                lookTimer += Time.deltaTime;
                //??????
                Atk_Dash(2f);
                return;
            }

            //?????????, ????????? ???????????? ??? ??????
            if(Vector3.Distance(this.transform.position, player.transform.position) > FarAtkDistance)
            {
                if(nowState == AttackState.none)
                {
                    //????????????
                    Look();
                    //??????(????????? ???)
                    Move(1.5f);
                    return;
                }
            }
        }

        //?????? ??????
        void Damaged()
        {
            //?????? ??????, ???????????????
            //GameObject.Find("...").GetComponent<playerAttack>().damage
        }

        void Look()
        {
            //??????????????? ??????
            lookVector = player.transform.position - this.transform.position;
            this.gameObject.transform.rotation = Quaternion.LookRotation(lookVector);
           //this.gameObject.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 0.5f);
           //Debug.Log("Look for player");
        }
        void Move(float speedRate)
        {   
            gameObject.GetComponent<Animator>().SetFloat("walkSpeed", speedRate);
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            lookVector = player.transform.position - this.transform.position;
            //speed ??????
            this.transform.position += lookVector.normalized * (speedRate * 10f) * Time.deltaTime;
            //Debug.Log("Move to player");
        }
        void Atk_BigAttack(float delaytime)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            //gameObject.GetComponent<Animator>().SetTrigger("isAttacking");
            isAnimating = true;
            BigAttack = false;
            //Debug.Log("Big Attack!");
            StartCoroutine(DelayTimer(delaytime));
        }
        //????????? ?????? ??????(?????????)
        void Atk_Wave(float delaytime)
        {
            if(isAnimating == false)
            {
                isAnimating = true;
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
                gameObject.GetComponent<Animator>().SetTrigger("attack_wave");
                StartCoroutine(DelayTimer(delaytime));
                StartCoroutine(Hitbox(delaytime - 0.5f, 0.2f, 5));
            }
            //Debug.Log("Wave Attack!");
            
        }
        //????????? ?????? ??????(??????)
        void Atk_Dash(float delaytime)
        {
            if(isAnimating == false)
            {
                isAnimating = true;
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
                gameObject.GetComponent<Animator>().SetTrigger("attack_dash");
                StartCoroutine(DelayTimer(delaytime));
                StartCoroutine(Hitbox(delaytime - 1f, 0.6f, 6));
            }
            else
            {
                if(dashAttackTimer <= 0f)
                {
                    //Vector3 look = player.transform.position - this.transform.position;
                    this.transform.position += lookVector.normalized * (20f) * Time.deltaTime;
                }
                dashAttackTimer -= Time.deltaTime;
                //this.gameObject.GetComponent<Rigidbody>().AddForce(look.normalized*10f, ForceMode.VelocityChange);
                //isAnimating = true;
                //Debug.Log("Dash Attack!");
            }

        }
        
        IEnumerator DelayTimer(float time)
        {
            yield return new WaitForSeconds(time);
            nowState = AttackState.none;
            isAnimating = false;
            gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
            dashAttackTimer = 0.8f;
            lookTimer = 0f;
            //this.gameObject.GetComponent<Rigidbody>()
        }
        IEnumerator Hitbox(float starttime, float endtime, int childnum)
        {
            yield return new WaitForSeconds(starttime);
            //hitbox
            this.transform.GetChild(childnum).gameObject.GetComponent<CapsuleCollider>().enabled = true;
            yield return new WaitForSeconds(endtime);
            this.transform.GetChild(childnum).gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }

        IEnumerator AttackedDelayTimer(float time)
        {
            yield return new WaitForSeconds(time);
            nowState = AttackState.none;
            //gameObject.GetComponent<Animator>().SetBool("attacked", false);

        }
        IEnumerator DeadDelayTimer(float time)
        {
            yield return new WaitForSeconds(time);
            nowState = AttackState.none;
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace K.Monster
{
    public class M_Rat : MonoBehaviour
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


        void Awake()
        {
            //???????????? ????????? ?????? ???????????? ?????? ??????
            data = new NormalMonster(1f, 400f, MonsterBase.type.normal);
        }

        void Start()
        {
            rigid = this.GetComponent<Rigidbody>();
            pinPos = this.transform.parent.transform;
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if(wanderTime < wanderRate)
            {
                wanderTime += Time.deltaTime;
            }
            if(wanderTime >= wanderRate)
            {
                //Debug.Log("??????");
                //??????????????? ????????? ???????????? ??????
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (180/2));
                Vector3 dic = player.transform.position - this.transform.position;

                if(dic.magnitude < 20)
                {
                    if(Vector3.Dot(dic.normalized, this.transform.forward) > dotValue)
                    {
                        attack();
                        Debug.Log(this.gameObject.name + "col");
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

        void FixedUpdate()
        {

        }

        void attack()
        {
            Vector3 mtop = player.transform.position - this.transform.position;
            Vector3 dis = pinPos.position - this.transform.position;
            //?????? ??????
            if(dis.magnitude >= 30f)
            {
                rigid.AddForce(new Vector3(dis.x*3, 0f, dis.z*3), ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
            }
            if(dis.magnitude < 30f)
            {
                //rigid.AddForce((mtop.normalized)*40f, ForceMode.Impulse);
                StartCoroutine(AttackToPlayer(mtop));
                this.transform.rotation = Quaternion.LookRotation(mtop.normalized);
                
            }
        }
        IEnumerator AttackToPlayer(Vector3 mtop)
        {
            yield return new WaitForSeconds(0.2f);
            rigid.AddForce((mtop.normalized)*40f, ForceMode.Impulse);
        }
        void wander()
        {
            //????????? ?????? ????????? ??????(??????????????? ????????? ??? ??????)
            //???>???????????? ??????????????? ??????
            //??????????????? ???????????? ??????
            Vector3 dis = pinPos.position - this.transform.position;
            //Debug.Log("ch: "+this.transform.position);
            //Debug.Log("pin: "+pinPos.position);
            //Debug.Log(dis.magnitude);
            if(dis.magnitude >= 15f-4f)
            {
                rigid.AddForce(new Vector3(dis.x*3, 0f, dis.z*3), ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(dis.normalized);
            }
            if(dis.magnitude < 15f-4f)
            {
                Vector3 v = new Vector3(rand(5f), 0f, rand(5f));
                rigid.AddForce(v, ForceMode.Impulse);
                this.transform.rotation = Quaternion.LookRotation(v.normalized);
                
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
    }
}