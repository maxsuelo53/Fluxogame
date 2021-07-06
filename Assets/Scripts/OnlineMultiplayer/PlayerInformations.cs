using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerInformations : MonoBehaviourPunCallbacks,IPunObservable
{
    public PhotonView varPhotonView;
    public Text textNickName;
    public Text textPointsPlayer;
    public Image playerIcon;
    private GameObject GanhadorObj;
    public Text jogadorGanhador;
    public bool acabouJogo = false;

    public int points = 0;
    public string namePlayer = "Player";


   void Start()
    {
        if (varPhotonView.IsMine)
        {
            AtualizaPontos(0);
        }           
    
        
    }

    private void Update()
    {
        if(GameObject.Find("GameIconController") != null && acabouJogo == false)
        {
            if (varPhotonView.IsMine)
            {
                if (GameObject.Find("GameIconController").GetComponent<IconGameController>().PontuacaoFinal <= points)
                {
                    varPhotonView.RPC("MostraGanhador", RpcTarget.All, textNickName.text);
                    acabouJogo = true;
                }
            }
            
        }
    }

    [PunRPC]
    public void MostraGanhador(string nome)
    {
        //GanhadorObj.SetActive(true);
        GameObject ganhador = PhotonNetwork.Instantiate("TelaGanhador", Vector3.zero, Quaternion.identity, 0);
        ganhador.transform.SetParent(GameObject.Find("Canvas").transform, false);
        ganhador.GetComponent<TelaGanhador>().setTextGanhador(nome);
    }

    [PunRPC]
    public void RPC_isPlayerOne()
    {
        transform.SetParent(GameObject.Find("LocalPlayerInformations").transform, false);
        transform.localPosition = new Vector3(-846, -442, -10);
        this.gameObject.name = "Player1 Information";
        this.transform.SetSiblingIndex(1);

    }

    [PunRPC]
    public void RPC_isPlayerTwo()
    {
        transform.SetParent(GameObject.Find("LocalPlayerInformations").transform, false);
        transform.localPosition = new Vector3(846, 412, -10);
        this.gameObject.name = "Player2 Information";
        this.transform.SetSiblingIndex(1);
        
    }

    public void viraMesa()
    {
        GameObject.Find("LocalPlayerInformations").transform.Rotate(0, 0, 180);
        //transform.Rotate(0, 0, 180);
    }

    public void AtualizaPontos(int amount)
    {        
        points += amount;
        varPhotonView.RPC("RPC_MostraPontos", RpcTarget.All, points);        
    }

    public void SetNamePlayer()
    {
        this.namePlayer = PhotonNetwork.LocalPlayer.NickName;
        varPhotonView.RPC("RPC_MostraNamePlayer", RpcTarget.All, namePlayer);
    }

    [PunRPC]
    public void RPC_MostraNamePlayer(string namePlayer)
    {
        textNickName.text = namePlayer;
    }

    [PunRPC]
    public void RPC_MostraPontos(int points)
    {
        textPointsPlayer.text = points.ToString() + " / " + GameObject.Find("GameIconController").GetComponent<IconGameController>().PontuacaoFinal.ToString();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
       
        if (stream.IsWriting)
        {
            stream.SendNext(points);
            stream.SendNext(namePlayer);
        }
        else
        {
            points = (int)stream.ReceiveNext();
            namePlayer = (string)stream.ReceiveNext();
        }
        
    }


    public void Virar()
    {
        this.gameObject.transform.Rotate(0, 0, 180);
    }
}
