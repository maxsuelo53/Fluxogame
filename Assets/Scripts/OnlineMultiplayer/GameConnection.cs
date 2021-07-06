using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;
public class GameConnection : MonoBehaviourPunCallbacks
{
    public Text chatLog;
    private void Awake()
    {
        chatLog.text += "\nConectando ao servidor...";
        PhotonNetwork.LocalPlayer.NickName = "MaxTeste_" + Random.Range(1, 1000);
        PhotonNetwork.ConnectUsingSettings();
    }
    //=====================================================================
    public override void OnConnectedToMaster()
    {
        //Executa comandaos que estão na função original
        base.OnConnectedToMaster();

        chatLog.text += "\nConectado ao servidor!";

        if (PhotonNetwork.InLobby == false)
        {
            chatLog.text += "\nEntrando no lobby...";
            PhotonNetwork.JoinLobby();
        }
    }
    //=====================================================================
    public override void OnJoinedLobby()
    {
        chatLog.text += "\nEntrei no Lobby!";

        chatLog.text += "\nEntrando na sala Teste...";
        PhotonNetwork.JoinRoom("Teste");
        
    }
    //=====================================================================

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        chatLog.text += "\nErro ao entrar na sala:" +message+
                        "|codigo: " +returnCode;

        if(returnCode == ErrorCode.GameDoesNotExist)
        {
            chatLog.text += "\nCriando sala Teste...";

            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 };
            PhotonNetwork.CreateRoom("Teste", roomOptions, null);
        }
    }
    //=====================================================================

    public override void OnLeftRoom()
    {
        chatLog.text += "\nVocê saiu da sala Teste!";
    }
    //=====================================================================

    public override void OnJoinedRoom()
    {
        chatLog.text += "\nVocê entrou na sala Teste! " +
                        "Seu username é" + PhotonNetwork.LocalPlayer.NickName;

        //Instanciar o player na tela
    }

    //=====================================================================
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        chatLog.text += "\nUm jogador entrou na sala Teste!"
                    + "Seu nickName é: " + newPlayer.NickName;
    }
    //=====================================================================
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        chatLog.text += "\nUm jogador entrou na sala Teste!"
                    + "Seu nickName é: " + otherPlayer.NickName;
    }
    //=====================================================================
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        chatLog.text = "\nAconteceu um erro!" + errorInfo.Info;
    }
    //=====================================================================
}
