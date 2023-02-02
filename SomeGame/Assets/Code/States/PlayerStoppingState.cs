using UnityEngine;

internal class PlayerStoppingState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Stopping");
    }
    internal override void UpdateState(PlayerStateManager player)
    {
        if (!player.playerMovement.playerInputActions.Player.Movement.IsPressed() && player.rigidBody.velocity.x == 0 && player.playerMovement.grounded == true)
        {
            player.SwitchState(player.idleState);
        }
    }
    internal override void OnCollisionEnter(PlayerStateManager player)
    {
        
    }


}
