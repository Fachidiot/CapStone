using Unity.Netcode;
using Cinemachine;
using UnityEngine;
using TMPro;

public class PlayerHud : NetworkBehaviour
{
    [SerializeField]
    TMP_Text UI_playerName;
    [SerializeField]
    GameObject UI_canvas;

    NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();

    bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer) {
            playersName.Value = $"Player {OwnerClientId}";
            UI_playerName.text = playersName.Value;
        }
    
        GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(IsOwner);
        UI_canvas.SetActive(!IsOwner);
    }

    public void SetOverLay() {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playersName.Value;
    }
    
    // void Update()
    // {
    //     if (!overlaySet && !string.IsNullOrEmpty(playersName.Value)) {
    //         SetOverLay();
    //         overlaySet = true;
    //     }
    // }
}
