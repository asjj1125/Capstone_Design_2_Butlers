using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace playerWeapon
{

    public class weaponController : MonoBehaviour
    {
        public GameObject[] weapons;
        public int[] hasWeapons;
        public int hasWeaponCount;
        public bool isSwap;
        public bool isSet;
        int weaponIndex = -1;

        Animator anim;

        public weapon handObject;

        /////////////////////////////////
        public bool isDagger;
        public bool isGiantsword;
        public bool isStaff;
        /////////////////////////////////
        public SceneData sceneData;

        //이미지 변경
        changeImage image;

        playerCat.playerStatReset atk;
        public playerCat.playerStatReset plDemage
        {
            get
            {
                return atk;
            }
            set
            {
                atk = value;
            }
        }

        void Start()
        {
            
            anim = GetComponentInChildren<Animator>();
            atk = new playerCat.playerStatReset(0);
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
                sceneData = GameObject.Find("SceneData").GetComponent<SceneData>();

            if (SceneManager.GetActiveScene().name != "Stage1" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
            {
                hasWeaponCount = sceneData.hasWeaponCount;
                hasWeapons = sceneData.hasWeapons;
                isDagger = sceneData.isDagger;
                isGiantsword = sceneData.isGiantsword;
                isStaff = sceneData.isStaff;
                setWeapon();
                setDemage();
            }
        }

        void Update()
        {
            swap();
        }

        void setWeapon()
        {
            isSet = true;
            anim.SetTrigger("doSwap");
            if (isDagger)
            {
                weaponIndex = 0;
            }
            else if (isGiantsword)
            {
                weaponIndex = 1;
            }
            else if (isStaff)
            {
                weaponIndex = 2;
            }
            else { return; }

            handObject = weapons[weaponIndex].GetComponent<weapon>();
            handObject.gameObject.SetActive(true);

            anim.SetBool("handDagger", isDagger);
            anim.SetBool("handGiantsword", isGiantsword);
            anim.SetBool("handStaff", isStaff);

            if (weaponIndex == hasWeaponCount) { weaponIndex = 0; };
            image = GameObject.FindObjectOfType<changeImage>();
            image.change();
            isSet = false;
        }

        void swap()
        {

            if (isSwap)
            {
                anim.SetTrigger("doSwap");

                if (hasWeaponCount == 0)
                {
                    return;
                }

                else if (hasWeaponCount == 1)
                {
                    weaponIndex++;
                    if (handObject != null)
                    {
                        handObject.gameObject.SetActive(false);
                    }
                    if (weaponIndex > 0)
                    {
                        weaponIndex = 0;
                    }
                    handObject = weapons[hasWeapons[0]].GetComponent<weapon>();
                    handObject.gameObject.SetActive(true);
                }

                else if (hasWeaponCount == 2)
                {
                    weaponIndex++;
                    if (weaponIndex == 2) { weaponIndex = 0; };
                    if (handObject != null)
                    {
                        handObject.gameObject.SetActive(false);
                    }
                    handObject = weapons[hasWeapons[weaponIndex]].GetComponent<weapon>();
                    handObject.gameObject.SetActive(true);
                }

                else if (hasWeaponCount == 3)
                {
                    weaponIndex++;
                    if (weaponIndex == 3) { weaponIndex = 0; };
                    if (handObject != null)
                    {
                        handObject.gameObject.SetActive(false);
                    }
                    handObject = weapons[hasWeapons[weaponIndex]].GetComponent<weapon>();
                    handObject.gameObject.SetActive(true);
                }
                

                if (hasWeaponCount>0)
                {
                    if (handObject.gameObject.name == "DaggerP") { isDagger = true; isGiantsword = false; isStaff = false; }
                    else if (handObject.gameObject.name == "GiantSwordP") { isDagger = false; isGiantsword = true; isStaff = false; }
                    else if (handObject.gameObject.name == "StaffP") { isDagger = false; isGiantsword = false; isStaff = true; }
                }
                //else { Debug.Log("그럴리가...");};

                anim.SetBool("handDagger", isDagger);
                anim.SetBool("handGiantsword", isGiantsword);
                anim.SetBool("handStaff", isStaff);

                image = GameObject.FindObjectOfType<changeImage>();
                image.change();

                setDemage();

                isSwap = false;

            }
        }

        public void setDemage()
        {
            if(isDagger)
                atk.demage = 6f + GetComponent<playerCat.playerMove>().plSTAT.plusDemage;
            if(isGiantsword) 
                atk.demage = 10f + GetComponent<playerCat.playerMove>().plSTAT.plusDemage;
            if(isStaff)
                atk.demage = 4f + GetComponent<playerCat.playerMove>().plSTAT.plusDemage;
        }

        public void clickSwap()
        {
            isSwap = true;
        }
    }
}
