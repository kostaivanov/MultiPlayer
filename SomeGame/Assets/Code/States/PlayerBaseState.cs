using UnityEngine;

internal abstract class PlayerBaseState
{
    internal abstract void EnterState(PlayerStateManager player);

    internal abstract void UpdateState(PlayerStateManager player);

    internal abstract void OnCollisionEnter(PlayerStateManager player);
}
