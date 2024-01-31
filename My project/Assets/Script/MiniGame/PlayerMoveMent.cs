using Fusion;
using Fusion.Addons.SimpleKCC;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace MiniGame
{
    public class PlayerMoveMent :NetworkBehaviour
    {
        public SimpleKCC kcc;
        public PlayerInput input;

        float moveSpeed = 10.0f;
        float jumpPower = 5.0f;
        public float GroundAcceleration = 55.0f;
        public float GroundDeceleration = 25.0f;
        public float AirAcceleration = 25.0f;
        public float AirDeceleration = 1.3f;


        [Networked] private Vector3 _moveVelocity { get; set; }


        public override void FixedUpdateNetwork()
        {
            if(input.HasInput == true)
    {
                // Apply look rotation delta. This propagates to Transform component immediately.
                kcc.AddLookRotation(input.CurrentInput.LookRotationDelta);

                // Set default world space input direction and jump impulse.
                Vector3 inputDirection = kcc.TransformRotation * new Vector3(input.CurrentInput.MoveDirection.x, 0.0f, input.CurrentInput.MoveDirection.y);
                Vector3 jumpImpulse = default;

                // Comparing current input to previous input - this prevents glitches when input is lost.
                if (input.CurrentInput.Actions.WasPressed(input.PreviousInput.Actions, GameplayInput.JUMP_BUTTON) == true)
                {
                    if (kcc.IsGrounded == true)
                    {
                        // Set world space jump vector.
                        jumpImpulse = Vector3.up * jumpPower;
                    }
                }
                Move(inputDirection);

                Jump(jumpImpulse);
            }
        }

        void Move(Vector3 inputDirection)
        {
            Vector3 desiredMoveVelocity = inputDirection * moveSpeed;

            float acceleration;
            if (desiredMoveVelocity == Vector3.zero)
            {
                // No desired move velocity - we are stopping.
                acceleration = kcc.IsGrounded == true ? GroundDeceleration : AirDeceleration;
            }
            else
            {
                acceleration = kcc.IsGrounded == true ? GroundAcceleration : AirAcceleration;
            }

            _moveVelocity = Vector3.Lerp(_moveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);

            kcc.Move(_moveVelocity, Vector3.zero);
        }

        void Jump(Vector3 jumpImpulse)
        {
            if (jumpImpulse != Vector3.zero)
            {
                kcc.Move(Vector3.zero, jumpImpulse);
            }
        }
    }
}
