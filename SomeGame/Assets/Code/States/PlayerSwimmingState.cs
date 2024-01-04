using UnityEngine;

internal class PlayerSwimmingState : PlayerBaseState
{
    private float initialGravity;
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Swimming");
        //initialGravity = player.rigidBody.gravityScale;
        //player.rigidBody.gravityScale = 0;
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.controller2D.collisions.below == true && player.playerMovement.inputVector.x != 0)
        {
            player.playerMovement.swimming = false;
            player.SwitchState(player.runningState);
            //player.playerMovement.gravityScale = initialGravity;
        }

        else if(player.playerMovement.controller2D.collisions.below == true)
        {
            player.playerMovement.swimming = false;
            player.SwitchState(player.idleState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }


}
