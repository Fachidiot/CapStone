using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class RelayManager : MonoBehaviour
{
    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDByte;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    public struct RelayJoinData
    {
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDByte;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }

    public static async Task<RelayHostData> SetUpRelay(int maxConn, string environment)
    {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);    // Service options

        await UnityServices.InitializeAsync();

        // Automatically Anonymously login
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Relay Server's session Create&Setting
        Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

        RelayHostData hostData = new RelayHostData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDByte = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key,
            JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId)
        };

        // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(hostData.IPv4Address, hostData.Port, hostData.AllocationIDByte, hostData.Key, hostData.ConnectionData);
        // NetworkManager.Singleton.StartHost();

        return hostData;
    }

    public static async Task<RelayJoinData> JoinRelay(string joinCode, string environment)
    {
        Debug.Log($"Start Join by {joinCode}");

        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);    // Service options

        await UnityServices.InitializeAsync();

        // Automatically Anonymously login
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        JoinAllocation allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData joinData = new RelayJoinData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDByte = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            Key = allocation.Key
        };

        // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(joinData.IPv4Address, joinData.Port, joinData.AllocationIDByte, joinData.Key, joinData.ConnectionData);
        // NetworkManager.Singleton.StartClient();

        return joinData;
    }
}
