using UnityEngine;

namespace K.Monster
{
    public class NormalMonster : MonsterBase
    {
        public NormalMonster(float atk, float hp, type t) : base(atk, hp, t) { FullHp = hp;}
        
        //히트박스 온/오프
        bool m_hitBox;
        //위치 정보
        Vector3 m_pos;
        //인스턴스 hp
        public float FullHp;

        public bool HitBox
        {
            get{return m_hitBox;}
            set{m_hitBox = value;}
        }
        public Vector3 Position
        {
            get{return m_pos;}
            set{m_pos = value;}
        }
    }
}

