using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy__Boss_Dead : Enemy_Dead
{
    private Enemy_Boss enemy_Boss;
    public Enemy__Boss_Dead(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Boss = en as Enemy_Boss;
    }
    public override void Enter()
    {
        base.Enter();
        enemy_Boss.airWall.GetComponent<AirWall>().DeactivateWall();
        UIManager.Instance.HidePanel<BossHealth_BarPanel>();
        enemy_Boss.airWall.gameObject.SetActive(false);
        bkMusic.Instance.ChangeAudioClip("game_bk_music");
    }
}
