using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInput : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool crouch;
    public bool jump;
    public bool sprint;
    public bool mouseL;
    public bool mouseR;
    public bool skill1;
    public bool skill2;
    public bool ultimate;
    public bool reload;
    public bool granade;
    public bool interact;
    public bool inventory;
    public bool holster;
    public bool alpha1;
    public bool alpha2;
    public bool escape;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("UI Settings")]
    public bool isFocus = true;

    public void OnMove(InputValue value)
    {
        if (isFocus)
            MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (!isFocus)
            return;
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (isFocus)
            CrouchInput(value.isPressed);
    }

    public void OnJump(InputValue value)
    {
        if (isFocus)
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        if (isFocus)
        SprintInput(value.isPressed);
    }

    public void OnMouseL(InputValue value)
    {
        if (isFocus)
        MouseLInput(value.isPressed);
    }

    public void OnMouseR(InputValue value)
    {
        if (isFocus)
        MouseRInput(value.isPressed);
    }

    public void OnSkill1(InputValue value)
    {
        if (isFocus)
        Skill1Input(value.isPressed);
    }

    public void OnSkill2(InputValue value)
    {
        if (isFocus)
        Skill2Input(value.isPressed);
    }

    public void OnUltimate(InputValue value)
    {
        if (isFocus)
        UltimateInput(value.isPressed);
    }

    public void OnReload(InputValue value)
    {
        if (isFocus)
        ReloadInput(value.isPressed);
    }

    public void OnGranade(InputValue value)
    {
        if (isFocus)
        GranadeInput(value.isPressed);
    }

    public void OnInteract(InputValue value)
    {
        if (isFocus)
        InteractInput(value.isPressed);
    }

    public void OnInventory(InputValue value)
    {
        if (isFocus)
        InventoryInput(value.isPressed);
    }

    public void OnHolster(InputValue value)
    {
        if (isFocus)
        HolsterInput(value.isPressed);
    }

    public void OnAlpha1(InputValue value)
    {
        if (isFocus)
        Alpha1Input(value.isPressed);
    }

    public void OnAlpha2(InputValue value)
    {
        if (isFocus)
        Alpha2Input(value.isPressed);
    }

    public void OnEscape(InputValue value)
    {
        EscapeInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void CrouchInput(bool newCrouchState)
    {
        crouch = newCrouchState;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void MouseLInput(bool newMouseLState)
    {
        mouseL = newMouseLState;
    }

    public void MouseRInput(bool newMouseRState)
    {
        mouseR = newMouseRState;
    }

    public void Skill1Input(bool newSkill1State)
    {
        skill1 = newSkill1State;
    }

    public void Skill2Input(bool newSkill2State)
    {
        skill2 = newSkill2State;
    }

    public void UltimateInput(bool newUltimateState)
    {
        ultimate = newUltimateState;
    }

    public void ReloadInput(bool newReloadState)
    {
        reload = newReloadState;
    }

    public void GranadeInput(bool newGranadeState)
    {
        granade = newGranadeState;
    }

    public void InteractInput(bool newInteractState)
    {
        interact = newInteractState;
    }

    public void InventoryInput(bool newInventoryState)
    {
        inventory = newInventoryState;
    }

    public void HolsterInput(bool newHolsterState)
    {
        holster = newHolsterState;
    }

    public void Alpha1Input(bool newAlpha1State)
    {
        alpha1 = newAlpha1State;
    }

    public void Alpha2Input(bool newAlpha2State)
    {
        alpha2 = newAlpha2State;
    }

    private void EscapeInput(bool newEscapeState)
    {
        escape = newEscapeState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}