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
                //공격 모션 실행 중이 아닐 때 attack 실행
                //if(isAnimating == false)
                Attack();

                //피격 시 공격 모션 무시하고 피격 모션
                //if(검 등 충돌 시)
                //Damaged();
            }

        }

        //hp에 따라 공격패턴 변화, 70%, 40% 10%
        //미구현

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

        //공격 함수
        void Attack()
        {
            /*
            //fullhp는 인스턴스 hp
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

            //근접 공격 호출, 근거리 범위 안
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
                    //돌아보기
                    Look();
                }
                lookTimer += Time.deltaTime;
                //공격
                Atk_Wave(2.0f);
                return;
            }
            
            //원거리 공격 호출, 원거리 범위 안
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
                    //돌아보기
                    Look();
                }
                lookTimer += Time.deltaTime;
                //공격
                Atk_Dash(2f);
                return;
            }

            //달리기, 원거리 범위보다 먼 경우
            if(Vector3.Distance(this.transform.position, player.transform.position) > FarAtkDistance)
            {
                if(nowState == AttackState.none)
                {
                    //돌아보기
                    Look();
                    //이동(스피드 업)
                    Move(1.5f);
                    return;
                }
            }
        }

        //피격 함수
        void Damaged()
        {
            //피격 정보, 애니메이션
            //GameObject.Find("...").GetComponent<playerAttack>().damage
        }

        void Look()
        {
            //플레이어를 향해
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
            //speed 처리
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
        //근거리 저격 공격(충격파)
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
        //원거리 저격 공격(돌진)
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
            //파라미터 부분은 파서 완성되는 대로 수정
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
                //Debug.Log("호출");
                //플레이어가 시야에 들어오면 쫒음
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
            //경계 영역
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
            //배회할 중심 포지션 지정(빈오브젝트 등으로 핀 지정)
            //핀>몬스터로 계층구조로 관리
            //몬스터에서 핀까지의 벡터
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