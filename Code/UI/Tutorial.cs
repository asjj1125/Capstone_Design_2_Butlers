using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject disImage1;
    public GameObject disImage2;
    public GameObject disImage3;

    public GameObject monster;
    public GameObject monster2;
    public GameObject potal;

    public Text Script;

    int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "item" && count == 0)
        {
            Destroy(disImage1);
            Script.text = "무기를 획득하면 공격을 할 수 있습니다.";
            count++;
        }
        if (transform.GetComponent<playerWeapon.weaponController>().hasWeaponCount == 3 && count == 1)
        {
            Destroy(disImage3);
            Script.text = "무기를 교체할 수 있습니다.";
            count++;
        }
        if (other.name == "Collider2" && count == 2)
        {
            Destroy(disImage2);
            Script.text = "회피로 장애물을 피할 수 있습니다.";
            count++;
        }
        if (other.name == "Collider3" && count == 3)
        {
            monster.SetActive(true);
            Script.text = "몬스터를 무찌르고 포탈을 건너 생선을 되찾으세요!";
            count++;
        }
    }
}
