using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GestorDeRede : MonoBehaviourPunCallbacks
{
    public static GestorDeRede Instancia { get; private set; }


    //INSTANCIA O OBJETO DE CONEXÃO
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
        //
    }

    //CONECTA NO SERVIDOR USANDO A CONEXÃO CONFIGURADA
    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //RETORNA A RESPOSTA QUE A CONEXÃO COM O SERVIDOR
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexão bem sucedida!");
    }

    //CRIA UMA SALA
    public void CriaSala(string nomeSala)
    {
        PhotonNetwork.CreateRoom(nomeSala, new RoomOptions{ IsVisible = true, IsOpen = true, MaxPlayers = 2 });
    }

    //ENTRAR EM UMA SALA
    public void EntraSala(string nomeSala)
    {
        PhotonNetwork.JoinRoom(nomeSala);
    }

    //ATRIBUIR O NOME PARA O PERSONAGEM
    public void MudaNick( string nickname)
    {
        PhotonNetwork.NickName = nickname;
    } 

    //CRIA LISTA DE JOGADORES PARA MOSTRAR NO LOBBY
    public string ObterListaJogadores()
    {
        var lista = "";

        foreach(var player in PhotonNetwork.PlayerList)
        {
            lista += player.NickName + "\n";
        }

        return lista;
    }

    //IDENTIFICA O DONO DE SALA E SOMENTE ELE PODE INICIAR O JOGO
    public bool DonoSala()
    {
        return PhotonNetwork.IsMasterClient;
    }

    //SAIR DO LOBBY
    public void SairDoLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    //DESTROI A CONEXÃO ANTIGA NO SERVIDOR E CRIA UMA NOVA CONEXÃO
    public void DestruindoGameConnection()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    //CHAMADA DA CENA DE CRIAÇÃO DO JOGO
    [PunRPC]
    public void ComecaJogo(string nomeCena)
    {
        PhotonNetwork.LoadLevel(nomeCena);
    }

    public void ButtonRetornaMenu()
    {
        Destroy(GameObject.Find("GameIconController").gameObject);
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MenuScene");
    }

}
