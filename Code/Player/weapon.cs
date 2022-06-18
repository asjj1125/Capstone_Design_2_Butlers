using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using K.Monster;

namespace playerWeapon
{
    public class weapon : MonoBehaviour
    {

        [SerializeField]
        public enum Type { Short, Long, fire };
        public Type type;

        //////////////////////////////////////////////
        //추가 변수
        public GameObject colObj;
        public List<GameObject> enemys;
        //////////////////////////////////////////////
        public GameObject enemy;
        public float shortDis;
        /////////////////////////////////////////////
        public Transform firepos;
        public GameObject fire;
        //////////////////////////////////////////////
        public float timeDelay;
        private bool isDelay;
        //////////////////////////////////////////////
        GameObject p;
        GameObject hit1;
        GameObject hit2;
        //////////////////////////////////////////////
        public GameObject trailEffect;

        void Start()
        {

            enemys = new List<GameObject>();
            //맵에 있는 몬스터 리스트
            enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();
            for (int i = 0; i < enemys.Count; i++)
                Debug.Log("field enemy[" + i + "]: " + enemys[i].name);

            ////////////////////////////////////////////////////////////////////////////////////////
            p = GameObject.FindGameObjectWithTag("Player");
            hit1 = p.transform.Find("HitBox1").gameObject;
            hit2 = p.transform.Find("HitBox2").gameObject;
        }


        public void Use()
        {

            //StopCoroutine("swing");
            //StartCoroutine("swing");    

            /////////////////////////////////////////////////////////////////////////////////////
            //코루틴을 왜 쓰는 건진 모르겠는데 플레이어 무브에서 이 함수가
            //현재 잡고 있는 무기를 사용하고자 할 때 호출되는 함수이면
            //if(type == Type.short) {해당 타입의 use 함수(use_short()같은거) 만들어서 거기에 정리, return; }
            //if(type == Type.Long) {마찬가지로 use_Long() 만들어서 거기에 정리}
            //등등 해서 일단 함수 분리
            //한 다음 그 함수에 부채꼴이나 그런거 구현해야 할 듯
            //우선 여기 enum형 type 말인데 playermove에서 객체만 만들어놓고 초기화를 안했던데 이 코드가 돌아가려면
            //payrmove에서 각 무기가 무슨 타입인지, 공격력은 몇인지 정보를 넣어줘야 함

            //Debug.Log("호출은됨");

            if (p.GetComponent<playerWeapon.weaponController>().handObject == null) { return; }
            else if (p.GetComponent<playerWeapon.weaponController>().handObject.type == weapon.Type.Short)
            {
                StartCoroutine(Use_Short());
                return;
            }

            else if (p.GetComponent<playerWeapon.weaponController>().handObject.type == weapon.Type.Long)
            {
                
                StartCoroutine(Use_Long());
                return;
            }

            else if (p.GetComponent<playerWeapon.weaponController>().handObject.type == weapon.Type.fire)
            {
                StartCoroutine(Use_fire());
                return;
            }


            /////////////////////////////////////////////////////////////////////////////////////
        }


        IEnumerator Use_Short()
        {
            /*
            float dotValue = 0f;
            Vector3 direction;

            dotValue = Mathf.Cos(Mathf.Deg2Rad * (120 / 2)); //120 부분은 범위각
            List<GameObject> TargetMonster = new List<GameObject>();

            yield return new WaitForSeconds(0.1f);

            //충돌 검사할 몬스터 거름망(검사량이 너무 많아지므로)
            foreach (GameObject f in enemys)
            {
                if (f == null)
                {
                    enemys.Remove(f);
                }
                Vector3 dis = f.gameObject.transform.position - p.gameObject.transform.position;
                if (dis.magnitude < 20f)
                {
                    TargetMonster.Add(f);
                    //Debug.Log("근방 몬스터: " + f.name);
                }
                //break;
                
            }

            //범위 내의 몬스터 거름망
            foreach (GameObject f in TargetMonster)
            {
                direction = f.transform.position - p.gameObject.transform.position;
                if (direction.magnitude < 5f)
                {
                    if (Vector3.Dot(direction.normalized, p.gameObject.transform.forward) > dotValue)
                    {
                        //네임스페이스 때문에 내 클래스들은 using K.Monster;를 추가 한 후 사용
                        if (f == null)
                        {
                            break;
                        }
                        if (f.gameObject.name == "PoopNormal")
                            f.GetComponent<M_Dump>().DATA.Hp -= p.GetComponent<weaponController>().plDemage.demage;
                        if (f.gameObject.name == "ShootingNomal")
                            f.GetComponent<M_Shooter>().DATA.Hp -= p.GetComponent<weaponController>().plDemage.demage;
                        if (f.transform.GetChild(2).transform.localScale.x >= 0f)
                        {
                            if (f.gameObject.name == "PoopNormal")
                            {
                                f.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                f.GetComponent<Rigidbody>().AddForce(direction.normalized * 5f, ForceMode.VelocityChange);
                                //체력바가 0~1인데 체력이 20에 공격력이 5면 음 5/20을 빼면 되나?
                                if (f.transform.GetChild(2).transform.localScale.x > (p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Dump>().DATA.FullHp))
                                    f.transform.GetChild(2).transform.localScale -= new Vector3((GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Dump>().DATA.FullHp), 0f, 0f);
                                else
                                    f.transform.GetChild(2).transform.localScale = new Vector3(0f, 0f, 0f);
                            }
                            if (f.gameObject.name == "ShootingNomal")
                            {
                                if (f.transform.GetChild(2).transform.localScale.x > (p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Shooter>().DATA.FullHp))
                                    f.transform.GetChild(2).transform.localScale -= new Vector3((p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Shooter>().DATA.FullHp), 0f, 0f);
                                else
                                    f.transform.GetChild(2).transform.localScale = new Vector3(0f, 0f, 0f);
                            }
                            Debug.Log(f.transform.GetChild(2).transform.localScale.x);

                        }
                    }
                }
            }
            */
            hit1.SetActive(true);
            yield return new WaitForSeconds(0.1f);

        }

