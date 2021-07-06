using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Carta : MonoBehaviourPunCallbacks
{
    public string atributo;
    public GameObject cartaAtras;
    public bool cartaVirada;
    public string type;

    public PhotonView varPhotonView;

    public void VirarCarta()
    {
        if (cartaVirada)
        {
            cartaAtras.SetActive(false);
        }
        else
        {
            cartaAtras.SetActive(true);
        }

    }

    [PunRPC]
    public void RPC_isCartaViradaDoBaralhoParaCompra()
    {
        GameObject localGenerate = GameObject.Find("LocalCartaBaralhoVirada");
        transform.SetParent(GameObject.Find("Canvas").transform,false);
        transform.position = localGenerate.transform.position;
        transform.SetSiblingIndex(1);
        transform.GetChild(0).gameObject.SetActive(false);
    }


    [PunRPC]
    public void RPC_DestruindoCarta()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void RPC_CartaFromAlgoritmo()
    {
        transform.SetParent(GameObject.Find("LocalAlgoritmo").transform, false);
        cartaVirada = true;
        VirarCarta();
    }

    [PunRPC]
    public void RPC_SetCardLocal(string local)
    {
        this.gameObject.transform.SetParent(GameObject.Find(local).transform,false);
        if (local == "Lixeira")
        {
            this.transform.SetAsLastSibling();
        }
    }

    [PunRPC]
    public void RPC_SetFlipCardForAll()
    {
        cartaVirada = false;
        cartaAtras.SetActive(false);
    }


    [PunRPC]
    public void RPC_AtivaPhotonTransformView()
    {
        this.gameObject.AddComponent<PhotonTransformView>();
        this.gameObject.GetComponent<PhotonTransformView>().enabled = true;
    }

    [PunRPC]
    public void RPC_SetEscondeCartaForAll()
    {
        cartaVirada = true;
        cartaAtras.SetActive(true);
    }

    [PunRPC]
    public void RPC_DesativaPhotonTransformView()
    {
        var varPhotonTrasnform = this.gameObject.GetComponent<PhotonTransformView>();
        Destroy (varPhotonTrasnform);
    }

    [PunRPC]
    public void RPC_DestuirEssaCarta()
    {
        var thisPhotonView = this.gameObject.GetComponent<PhotonView>();
        PhotonNetwork.Destroy(thisPhotonView);
    }

    [PunRPC]
    public void RPC_AnimationTakeCard(string posicaoFinal)
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
        LeanTween.move(this.gameObject, GameObject.Find(posicaoFinal).transform.position, 0.16f);
        StartCoroutine(RemoveLeanTween());
        
    }

    public IEnumerator RemoveLeanTween()
    {
        yield return new WaitForSeconds(0.155f);
        Destroy(this.gameObject);
    }
}
