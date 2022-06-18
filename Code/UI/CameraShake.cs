using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float t;
    float x;
    Vector3 s;
    Vector3 e;
    Vector3 sp;
    Vector3 mp;
    Vector3 ep;

    void Start()
    {
        s = this.transform.eulerAngles;
        e = new Vector3(this.transform.eulerAngles.x - 20f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        sp = this.transform.position;
        mp = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 15f);
        Debug.Log(mp);
        ep = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 30f);
    }

    void Update()
    {
        if(t < 10)
        {
            t += Time.deltaTime;
            //Debug.Log(t);
            //x += Time.deltaTime*0.01f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(e), Time.deltaTime*0.3f);
            
            this.transform.position = Vector3.Lerp(this.transform.position, ep, 10*Time.deltaTime/Vector3.Distance(sp, ep));
            //this.transform.position  = Vector3.Lerp(this.transform.position, ep, Time.deltaTime*0.22f);
            //this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x - x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            //this.transform.position += new Vector3(0, 0, -x);
        }
        //딜레이(10초)
        if(t >= 10 && t < 20)
        {
            t += Time.deltaTime;
        }
        if(t >= 20 && t < 30)
        {
            t += Time.deltaTime;
            //x += Time.deltaTime*0.01f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(s), Time.deltaTime*0.3f);
            this.transform.position = Vector3.Lerp(this.transform.position, sp, 10*Time.deltaTime/Vector3.Distance(ep, sp));
        }
        //딜레이(30초)
        if(t >= 30 && t < 60)
        {
            t += Time.deltaTime;
        }
        if(t >= 60)
        {
            t = 0;
        }
    }
}
