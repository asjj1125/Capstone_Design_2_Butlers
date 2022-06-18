using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMJ.UI
{
    public class GameManager
    {
        //게임 시작 시 종료될 때까지 계속 실행되는 매니저
        //사운드 볼륨, 씬 별로 씬 매니저를 통해 사운드 조절

        public bool G_volume;
        public bool G_EndTutorial;
        public List<float> G_Score;
        public float G_Timer;
        public bool G_TimerStart;
        static GameManager instance = null;
        //
        public int sceneCount;
        public int monsterCount;

        public GameManager()
        {
            G_volume = true;
            G_EndTutorial = false;
            G_Score = new List<float>();
            G_Timer = 0f;
            G_TimerStart = false;
            sceneCount = 0;
            monsterCount = 0;
        }

        public static GameManager Instance()
        {
            if(instance == null)
                instance = new GameManager();

            return instance;
        }
    }
}

