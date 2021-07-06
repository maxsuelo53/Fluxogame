using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLobby : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private Text _listaDeJogadores;
    [SerializeField] private Button _comecaJogo;
    [SerializeField] private InputField _pontoMaximo;

    [PunRPC]
    public void AtualizaLista()
    {
        _listaDeJogadores.text = GestorDeRede.Instancia.ObterListaJogadores();
        _comecaJogo.interactable = GestorDeRede.Instancia.DonoSala();
        _pontoMaximo.interactable = GestorDeRede.Instancia.DonoSala();
    }

}
