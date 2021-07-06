using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconGameController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int PontuacaoFinal = 0;
    public GameObject escolhePontos;
    public GameObject Pontos;
    public InputField pontosJogo;
    public Button _comecarJogo;
    public GameObject mostraPontos;

    public PhotonView varphotonView;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Pontos != null)
        {
            if (Pontos.GetComponent<Text>().text == "" && (_comecarJogo != null))
            {
                _comecarJogo.interactable = false;
            }
            else if (PhotonNetwork.LocalPlayer.IsMasterClient && (_comecarJogo != null) && PhotonNetwork.PlayerList.Length == 2)
            {
                _comecarJogo.interactable = true;
            }
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PontuacaoFinal);
        }
        else if (stream.IsReading)
        {
            PontuacaoFinal = (int) stream.ReceiveNext();
        }
    }

    public bool DefinePontos()
    {
        PontuacaoFinal = int.Parse(Pontos.GetComponent<Text>().text);
        varphotonView.RPC("RPC_setPoints", RpcTarget.All,PontuacaoFinal);
        return true;
    }

    [PunRPC]
    public void RPC_setPoints(int pontos)
    {
        PontuacaoFinal = pontos;
    }

    public void DestruindoObjeto()
    {
        PhotonNetwork.Destroy(varphotonView);
    }

}
