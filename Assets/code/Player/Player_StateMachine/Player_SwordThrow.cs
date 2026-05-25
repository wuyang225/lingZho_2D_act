using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SwordThrow : PlayerState
{
    private float isfill = 1;
    private Camera mainCam;

    public Player_SwordThrow(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }

    public override void Enter()
    {
        base.Enter();
        mainCam = Camera.main;
        if (mainCam == null)
        {
            return;
        }
        skillsManager.swordThrow.EnableDots(true);
    }

    public override void Update()
    {
        base.Update();
        player.setvelocity(0, rigi.velocity.y);
        //将鼠标屏幕坐标转换为世界坐标（Z值设为相机到玩家的距离，确保在同一平面）
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCam.WorldToScreenPoint(player.transform.position).z // 保证Z轴一致，避免深度偏移
        ));
        mouseWorldPos.z = 0; // 2D游戏强制Z轴为0

        // 计算「玩家→鼠标」的方向向量，并归一化（消除距离影响，仅保留方向）
        Vector2 throwDirection = (mouseWorldPos - player.transform.position).normalized;

        // 翻转逻辑
        isfill = player.mousePosition.x < Screen.width / 2 ? -1 : 1;
        player.HandleFilp(isfill);

        skillsManager.swordThrow.PredictTrajectory(throwDirection);

        if (Input.GetMouseButton(0))
        {
            anim.SetBool("playerThrowSword_Performed", true);
            skillsManager.swordThrow.ConfirmTrajectory(throwDirection); // 同步修改确认方向
        }

        if (Input.GetMouseButton(1) || triggeratt)
        {
            player.statemachine.ChangeState(player.idlestate);
            skillsManager.swordThrow.EnableDots(false);
        }
    }

    public override void Exit()
    {
        anim.SetBool("playerThrowSword_Performed", false);
        base.Exit();
        skillsManager.swordThrow.EnableDots(false);
    }
}