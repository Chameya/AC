using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  PlayerControls playerControls;
  PlayerLocomotion playerLocomotion;
  AnimatorManager animatorManager;
  public Vector2 movementInput;
  public Vector2 cameraInput;
  public float cameraInputX;
  public float cameraInputY;
  public float moveAmount;
  public float verticalInput;
  public float horizontalInput;

  [SerializeField] bool sprintInput;
  [SerializeField] bool dodgeInput;
  [SerializeField] bool jumpInput;
  [SerializeField] bool lightAttackInput;
  [SerializeField] bool heavyAttackInput;
  [SerializeField] bool FirstWeaponInput;

  public float jump; 
  public float fire;// Light attack
  public float fire1; // heavy attack
  public float shift;
  [HideInInspector]public float rawVertical , rawHorizontal;

  private void Awake()
  {
    animatorManager = GetComponent<AnimatorManager>();
    playerLocomotion = GetComponent<PlayerLocomotion>();
  }

  private void OnEnable()
{
    if(playerControls == null)
    {
        playerControls = new PlayerControls();
        playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue <Vector2>();

        playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
        playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

        playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
        playerControls.PlayerActions.Dodge.canceled += i => dodgeInput = false;

        playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
        playerControls.PlayerActions.Jump.canceled += i => jumpInput = false;

        playerControls.PlayerActions.FirstWeapon.performed += i => FirstWeaponInput = true;
        playerControls.PlayerActions.FirstWeapon.canceled += i => FirstWeaponInput = false;

        playerControls.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
        playerControls.PlayerActions.LightAttack.canceled += i => lightAttackInput = false;

        playerControls.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
        playerControls.PlayerActions.HeavyAttack.canceled += i => heavyAttackInput = false;
    }

    playerControls.Enable();
}

private void OnDisable()
{
    playerControls.Disable();
}


  public void HandleAllInputs()
  {
    HandleMovementInput ();
    HandleSprintingInput();
    HandleJumpingInput();
    HandleDodgeInput();
    HandleLightAttackInput();
    HandleHeavyAttackInput();
  }

  private void HandleMovementInput ()
  {
    verticalInput = movementInput.y;
    horizontalInput = movementInput.x;

    cameraInputY = cameraInput.y;
    cameraInputX = cameraInput.x;

    moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    animatorManager.UpdateAnimatorValues(0,moveAmount,playerLocomotion.isSprinting);
  }

  private void HandleSprintingInput()
  {
    if(sprintInput && moveAmount>0.5f)
    {
      playerLocomotion.isSprinting = true;
    }
    else
    {
      playerLocomotion.isSprinting = false;
    }
  }

  private void HandleJumpingInput()
  {
    if (jumpInput)
    {
      jumpInput = false;
      playerLocomotion.HandleJumping();
    }
  }

  private void HandleDodgeInput()
  {
    if(dodgeInput)
    {
      dodgeInput = false;
      playerLocomotion.HandleDodge();
    }
  }

 private void HandleFirstWeaponInput()
  {
      if (FirstWeaponInput)
      {
          FirstWeaponInput = false;
          playerLocomotion.HandleFirstWeapon();
      }
  }

  private void HandleLightAttackInput()
  {
      if (lightAttackInput)
      {
          lightAttackInput = false;
          playerLocomotion.HandleLightAttack();
      }
  }

  private void HandleHeavyAttackInput()
  {
      if (heavyAttackInput)
      {
          heavyAttackInput = false;
          playerLocomotion.HandleHeavyAttack();
      }
  }
  
}