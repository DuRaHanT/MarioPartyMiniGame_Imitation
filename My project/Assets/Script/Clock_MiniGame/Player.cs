using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    CharacterController playerController;
    Vector3 _velocity;

    float jumpPower = 5.0f;
    float GravityValue = -9.81f;
    bool _jumpPressed = false;

    void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        if (playerController.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        Vector3 move = Vector3.zero;

        _velocity.y += GravityValue * Runner.DeltaTime;
        if (_jumpPressed && playerController.isGrounded)
        {
            _velocity.y += jumpPower;
        }
        playerController.Move(move + _velocity * Runner.DeltaTime);

        _jumpPressed = false;
    }
}
