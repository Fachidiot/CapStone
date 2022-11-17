using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject Interface;
    public GameObject Mainmenu;
    public GameObject Option;
    public GameObject reload;
    public RawImage weaponImg;

    public Texture2D[] weaponImages;
    
    [SerializeField]ActiveWeapon.WeaponSlot currentWeapon = ActiveWeapon.WeaponSlot.None;
    ActiveWeapon weaponManager;
    CustomInput playerInput;
    Cinemachine.CinemachineFreeLook playerCamera;

    void Start()
    {
        playerInput = FindObjectOfType<CustomInput>();
        weaponManager = FindObjectOfType<ActiveWeapon>();
        playerCamera = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
    }
    
    void Update() {
        OpenMenu();
        UpdateWeaponUI();
    }

    void UpdateWeaponUI() {
        currentWeapon = weaponManager.currentWeapon;
        if (currentWeapon != ActiveWeapon.WeaponSlot.None) {
            weaponImg.gameObject.SetActive(true);

            reload.SetActive(weaponManager.isNeedReload);
            switch (currentWeapon) {
                case ActiveWeapon.WeaponSlot.Primary:
                    weaponImg.texture = weaponImages[0];
                    break;
                case ActiveWeapon.WeaponSlot.Secondary:
                    weaponImg.texture = weaponImages[1];
                    break;
            }
        } else
            weaponImg.gameObject.SetActive(false);
    }

    bool currentState = false;
    void OpenMenu() {
        if (playerInput.escape) {
            if (Option.activeSelf) {
                Option.SetActive(false);
                Mainmenu.SetActive(true);
                playerInput.escape = false;
                return;
            }
            Interface.SetActive(currentState);
            Mainmenu.SetActive(!currentState);
            Cursor.lockState = currentState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !currentState;
            playerInput.isFocus = currentState;
            playerCamera.enabled = currentState;

            currentState = !currentState;
            playerInput.escape = false;
        }
    }

    public void CloseMenu() {
        Interface.SetActive(currentState);
        Mainmenu.SetActive(!currentState);
        Cursor.lockState = currentState ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !currentState;
        playerInput.isFocus = currentState;
        playerCamera.enabled = currentState;

        currentState = !currentState;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
