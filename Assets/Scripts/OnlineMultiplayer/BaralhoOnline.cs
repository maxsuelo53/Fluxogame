using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BaralhoOnline : MonoBehaviourPunCallbacks
{
    public PhotonView varPhotonView;
    public GameObject TelaJogo; //TELA DO JOGO
    //============================================================
    //VARIAVEIS DO BARALHO
    public List<GameObject> listCardBaralho = new List<GameObject>(7);
    public GameObject localBaralho; // LOCAL ONDE SERÁ GERADO O BARALHO
    public GameObject buttonComprarCartaVirada;//LOCAL ONDE É GERADA A CARTA VIRADA
    private List<GameObject> listBaralhoOnline = new List<GameObject>();
    //============================================================
    private void Awake()
    {
        if( (PhotonNetwork.IsMasterClient == true) && (varPhotonView.IsMine == true))
        {
            CriandoBaralho();
            varPhotonView.RPC("CriandoBaralhoOnline", RpcTarget.All);
        }
        
    }

    private void Update()
    {

    }

    public void CriandoBaralho()
    {
        
        for (int i=0; i<7; i++ )
        {
            for (int j=0; j<8; j++)
            {
              GameObject cartaObj =  PhotonNetwork.Instantiate(listCardBaralho[i].name, localBaralho.transform.position, Quaternion.identity);
              listBaralhoOnline.Add(cartaObj);

            }
        }       

    }

    [PunRPC]
    public void CriandoBaralhoOnline()
    {
        for(int i=0; i<listBaralhoOnline.Count-1; i++)
        {
            listBaralhoOnline[i].transform.SetParent(localBaralho.transform, false);
            listBaralhoOnline[i].transform.position = localBaralho.transform.position;
        }

    }
}
