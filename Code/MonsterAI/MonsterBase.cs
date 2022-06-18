namespace K.Monster
{
    public class MonsterBase
    {
        float m_attack;
        float m_hp;
        public enum type
        {
            normal,
            boss
        }
        type m_type;


        public float Attack
        {
            get{return m_attack;}
            set{m_attack = value;}
        }
        public float Hp        
        {
            get{return m_hp;}
            set{m_hp = value;}
        }
        public type Type
        {
            get 
            {return m_type;}
            set
            {m_type = value;}
        }

        public MonsterBase(float atk, float hp, type t)
        {
            m_attack = atk;
            m_hp = hp;
            m_type = t;
        }
    }
}

