using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K.Monster
{
    public class Niddle : MonoBehaviour
    {
        //[SerializeField]
        float speed = 20f;
        NormalMonster data;
        void Start()
        {
            data = this.transform.parent.gameObject.GetComponent<M_Shooter>().DATA;
            Destroy(this.gameObject, 0.8f);
        }

        void Update()
        {
            this.transform.rotation = this.transform.parent.transform.rotation;
            this.transform.localPosition += new Vector3(0f, 0f, 1f * speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            Destroy(this.gameObject);
        }
    }
}

