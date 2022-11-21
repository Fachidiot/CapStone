using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera aimCamera;
    [SerializeField]
    LayerMask aimColliderLayerMask = new LayerMask();

    public GameObject crossHairUI;
    public GameObject hitUI;
    public GameObject ammoCountUI;
    public GameObject maxAmmoCountUI;
    public Transform target;

    GameObject player;

    void Update()
    {
        if (!player)
        {
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playerList)
            {
                NetworkTPController networkTPController = player.GetComponent<NetworkTPController>();
                // Is Player
                if (networkTPController.IsOwner)
                {
                    this.player = player;
                    NetworkTPSController networkTPSController = player.GetComponent<NetworkTPSController>();
                    networkTPSController.SetCamera(aimCamera);
                    networkTPSController.SetAimLayer(aimColliderLayerMask);
                    networkTPSController.SetUIAll(crossHairUI, hitUI, target);
                    ActiveWeapon activeWeapon = player.GetComponent<ActiveWeapon>();
                    activeWeapon.SetUI(hitUI, ammoCountUI, maxAmmoCountUI, target);
                    followCamera.Follow = networkTPController.GetCameraRoot();
                    aimCamera.Follow = networkTPController.GetCameraRoot();
                    followCamera.gameObject.SetActive(true);
                }
                else
                {
                    GameObject client = networkTPController.gameObject;

                    Destroy(client.GetComponentInChildren<Cinemachine.CinemachineFreeLook>());
                    Destroy(client.GetComponent<CustomInput>());
                    Destroy(client.GetComponent<PlayerInput>());
                }
            }
        }
    }
}
