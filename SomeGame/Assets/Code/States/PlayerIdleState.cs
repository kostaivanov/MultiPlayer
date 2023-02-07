using UnityEngine;

internal class PlayerIdleState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("IDLE");
        //Debug.Log("Playing animation IDLE!");
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.moving == true && player.playerMovement.grounded)
        {
            player.SwitchState(player.runningState);
        }
        if (player.rigidBody.velocity.y > 1f && player.playerMovement.grounded != true)
        {
            player.SwitchState(player.jumpingState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }

}
