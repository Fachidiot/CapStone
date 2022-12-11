using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverButton;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] TMP_Text playersInGameText;
    [SerializeField] TMP_InputField joinCodeInputField;

    private void Awake()
    {
        serverButton.onClick.AddListener(() => ServerButtonClick());
        hostButton.onClick.AddListener(() => HostButtonClick());
        clientButton.onClick.AddListener(() => ClientButtonClick());
    }

    void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    public void ServerButtonClick()
    {
        NetworkManager.Singleton.StartServer();
    }

    public async void HostButtonClick()
    {
        var hostData = await RelayManager.SetUpRelay(10, "production");
        Debug.Log(hostData.AllocationID);

        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetRelayServerData(hostData.IPv4Address, hostData.Port, hostData.AllocationIDByte, hostData.Key, hostData.ConnectionData);

        joinCodeInputField.text = hostData.JoinCode;

        NetworkManager.Singleton.StartHost();
    }

    public async void ClientButtonClick()
    {
        var joinData = await RelayManager.JoinRelay(joinCodeInputField.text, "production");
        Debug.Log(joinData.AllocationID);

        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetRelayServerData(joinData.IPv4Address, joinData.Port, joinData.AllocationIDByte, joinData.Key, joinData.ConnectionData, joinData.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
