using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{

	Controls controls;

	public CameraInteractions cameraInteractions;

	public LockedNavigation lockedNav;

	public InputDevice currentInputDevice { get; private set; }
	public string currentScheme { get; private set; }

    public bool IsGamepad => currentScheme.Equals("Gamepad");

    public Vector2 controllerSensitivityScale;

    public Shotgun gun; 
    public void Awake()
	{
		controls = new Controls();

		controls.Main.Interact.performed += InteractPressed;
		controls.Main.Running.performed += HandleRunning;
		controls.Main.Pause.performed += Pause;
		controls.Main.UIControllerSelect.performed += UISelect;
        controls.Main.Fire.performed += Fire;
         
        InputSystem.onActionChange += OnActionChange;

		controls.Enable();
	}

	private  void OnActionChange(object obj, InputActionChange change)
	{
		if (change == InputActionChange.ActionPerformed)
		{
			InputDevice lastDevice = ((InputAction)obj).activeControl.device;

			if (lastDevice != currentInputDevice || currentInputDevice == null)
			{
				if (lastDevice.ToString().Contains("Mouse") || lastDevice.ToString().Contains("Keyboard"))
				{
					if (currentScheme != "Keyboard")
						lockedNav.OnControllerInput(false);

					currentScheme = "Keyboard";
				}
				else
				{
					if (currentScheme != "Gamepad")
						lockedNav.OnControllerInput(true);

					currentScheme = "Gamepad"; 
				}
			}
		}
	}

	public void OnDestroy()
	{
		controls.Main.Interact.performed-= InteractPressed;
		controls.Main.Running.performed -= HandleRunning;
		controls.Main.Pause.performed -= Pause;
		controls.Main.UIControllerSelect.performed -= UISelect;
        controls.Main.Fire.performed -= Fire;
      

		InputSystem.onActionChange -= OnActionChange;
	}

	public void Update()
	{ 
		if (GameManager.Instance)
		{
			if (GameManager.Instance.AllowInput)
			{
				PlayerController.Instance.moveInput = controls.Main.Move.ReadValue<Vector2>();

                if (!IsGamepad)
                {
                    PlayerController.Instance.lookInput = controls.Main.Look.ReadValue<Vector2>() * 0.5f * 0.1f;
                }
                else
                {
                    PlayerController.Instance.lookInput = controls.Main.Look.ReadValue<Vector2>() * Time.deltaTime * controllerSensitivityScale; 
                }
                //PlayerController.Instance.lookInput = controls.Main.Look.ReadValue<Vector2>() * 0.5f * 0.1f;
			}
			else
			{
				PlayerController.Instance.moveInput = Vector2.zero;
				PlayerController.Instance.lookInput = Vector2.zero;
			}
		}

		lockedNav.Input(controls.Main.UIController.ReadValue<float>());  
	}

    public void Fire(InputAction.CallbackContext ctx)
    { 
        if (ctx.performed && !GameManager.Instance.GameOver) 
        { 
            gun.TryFire(); 
        }
    }

	public void UISelect(InputAction.CallbackContext ctx)
	{
		if (lockedNav.gameObject.activeInHierarchy)
		{
			lockedNav.SelectButton();
		}
	}

	public void InteractPressed(InputAction.CallbackContext ctx)
	{
		if (GameManager.Instance && GameManager.Instance.AllowInput)
		{
			cameraInteractions.TryInteract();
		}
	}

    public void Pause(InputAction.CallbackContext ctx)
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.Pause();
			}
    }

	public void HandleRunning(InputAction.CallbackContext ctx)
	{
			if (GameManager.Instance && GameManager.Instance.AllowInput)
			{
				PlayerController.Instance.isRunning = ctx.ReadValueAsButton();
			}
	}

}
