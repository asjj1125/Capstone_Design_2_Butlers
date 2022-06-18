using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noDestroyObj : MonoBehaviour
{
    private static noDestroyObj s_Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (s_Instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
