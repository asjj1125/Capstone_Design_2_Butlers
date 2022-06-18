using UnityEngine;

namespace K.Monster
{
    public class BossMonster : MonsterBase
    {
        public BossMonster(float atk, float hp, type t) : base(atk, hp, t) { FullHp = hp;}

        public float FullHp;
        bool m_hitBox;
        //위치 정보
        Vector3 m_pos;
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

        //체력 1000에 공격 1하트
        //보스몹은 각 무기에 대한 공격력 보정이 필요할 것 같음, 근접 x5에 원거리 x3?
        //3가지 패턴 돌려막기(기본 패턴 2개에 큰 패턴 1개)
        //피가 80% 60% 40% 20% 일 때 큰 패턴 사용
        //큰 패턴은 광역기? 치즈나 똥 부하를 걍 오브젝트 형식으로 하늘에서 소환해 던진다던가
        //맵 크기는 적당히 넓게(강의실 4배?)
        //돌진, 또 뭐가 있었지
        //큰 기술 사용 시 후에 다운 패턴?
        //던져진 부하를 일정 시간 내(큰 기술 사용후 시간이 지나면 부하 수거) 모두 처리하면 다운
        


    }
}
