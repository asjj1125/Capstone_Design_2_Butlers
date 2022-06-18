using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : MonoBehaviour
{
    public float FadeTime = 2f;
    Image fadeImg;
    float start;
    float end;
    float time = 0f;
    bool isPlaying = false;

    void Awake()
    {
        fadeImg = this.transform.GetChild(0).GetComponent<Image>();
        OutStartFadeAnim();
    }
    public void OutStartFadeAnim()
    {
        if (isPlaying == true) //�ߺ��������
        {
            return;
        }
        start = 1f;
        end = 0f;
        StartCoroutine("fadeoutplay");    //�ڷ�ƾ ����
    }
    public void InStartFadeAnim()
    {
        if (isPlaying == true) //�ߺ��������
        {
            return;
        }
        start = 0f;
        end = 1f;
        StartCoroutine("fadeinplay");
    }
    IEnumerator fadeoutplay()
    {
        isPlaying = true;

        Color fadecolor = fadeImg.color;
        time = 0f;
        //color.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }
        isPlaying = false;
    }
    IEnumerator fadeinplay()
    {
        isPlaying = true;

        Color fadecolor = fadeImg.color;
        time = 0f;
        //color.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a < 1f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }
        isPlaying = false;
    }
}
