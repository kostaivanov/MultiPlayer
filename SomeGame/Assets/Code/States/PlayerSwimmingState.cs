using UnityEngine;

internal class PlayerSwimmingState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Swimming");
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.grounded == true && player.playerMovement.moving == true)
        {
            player.playerJump.swimming = false;
            player.SwitchState(player.runningState);
        }

        else if(player.playerMovement.grounded == true)
        {
            player.playerJump.swimming = false;
            player.SwitchState(player.idleState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }


}
