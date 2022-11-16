using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class NetworkCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera aimCamera;
    [SerializeField]
    LayerMask aimColliderLayerMask = new LayerMask();

    public GameObject crossHairUI;
    public GameObject hitUI;
    public GameObject ammoCountUI;
    public Transform target;

    GameObject player;

    void Update()
    {
        if (!player)
        {
            // 플레이어 찾고 UI필요한 컴포넌트에 다 적용 해줘야함
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playerList)
            {
                NetworkTPController networkTPController = player.GetComponent<NetworkTPController>();
                if (networkTPController.IsOwner)
                {
                    this.player = player;
                    NetworkTPSController networkTPSController = player.GetComponent<NetworkTPSController>();
                    networkTPSController.SetCamera(aimCamera);
                    networkTPSController.SetAimLayer(aimColliderLayerMask);
                    networkTPSController.SetUIAll(crossHairUI, hitUI, target);
                    ActiveWeapon activeWeapon = player.GetComponent<ActiveWeapon>();
                    activeWeapon.SetUI(hitUI, ammoCountUI, target);
                    followCamera.Follow = networkTPController.GetCameraRoot();
                    aimCamera.Follow = networkTPController.GetCameraRoot();
                    followCamera.gameObject.SetActive(true);
                }
            }
        }
    }
}
