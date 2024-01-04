using UnityEngine;

internal class PlayerStoppingState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Stopping");
    }
    internal override void UpdateState(PlayerStateManager player)
    {
        if (Mathf.Abs(player.playerMovement.velocity.x) < 0.1f && player.playerMovement.controller2D.collisions.below == true)
        {
            player.SwitchState(player.idleState);
        }
        else if (player.playerMovement.inputVector.x != 0 && player.playerMovement.controller2D.collisions.below == true)
        {
            player.SwitchState(player.runningState);
        }
    }
    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }


}
