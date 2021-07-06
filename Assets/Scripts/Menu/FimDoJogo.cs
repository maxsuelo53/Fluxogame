using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FimDoJogo : MonoBehaviourPunCallbacks
{
    public GameObject GameConnectionObj;


    private void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SairDoJogo);
    }
    public void SairDoJogo()
    {
        
        GameObject.Find("GameIconController").GetComponent<IconGameController>().DestruindoObjeto();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        
        PhotonNetwork.LoadLevel("MenuMultiplayer"); 
        GestorDeRede.Instancia.DestruindoGameConnection();

    }
}
