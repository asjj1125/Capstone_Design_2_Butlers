using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using K.Monster;

namespace KMJ.UI
{
    public class SceneManager : MonoBehaviour
    {
        bool gameEnd;
        //static GameManager GM;
        void Awake()
        {
            //GM = new GameManager();
            SoundSwitch(GameManager.Instance().G_volume);
            gameEnd = false;
        }

        void Start()
        {
            if(GameManager.Instance().G_TimerStart == true)
            {
                SetTimerScene();
            }
        }

        void Update()
        {
            if(GameObject.Find("Rat") != null)
            {
                if(GameObject.Find("Rat").GetComponent<M_Rat>().DATA.FullHp <= 0f && gameEnd == false)
                   {
                       gameEnd = true;
                        GameManager.Instance().G_TimerStart = false;
                    if(PlayerPrefs.HasKey("playerScore"))
                    {
                        float bestScore = (PlayerPrefs.GetFloat("playerScore", 300) > (300 - GameManager.Instance().G_Timer)) ?
                           (300 - GameManager.Instance().G_Timer) : (PlayerPrefs.GetFloat("playerScore", 300));
                        PlayerPrefs.SetFloat("playerScore", bestScore);
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("playerScore", 300 - GameManager.Instance().G_Timer);
                    }
                        PlayerPrefs.Save();
                        Debug.Log(PlayerPrefs.GetFloat("playerScore"));
                       GameObject.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.Instance().G_Timer.ToString("F0") + "\n" + "Clear!";

                        StartCoroutine("ChangeScene", 10f);
                       return;
                   } 
            }

            if(GameManager.Instance().G_TimerStart == true && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "StartMap")
            {
                if(GameManager.Instance().G_Timer > 0)
                {
                    GameManager.Instance().G_Timer -= Time.deltaTime;
                    SetTimerScene();
                }
                else
                {
                    GameManager.Instance().G_TimerStart = false;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("StartMap");
                }

            }
        }

        public IEnumerator ChangeScene(float time)
        {
            yield return new WaitForSeconds(5f);
            GameObject.Find("Time").SetActive(false);
            yield return new WaitForSeconds(2f);
            GameObject.Find("blackout").GetComponent<BlackOut>().InStartFadeAnim();
            yield return new WaitForSeconds(time -2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartMap");
        }

        public void SoundSwitch(bool check)
        {
            if(check == true)
            {
                this.GetComponent<AudioSource>().mute = false;
                GameManager.Instance().G_volume = true;
            }
            else
            {
                this.GetComponent<AudioSource>().mute = true;
                GameManager.Instance().G_volume = false;
            }
        }

        public void SetTimerFirst(float time)
        {
            GameManager.Instance().G_Timer = time;
        }

        void SetTimerScene()
        {
            GameObject.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.Instance().G_Timer.ToString("F0");
        }

    }
}
