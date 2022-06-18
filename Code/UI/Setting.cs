using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KMJ.UI
{
    public class Setting : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        GameObject target;

        [SerializeField]
        Sprite soundSpriteOff;
        [SerializeField]
        Sprite soundSpriteOn;
        bool soundCheck = true;
        int sceneCount = 0;

        public void OnPointerDown(PointerEventData eventData)
        {
            switch(this.gameObject.name)
            {
                case "SettingButton":
                    target.SetActive(true);
                    this.gameObject.SetActive(false);
                    break;
                case "StartButton":
                    //튜토리얼 씬 이동
                    //추가...

                    //시작 씬 이동
                    GameObject.Find("SceneManager").GetComponent<SceneManager>().SetTimerFirst(300f);
                    sceneCount = 0;
                    GameManager.Instance().monsterCount = 0;
                    GameManager.Instance().G_TimerStart = false;
                    target.GetComponent<BlackOut>().InStartFadeAnim();
                    StartCoroutine(FadeWaitToSceneChange(target.GetComponent<BlackOut>().FadeTime, "Tutorial"));
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
                    break;
                case "SelectButton":
                    //난이도 선택 씬 이동
                    break;
                case "sound":
                    if(soundCheck == true)
                    {
                        this.gameObject.GetComponent<Image>().sprite = soundSpriteOff;
                        soundCheck = false;
                        GameObject.Find("SceneManager").GetComponent<SceneManager>().SoundSwitch(soundCheck);
                    }
                    else
                    {
                        this.gameObject.GetComponent<Image>().sprite = soundSpriteOn;
                        soundCheck = true;
                        GameObject.Find("SceneManager").GetComponent<SceneManager>().SoundSwitch(soundCheck);
                    }
                    break;
                case "x":
                    target.SetActive(true);
                    this.gameObject.transform.parent.gameObject.SetActive(false);
                    break;
            }
        }

        void Start()
        {
            if(this.gameObject.name == "sound")
            {
                soundCheck = GameManager.Instance().G_volume;
                if(soundCheck == false)
                {
                    this.gameObject.GetComponent<Image>().sprite = soundSpriteOff;
                    soundCheck = false;
                    GameObject.Find("SceneManager").GetComponent<SceneManager>().SoundSwitch(soundCheck);
                }
                else
                {
                    this.gameObject.GetComponent<Image>().sprite = soundSpriteOn;
                    soundCheck = true;
                    GameObject.Find("SceneManager").GetComponent<SceneManager>().SoundSwitch(soundCheck);
                }
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.tag == "Player")
            {
                sceneCount = GameManager.Instance().sceneCount;
                GameManager.Instance().monsterCount = 0;
                if(sceneCount==0)
                {
                    GameManager.Instance().G_TimerStart = true;
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1");
                    target.GetComponent<BlackOut>().InStartFadeAnim();
                    StartCoroutine(FadeWaitToSceneChange(target.GetComponent<BlackOut>().FadeTime, "Stage1"));
                }
                if(sceneCount==1)
                {
                    saveData();
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");   
                    target.GetComponent<BlackOut>().InStartFadeAnim();
                    StartCoroutine(FadeWaitToSceneChange(target.GetComponent<BlackOut>().FadeTime, "Stage2"));
                }
                if(sceneCount==2)
                {
                    saveData();
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("Stage3");
                    target.GetComponent<BlackOut>().InStartFadeAnim();
                    StartCoroutine(FadeWaitToSceneChange(target.GetComponent<BlackOut>().FadeTime, "Stage3"));
                }

                GameManager.Instance().sceneCount++;
            }

        }

        void saveData()
        {
            GameObject p = GameObject.Find("player_cat");
            GameObject S = GameObject.Find("SceneData");
            S.GetComponent<SceneData>().plSTAT = p.GetComponent<playerCat.playerMove>().plSTAT;
            S.GetComponent<SceneData>().plDemage = p.GetComponent<playerWeapon.weaponController>().plDemage;

            S.GetComponent<SceneData>().hasWeapons = p.GetComponent<playerWeapon.weaponController>().hasWeapons;
            S.GetComponent<SceneData>().hasWeaponCount = p.GetComponent<playerWeapon.weaponController>().hasWeaponCount;
            S.GetComponent<SceneData>().isDagger = p.GetComponent<playerWeapon.weaponController>().isDagger;
            S.GetComponent<SceneData>().isStaff = p.GetComponent<playerWeapon.weaponController>().isStaff;
            S.GetComponent<SceneData>().isGiantsword = p.GetComponent<playerWeapon.weaponController>().isGiantsword;

            Debug.Log("플레이어 데이터 저장");
        }

        IEnumerator FadeWaitToSceneChange(float time, string sceneName)
        {
            yield return new WaitForSeconds(time);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}

