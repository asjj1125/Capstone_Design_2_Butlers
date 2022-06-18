using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using playerCat;
using playerWeapon;

public class SceneData : MonoBehaviour
{
    public static SceneData sceneData;

    GameObject player;

    playerCat.playerStatReset stat;
    public playerCat.playerStatReset plSTAT
    {
        get
        {
            return stat;
        }
        set
        {
            stat = value;
        }
    }

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

    public int[] hasWeapons;
    public int hasWeaponCount;
    public bool isSwap;
    int weaponIndex = -1;

    /////////////////////////////////
    public bool isDagger;
    public bool isGiantsword;
    public bool isStaff;
    /////////////////////////////////



    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (sceneData == null)
        {
            sceneData = this;
        }

        else if (sceneData != this)
        {
            Destroy(gameObject);
        }
    }

}
