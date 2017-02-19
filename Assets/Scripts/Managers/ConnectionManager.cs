﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : Photon.PunBehaviour {
    string gameVersion = "1";
    public byte MaxPlayersPerRoom = 4;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public bool offlineMode;


    private void Awake() {
        //force full log.
        PhotonNetwork.logLevel = Loglevel;

        //it must be false to list lobbies
        PhotonNetwork.autoJoinLobby = false;

        //make sure use photonNetwork on master and clients
        if(offlineMode) {
            PhotonNetwork.offlineMode = true;
        } else {
            PhotonNetwork.automaticallySyncScene = true;
            if(PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
                // Connect to the photon master-server.
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateRoom() {
        PhotonNetwork.CreateRoom("4n4n", new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        PlayerPrefs.SetInt("player", 1);

    }
    //joining same room for 2 player
    public void JoinRoom() {
        PlayerPrefs.SetInt("player", 2);

        PhotonNetwork.JoinRoom("4n4n");
    }

    //------------------------------------------------------------------------------------------
    public override void OnConnectedToMaster() {

        Debug.Log("OnConnectedToMaster() was called by PUN | Ping: " + PhotonNetwork.GetPing());

    }

    public override void OnDisconnectedFromPhoton() {


        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        JoinRoom();
    }
    public override void OnJoinedRoom() {
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        Debug.Log("We load the 'Room for 1' ");


        // #Critical
        // Load the Room Level. 
        PhotonNetwork.LoadLevel("CharacterSelect");

    }
    public override void OnCreatedRoom() {
        //Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel("CharacterSelect");
    }
}