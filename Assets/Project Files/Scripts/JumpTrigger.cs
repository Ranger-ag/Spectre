using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : PlayerTrigger<MainPlayerController>
{
    public float jumpHeight = 30;

    protected override void OnPlayerTriggerEnter(MainPlayerController playerController)
    {
        playerController.Jump(jumpHeight);
    }
}
