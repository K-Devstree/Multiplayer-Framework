using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
public class NetworkManagerMMO : NetworkManager
{
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;



    public void OnClickConnectButton()
    {
        Debug.Log("##### OnClickConnectButton!");
        discoveredServers.Clear();
        StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void StartNetworkDiscovery()
    {
        Debug.Log("##### StartNetworkDiscovery!");
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

   

    public override void OnStartServer()
    {
        Debug.Log("##### OnStartServer!");
        base.OnStartServer();
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        Debug.Log("##### OnServerDisconnect!");
        base.OnServerDisconnect(connection);
     
        if (NetworkServer.localClientActive)
        {
            Debug.Log("@@@@@ NetworkServer.localClientActive");
        }
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        Debug.Log("##### OnClientDisconnect!");
        base.OnClientDisconnect(connection);
    }

    public override void OnStopHost()
    {
        Debug.Log("##### OnStopHost!");
        base.OnStopHost();
    }

    public override void OnStopClient()
    {
        Debug.Log("##### OnStopClient!");
        base.OnStopClient();
    }

    void Update()
    {
        if (NetworkManager.singleton == null)
            return;

        if (NetworkServer.active || NetworkClient.active)
            return;

        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
        {
            print("Discoverd Network : " + discoveredServers.Count);
        }
            
    }


   
   

  

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
        //foreach (ServerResponse info in discoveredServers.Values)
        //{
        //    print("Found Server : " + info.EndPoint.Address.ToString());
        //}
    }

}
