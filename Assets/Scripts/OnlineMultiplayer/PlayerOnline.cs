using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOnline : MonoBehaviourPunCallbacks
{
    [Header("Atributos")]
    public Text nomePlayer;
    public Text pontoPlayer;
    public GameObject PlayerCamera;
    public PhotonView varPhotonView;

    [Header("Prefabs")]
    public List <GameObject> listCartasDoJogo = new List<GameObject>();
    public GameObject menuActions;
    GameObject objectForDestroy;
    public List<GameObject> listaAlgoritmo;
    public GameObject Lixeira;
    public List<GameObject> CartasHandPlayer = new List<GameObject>();

    [Header("Turn Animations")]
    public GameObject turnPlayerAnimation;
    public GameObject turnEnemyAnimation;

    private GameObject CartaForPlayer;
    public bool verificaInfoPlayer;



    //MOSTRA AS INFORMAÇÕES DO JOGADOR NO INICIO DO JOGO
    void Awake()
    {
        if (varPhotonView.IsMine)
        {
            PlayerCamera.SetActive(true);
            verificaInfoPlayer = true;
        }
        
    }

    //NO INICIO DESATIVA AS FUNÇÕES DO OUTRO JOGADOR PARA ESTE PLAYER
    private void Start()
    {
        if (!varPhotonView.IsMine)
        {
            this.gameObject.GetComponent<DropZone>().enabled = false;
            return;
        }

        if (this.gameObject.name == "Player2")
        {
            viraMesa();
        }
        StartCoroutine(SetCardsHandPlayer(3));
    }

    //VERIFICA A TODO MOMENTO O NÚMERO DE CARTAS DO JOGADOR 
    private void Update()
    {
        if (varPhotonView.IsMine)
        {
            SetFlipCartasPlayer();

            if(this.gameObject.name == "Player2" && GameObject.Find("LocalPlayerInformations").transform.childCount == 2 && verificaInfoPlayer == true)
            {
                GameObject.Find("LocalPlayerInformations").transform.transform.Rotate(0, 0, 180);
                for (int i=0; i< GameObject.Find("LocalPlayerInformations").transform.childCount; i++)
                {
                    GameObject.Find("LocalPlayerInformations").transform.GetChild(i).transform.Rotate(0, 0, 180);
                    
                }
                verificaInfoPlayer = false;
            }
        }
        else
        {
            for (int i=0;i< this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<HoverInformation>().enabled = false;
            }
            
        }
        
    }

    //VIRA A CARTA DO JOGADOR
    private void SetFlipCartasPlayer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cartaForFlip = transform.GetChild(i).gameObject;
            
            if (cartaForFlip.name != "New Game Object")
            {
                cartaForFlip.GetComponent<Carta>().cartaVirada = true;
                cartaForFlip.GetComponent<Carta>().VirarCarta();
            }
                
        }
    }

    //ANIMAÇÃO DE ENVIO DAS CARTAS PARA A MÃO DO JOGADOR
    public IEnumerator SetCardsHandPlayer(int qtdCartas)
    {
        yield return new WaitForSeconds(0.16f);
        for (int i = 0; i < qtdCartas; i++)
        {
            var carta = PhotonNetwork.Instantiate(this.listCartasDoJogo[UnityEngine.Random.Range(0,7)].name, Vector3.zero, Quaternion.identity, 0);
            carta.GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, this.gameObject.name);                     
            carta.gameObject.AddComponent<DragDrop>();
        }
    }

    //PEGA CARTA PARA A MÃO DO PLAYER
    public void TakingCard(string localInicial, int qtdCartas,string cartaVirada)
    {        
        var carta = PhotonNetwork.Instantiate(this.listCartasDoJogo[UnityEngine.Random.Range(0, 7)].name, GameObject.Find(localInicial).transform.localPosition, Quaternion.identity, 0);
        carta.GetComponent<PhotonView>().RPC("RPC_AnimationTakeCard", RpcTarget.All, this.gameObject.name);

        if(localInicial == "ButtonComprarCartaViradaBaralho")
        {
            StartCoroutine(ComprandoCartaVirada(cartaVirada));
        }
        else if (localInicial == "ButtonComprarCartaBaralho")
        {
            StartCoroutine(SetCardsHandPlayer(qtdCartas));
        }

    }

    //DEFINE O OBJETO DO PLAYER
    [PunRPC]
    public void RPC_isPlayerOne()
    {
        transform.SetParent(GameObject.Find("LocalPlayer").transform,false);
        transform.localPosition = new Vector3(0, -410, -10);
        this.gameObject.name = "Player1";
    }

    //DEFINE O OBJETO DO PLAYER 2
    [PunRPC]
    public void RPC_isPlayerTwo()
    {
        transform.SetParent(GameObject.Find("LocalPlayer").transform, false);
        transform.localPosition = new Vector3(0, 410, -10);
        this.gameObject.name = "Player2";
        
    }

    //O JOGADOR É VIRADO PARA O OUTRO LADO DA MESA SE ELE FOR O PLAYER 2
    public void viraMesa()
    {
        GameObject.Find("LocalPlayer").transform.Rotate(0, 0, 180);
        transform.Rotate(0, 0, 180);
    }

    //ANIMAÇÃO DA CARTA SENDO COMPRADA
    public IEnumerator ComprandoCartaVirada(string NomeCartaViradaComprada)
    {
        yield return new WaitForSeconds(0.16f);
        string[] subs = NomeCartaViradaComprada.Split('(');
        var carta = PhotonNetwork.Instantiate(subs[0], Vector3.zero, Quaternion.identity, 0);
        carta.GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, this.gameObject.name);
        carta.gameObject.AddComponent<DragDrop>();
    }

    //ANIMAÇÃO DO TURNO DO ADVERSÁRIO
    public void ChangeTurnForEnemy()
    {
        Instantiate(turnEnemyAnimation, GameObject.Find("Canvas").transform, false);
    }

    //ANIMAÇÃO DO TURNO DO JOGADOR
    public void ChangeTurnForPlayer()
    {
        Instantiate(turnPlayerAnimation, GameObject.Find("Canvas").transform, false);
    }

    //INSERE O SCRIPT NAS CARTAS DA MAO DO JOGADOR PARA SER ADICIONADOS NA LISTA DE MONTAGEM DO ALGORITMO
    public void ActionConfereAlgoritmo()
    {
        for(int i=0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<SelecionarCartaAlgoritmo>();
        }

    }

    //ENVIA CARTA DA MÃO DO JOGADOR PARA O CENTRO DA MESA PARA VERIFICAÇÃO DO ALGORITMO
    public void EnviandoCartasParaCentroAlgoritmo()
    {
        listaAlgoritmo = GameObject.Find("LocalAlgoritmo").GetComponent<CriarAlgoritmoDeResposta>().EnviaListaDeCartasAlgoritmoMontado();

        for (int i = 0; i < listaAlgoritmo.Count; i++)
        {
            listaAlgoritmo[i].GetComponent<DragDrop>().enabled = false;
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_AtivaPhotonTransformView", RpcTarget.All);
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, "LocalAlgoritmo");
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_SetFlipCardForAll", RpcTarget.All);
            listaAlgoritmo[i].GetComponent<SelecionarCartaAlgoritmo>().Indice.GetComponent<IndiceCardControl>().DestruindoIndiceCard();
            var scriptSelecionarCartaAlgoritmo = listaAlgoritmo[i].GetComponent<SelecionarCartaAlgoritmo>();
            Destroy(scriptSelecionarCartaAlgoritmo);
            var outlineComponent =  listaAlgoritmo[i].GetComponent<Outline>();
            Destroy(outlineComponent);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            var scriptSelecionarCartaAlgoritmo =transform.GetChild(i).GetComponent<SelecionarCartaAlgoritmo>();
            Destroy(scriptSelecionarCartaAlgoritmo);
        }
        GameObject.Find("LocalAlgoritmo").GetComponent<CriarAlgoritmoDeResposta>().LimparLista();
    }

    //GERA A ANIMAÇÃO DOS PONTOS CORRETOS
    public void MostraPontosCorretos(int pontos)
    {
        GameObject pointAnimation = PhotonNetwork.Instantiate("AnimationPointsAll", Vector3.zero, Quaternion.identity);
        pointAnimation.GetComponent<TextMeshProUGUI>().text = pointAnimation.GetComponent<TextMeshProUGUI>().text + "\n" +pontos.ToString();
        pointAnimation.GetComponent<PhotonView>().RPC("RPC_MostrarParaTodos", RpcTarget.All);
    }

    //GERA A ANIMAÇÃO DOS PONTOS ERRADOS
    public void MostraPontosErrados()
    {
        GameObject pointAnimation = PhotonNetwork.Instantiate("AnimationWrongAlgoritmo", Vector3.zero, Quaternion.identity);
        pointAnimation.GetComponent<PhotonView>().RPC("RPC_MostrarParaTodos", RpcTarget.All);
    }

    //DEVOLVE AS CARTAS PARA O JOGO CASO O ALGORITMO ESTEJA ERRADO
    public void RecebeCartasDoAlgoritmoErrado()
    {
        GameObject localListAlgoritmo = GameObject.Find("LocalAlgoritmo");
        var tam = localListAlgoritmo.transform.childCount;
        
        for(int i=0;i<tam; i++)
        {            
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, this.gameObject.name);            
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_SetEscondeCartaForAll", RpcTarget.All);
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_DesativaPhotonTransformView", RpcTarget.All);
            listaAlgoritmo[i].GetComponent<DragDrop>().enabled = true;

        }
    }

    //DESTROI AS CARTAS DO JOGADOR CASO O SEU ALGORITMO ESTEJA CORRETO
    public void DestruindoCartasDoAlgoritmoCerto()
    {
        GameObject localListAlgoritmo = GameObject.Find("LocalAlgoritmo");
        var tam = localListAlgoritmo.transform.childCount;

        for (int i = 0; i < tam; i++)
        {
            listaAlgoritmo[i].GetComponent<PhotonView>().RPC("RPC_DestuirEssaCarta", RpcTarget.All);

        }
    }
}

