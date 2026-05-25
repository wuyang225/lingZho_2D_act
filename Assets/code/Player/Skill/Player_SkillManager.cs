using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_SwordQi swordQi { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    private void Awake()
    {
        dash = this.GetComponentInChildren<Skill_Dash>();
        swordQi = this.GetComponentInChildren<Skill_SwordQi>();
        shard = this.GetComponentInChildren<Skill_Shard>();
        swordThrow = this.GetComponentInChildren<Skill_SwordThrow>();
        
    }

}
