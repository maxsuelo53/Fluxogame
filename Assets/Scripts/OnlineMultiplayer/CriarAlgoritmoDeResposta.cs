using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class CriarAlgoritmoDeResposta : MonoBehaviour
{
    public GameObject handPlayer;
    public List<GameObject> listaAlgoritmo;
    public PhotonView varPhotonView;

    //LIMPA A LISTA DAS CARTAS
    public void LimparLista()
    {
        listaAlgoritmo = new List<GameObject>();
    }
    //ADICIONA AS CARTAS SELECIONADAS NA LISTA PARA ENVIAR PARA O CENTRO DA MESA
    public void MontandoAlgoritmo(GameObject cartaAtual, string acao)
    {
        if (acao.Equals("add"))
        {
            listaAlgoritmo.Add(cartaAtual);
            Debug.Log("LISTA:" + listaAlgoritmo.Count);
        }
        else
        {
            listaAlgoritmo.Remove(cartaAtual);
            Debug.Log("LISTA:" + listaAlgoritmo.Count);
        }

    }

    //RETORNA INDICE DA LISTA
    public int GetIndiceAtualLista(GameObject estaCartaSelecionada)
    {
        return (listaAlgoritmo.IndexOf(estaCartaSelecionada) + 1);
    }
    

    public List<GameObject> EnviaListaDeCartasAlgoritmoMontado()
    {
        return listaAlgoritmo;
    }
}