        IEnumerator Use_Long()
        {
            /*
            float dotValue = 0f;
            Vector3 direction;

            dotValue = Mathf.Cos(Mathf.Deg2Rad * (180 / 2));
            List<GameObject> TargetMonster = new List<GameObject>();

            yield return new WaitForSeconds(0.5f);

            //충돌 검사할 몬스터 거름망(검사량이 너무 많아지므로)
            foreach (GameObject f in enemys)
            {
                if (f == null)
                {
                    enemys.Remove(f);
                }
                Vector3 dis = f.gameObject.transform.position - p.gameObject.transform.position;
                if (dis.magnitude < 25f)
                {
                    TargetMonster.Add(f);
                    //Debug.Log("근방 몬스터: " + f.name);
                }
                //break;
            }

            //범위 내의 몬스터 거름망
            foreach (GameObject f in TargetMonster)
            {
                direction = f.transform.position - p.gameObject.transform.position;
                if (direction.magnitude < 7f)
                {
                    if (Vector3.Dot(direction.normalized, p.gameObject.transform.forward) > dotValue)
                    {
                        //네임스페이스 때문에 내 클래스들은 using K.Monster;를 추가 한 후 사용
                        if (f == null)
                        {
                            break;
                        }
                        if (f.gameObject.name == "PoopNormal")
                            f.GetComponent<M_Dump>().DATA.Hp -= p.GetComponent<weaponController>().plDemage.demage;
                        if (f.gameObject.name == "ShootingNomal")
                            f.GetComponent<M_Shooter>().DATA.Hp -= p.GetComponent<weaponController>().plDemage.demage;
                        //Debug.Log("use_long: 몬스터가 범위 내에 있음, monster hp:" + f.GetComponent<M_Dump>().DATA.Hp);
                        if (f.transform.GetChild(2).transform.localScale.x >= 0f)
                        {
                            if (f.gameObject.name == "PoopNormal")
                            {
                                f.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                f.GetComponent<Rigidbody>().AddForce(direction.normalized * 5f, ForceMode.VelocityChange);
                                //체력바가 0~1인데 체력이 20에 공격력이 5면 음 5/20을 빼면 되나?
                                if (f.transform.GetChild(2).transform.localScale.x > (p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Dump>().DATA.FullHp))
                                    f.transform.GetChild(2).transform.localScale -= new Vector3((p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Dump>().DATA.FullHp), 0f, 0f);
                                else
                                    f.transform.GetChild(2).transform.localScale = new Vector3(0f, 0f, 0f);
                            }
                            if (f.gameObject.name == "ShootingNomal")
                            {
                                if (f.transform.GetChild(2).transform.localScale.x > (p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Shooter>().DATA.FullHp))
                                    f.transform.GetChild(2).transform.localScale -= new Vector3((p.GetComponent<weaponController>().plDemage.demage / f.GetComponent<M_Shooter>().DATA.FullHp), 0f, 0f);
                                else
                                    f.transform.GetChild(2).transform.localScale = new Vector3(0f, 0f, 0f);
                            }
                            Debug.Log(f.transform.GetChild(2).transform.localScale.x);
                        }
                    }

                }
            }
            */
            hit2.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        IEnumerator Use_fire()
        {

            enemys = new List<GameObject>();
            //맵에 있는 몬스터 리스트
            enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();
            for (int i = 0; i < enemys.Count; i++)
                Debug.Log("field enemy[" + i + "]: " + enemys[i].name);


            Vector3 direction;

            List<GameObject> TargetMonster = new List<GameObject>();

            yield return new WaitForSeconds(0.4f);

            //충돌 검사할 몬스터 거름망(검사량이 너무 많아지므로)
            foreach (GameObject f in enemys)
            {
                if (f == null)
                {
                    enemys.Remove(f);
                }
                Vector3 dis = f.gameObject.transform.position - p.gameObject.transform.position;
                if (dis.magnitude < 10f)
                {
                    TargetMonster.Add(f);
                    //Debug.Log("근방 몬스터: " + f.name);
                }
                //break;
            }

            foreach (GameObject f in TargetMonster)
            {
                direction = f.transform.position - p.gameObject.transform.position;
                if (direction.magnitude < 40f)
                {
                    enemy = TargetMonster[0];
                    foreach (GameObject found in TargetMonster)
                    {
                        float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

                        if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                        {
                            enemy = found;
                            shortDis = Distance;
                        }
                    }
                    //Debug.Log("제일 가까운 적: " + enemy.name);
                }
            }
            GameObject instantFire = Instantiate(fire, firepos.position, firepos.rotation);
            //GameObject instantFire = Instantiate(fire, firepos.position, firepos.rotation, this.transform);
        }

    }
}
