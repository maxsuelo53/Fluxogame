using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerOnline : MonoBehaviourPunCallbacks
{
    [SerializeField] private MenuEntrada _menuEntrada;
    [SerializeField] private MenuLobby _menuLobby;
    bool pronto = false;

    private void Start()
    {
        _menuEntrada.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);

    }

    public override void OnConnectedToMaster()
    {
        _menuEntrada.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        
        MudaMenu(_menuLobby.gameObject);
        _menuLobby.photonView.RPC("AtualizaLista", RpcTarget.All);
    }

    public void MudaMenu(GameObject menu)
    {
        _menuEntrada.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);

        menu.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _menuLobby.AtualizaLista();

    }

    public void SairDoLobby()
    {
        GestorDeRede.Instancia.SairDoLobby();
        MudaMenu(_menuEntrada.gameObject);
    }
    
    public void ComecaJogo(string nomeCena)
    {        
        GameObject.Find("GameIconController").GetComponent<IconGameController>().DefinePontos();        
        GestorDeRede.Instancia.photonView.RPC("ComecaJogo", RpcTarget.All, nomeCena);
    }
}
