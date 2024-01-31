using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

namespace MiniGame
{
    public sealed class PlayerInput : NetworkBehaviour, IBeforeUpdate, IBeforeTick
    {
		public bool HasInput      => _hasInput;
		public GameplayInput CurrentInput  => _currentInput;
		public GameplayInput PreviousInput => _previousInput;

		[SerializeField][Tooltip("Mouse delta multiplier.")]
		private Vector2 _lookSensitivity = Vector2.one;

		// We need to store current input to compare against previous input (to track actions activation/deactivation). It is also used if the input for current tick is not available.
		// This is not needed on proxies and will be replicated to input authority only.
		[Networked]
		private GameplayInput _currentInput { get; set; }

		private bool          _hasInput;
		private GameplayInput _previousInput;
		private GameplayInput _accumulatedInput;
		private bool          _resetAccumulatedInput;

		public override void Spawned()
		{
			// Reset to default state.
			_hasInput              = default;
			_currentInput          = default;
			_previousInput         = default;
			_accumulatedInput      = default;
			_resetAccumulatedInput = default;

			if (Object.HasInputAuthority == true)
			{
				// Register local player input polling.
				NetworkEvents networkEvents = Runner.GetComponent<NetworkEvents>();
				networkEvents.OnInput.AddListener(OnInput);

				if (Application.isMobilePlatform == false || Application.isEditor == true)
				{
					// Hide cursor
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible   = false;
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (runner == null)
				return;

			NetworkEvents networkEvents = runner.GetComponent<NetworkEvents>();
			if (networkEvents != null)
			{
				// Unregister local player input polling.
				networkEvents.OnInput.RemoveListener(OnInput);
			}
		}

		protected override bool ReplicateTo(PlayerRef player)
		{
			// Only local player needs networked properties (previous input).
			// This saves network traffic by not synchronizing networked properties to other clients except local player.
			return player == Object.InputAuthority;
		}

		/// <summary>
		/// 1. Collect input from devices, can be executed multiple times between FixedUpdateNetwork() calls because of faster rendering speed.
		/// </summary>
		void IBeforeUpdate.BeforeUpdate()
		{
			if (Object == null || Object.HasInputAuthority == false)
				return;

			// Accumulated input was polled and explicit reset requested.
			if (_resetAccumulatedInput == true)
			{
				_resetAccumulatedInput = false;
				_accumulatedInput = default;
			}

			if (Application.isMobilePlatform == false || Application.isEditor == true)
			{
				// Input is tracked only if the cursor is locked.
				if (Cursor.lockState != CursorLockMode.Locked)
					return;
			}

			// Always use KeyControl.isPressed, Input.GetMouseButton() and Input.GetKey().
			// Never use KeyControl.wasPressedThisFrame, Input.GetMouseButtonDown() or Input.GetKeyDown() otherwise the action might be lost.

			Mouse mouse = Mouse.current;
			if (mouse != null)
			{
				Vector2 mouseDelta = mouse.delta.ReadValue();
				_accumulatedInput.LookRotationDelta += new Vector2(-mouseDelta.y, mouseDelta.x) * _lookSensitivity;
			}

			Keyboard keyboard = Keyboard.current;
			if (keyboard != null)
			{
				Vector2 moveDirection = Vector2.zero;

				if (keyboard.wKey.isPressed == true) { moveDirection += Vector2.up;    }
				if (keyboard.sKey.isPressed == true) { moveDirection += Vector2.down;  }
				if (keyboard.aKey.isPressed == true) { moveDirection += Vector2.left;  }
				if (keyboard.dKey.isPressed == true) { moveDirection += Vector2.right; }

				_accumulatedInput.MoveDirection = moveDirection.normalized;

				_accumulatedInput.Actions.Set(GameplayInput.JUMP_BUTTON, keyboard.spaceKey.isPressed);
			}
		}

		/// <summary>
		/// 3. Read input from Fusion.
		/// </summary>
		void IBeforeTick.BeforeTick()
		{
			if (Object == null)
				return;

			_hasInput = Object.HasInputAuthority == true || Object.HasStateAuthority == true;
			if (_hasInput == false)
			{
				_currentInput  = default;
				_previousInput = default;
				return;
			}

			// Set current in input as previous.
			_previousInput = _currentInput;

			// Clear all properties which should not propagate from last known input in case of missing new input. As example, following line will reset look rotation delta.
			// This results to the player not being incorrectly rotated (by using rotation delta from last known input) in case of missing input on state authority, followed by a correction on the input authority.
			GameplayInput currentInput = _currentInput;
			currentInput.LookRotationDelta = default;
			_currentInput = currentInput;

			if (Object.InputAuthority != PlayerRef.None)
			{
				// If this fails, the current input won't be updated and input from previous tick will be reused.
				if (GetInput(out GameplayInput input) == true)
				{
					// New input received, we can store it as current.
					_currentInput = input;
				}
			}
		}

		/// <summary>
		/// 2. Push accumulated input and reset properties, can be executed multiple times within single Unity frame if the rendering speed is slower than Fusion simulation.
		/// This is usually executed multiple times if there is a performance spike, for example after expensive spawn which includes asset loading.
		/// </summary>
		private void OnInput(NetworkRunner runner, NetworkInput networkInput)
		{
			// Set accumulated input.
			networkInput.Set(_accumulatedInput);

			// Now we reset all properties which should not propagate into next OnInput() call (for example LookRotationDelta - this must be applied only once and reset immediately).
			// If there's a spike, OnInput() and FixedUpdateNetwork() will be called multiple times in a row without BeforeUpdate() in between, so we don't reset move direction to preserve movement.
			// Move direction and other properties are reset in next BeforeUpdate() - driven by _resetAccumulatedInput flag.
			_accumulatedInput.LookRotationDelta = default;

			// Input is polled for single fixed update, but at this time we don't know how many times in a row OnInput() will be executed.
			// This is the reason to have a reset flag instead of resetting input immediately, otherwise we could lose input for next fixed updates (for example move direction).
			_resetAccumulatedInput = true;
		}
	}
}
