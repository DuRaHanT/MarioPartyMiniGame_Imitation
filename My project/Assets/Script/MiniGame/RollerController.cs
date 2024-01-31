using UnityEngine;
using Fusion;

namespace MiniGame
{
    public class RollerController : NetworkBehaviour     
    {
        public float rollSpeed = 5f;

        public override void FixedUpdateNetwork()
        {
            float rollAmount = rollSpeed * Runner.DeltaTime * Mathf.Rad2Deg;
            transform.Rotate(Vector3.up, rollAmount);
            base.FixedUpdateNetwork();
        }
    }
}
