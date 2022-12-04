using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerMakeData {
    public GameObject playerPrefab;

}

public class PlayerMaker : NetworkBehaviour
{
    [Header("Network")]
    [SerializeField] bool Owner;
    [SerializeField] NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] Canvas playerMakerUI;

    void Start()
    {
        if (!Owner)
            playerMakerUI.gameObject.SetActive(false);
        else {
            playerMakerUI.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
    
    public void PlayerMake(PlayerMakeData playerData) {
        var player = Instantiate(playerData.playerPrefab);
        if (!Owner) {
            
        }
    }
}
