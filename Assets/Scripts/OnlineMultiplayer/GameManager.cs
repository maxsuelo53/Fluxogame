using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum TurnState { PLAYERTURN, ENEMYTURN}
    public PhotonView varPhotonView;

    public IconGameController IconController;

    [Header("Prefabs Generate")]
    public GameObject PlayerPrefab;
    public GameObject PlayerInformationPrefab;
    public List<GameObject> CartasDoBaralho;
    public GameObject Player;
    public GameObject PlayerInformation;
    public GameObject menuAction;
    public GameObject localAlgoritmo;
    public GameObject localCartaVirada;
    public GameObject PrefabAlertMessage;

    [Header("Prefabs Generate")]
    public GameObject buttonComprarAlgoritmo;
    public GameObject buttonComprarCartaDoMonte;
    public GameObject buttonComprarCartaVirada;
    public GameObject viewObjetivo;
    public GameObject listObjetivos;
    public GameObject CartaObjetivoEscolhida;
    public GameObject buttonVerificarAlgoritmoPronto;
    public GameObject Lixeira;
    public GameObject buttonEndTurn;
    public GameObject menuShow;
    public GameObject listaAlgoritmosMao;

    [Header("Buttons Actions")]
    public GameObject MenuButtonComprarCartas;
    public GameObject MenuButtonResolverAlgoritmo;

    private int turns = 1;
    TurnState turnPlayer;

    private string CartaGeradaParaPlayersComprar;
    private bool GerarNovaCarta = false;



    private void Awake()
    {
        CriaPlayer();
        StartingGame();
        //TurnControl();
        //SetImagensForIconsPlayer();
        DefineTurno();
        

    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(localCartaVirada.transform.childCount <= 0 || GerarNovaCarta == true)
            {
                GeraCartaViradaBaralho();
            }
        }
        
        if(Player != null)
        {
            if ( (Player.GetComponent<PhotonView>().IsMine == true) ){
                
                if (Player.transform.childCount > 10)
                {
                    string textAlert = "Você ultrapassou o limite de 10 cartas...Descarte alguma carta até atingir 10 cartas!!";
                    buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
                    buttonEndTurn.GetComponent<Button>().onClick.AddListener(delegate { SendMessageAlert(textAlert); });
                    Lixeira.GetComponent<DropZone>().enabled = true;
                }

                if(listaAlgoritmosMao.transform.childCount <= 0)
                {
                    MenuButtonComprarCartas.SetActive(false);
                    MenuButtonResolverAlgoritmo.SetActive(false);
                    if (CartaObjetivoEscolhida.transform.childCount > 0)
                    {
                        Destroy(CartaObjetivoEscolhida.transform.GetChild(0).gameObject);
                    }
                        
                }
                else if (listaAlgoritmosMao.transform.childCount > 0)
                {
                    MenuButtonComprarCartas.SetActive(true);
                    MenuButtonResolverAlgoritmo.SetActive(true);
                }
                
            }
        }

        
        
    }

    


    /// <summary>
    /// INICIO BLOCO DE CONFIGURAÇÃO INICIAL DO JOGO
    /// </summary>

    //CRIA OS PLAYERS DO JOGO E COLOCA ELES DE FRENTE UM PARA O OUTRO
    public void CriaPlayer()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Player = PhotonNetwork.Instantiate(this.PlayerPrefab.name, Vector3.zero, Quaternion.identity, 0);
            Player.GetComponent<PhotonView>().RPC("RPC_isPlayerOne", RpcTarget.All);
            PlayerInformation = PhotonNetwork.Instantiate(this.PlayerInformationPrefab.name, Vector3.zero, Quaternion.identity, 0);
            PlayerInformation.GetComponent<PhotonView>().RPC("RPC_isPlayerOne", RpcTarget.All);
                
        }
        else
        {
            Player = PhotonNetwork.Instantiate(this.PlayerPrefab.name, Vector3.zero, Quaternion.identity, 0);
            Player.GetComponent<PhotonView>().RPC("RPC_isPlayerTwo", RpcTarget.All);
            PlayerInformation = PhotonNetwork.Instantiate(this.PlayerInformationPrefab.name, Vector3.zero, Quaternion.identity, 0);
            PlayerInformation.GetComponent<PhotonView>().RPC("RPC_isPlayerTwo", RpcTarget.All);
        }
    }

    public void StartingGame()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {            
            buttonComprarCartaDoMonte.GetComponent<Button>().enabled = false;
            buttonComprarCartaVirada.GetComponent<Button>().enabled = false;
            buttonVerificarAlgoritmoPronto.SetActive(false);
            buttonEndTurn.SetActive(false);
            menuAction.SetActive(false);
            Lixeira.GetComponent<DropZone>().enabled = false;
            viewObjetivo.SetActive(false);
            localAlgoritmo.GetComponent<Button>().enabled = false;
            RecebeAlgoritmos(3);            
            PlayerInformation.GetComponent<PlayerInformations>().SetNamePlayer();
        }
    }

    public void SetImagensForIconsPlayer()
    {
        IconController = GameObject.Find("GameIconController").GetComponent<IconGameController>();
        if (PhotonNetwork.IsMasterClient)
        {            
            PlayerInformation.GetComponent<PhotonView>().RPC("RPC_SetImagePlayerIconMaster", RpcTarget.All);
        }
        else
        {
            PlayerInformation.GetComponent<PhotonView>().RPC("RPC_SetImagePlayerIconNotMaster", RpcTarget.All);
        }
        
    }

    private void DesativaActions()
    {
        if (Player.transform.GetComponent<PhotonView>().IsMine == true)
        {
            buttonComprarCartaDoMonte.GetComponent<Button>().enabled = false;
            buttonComprarCartaVirada.GetComponent<Button>().enabled = false;
            localAlgoritmo.GetComponent<Button>().enabled = false;
            Lixeira.GetComponent<DropZone>().enabled = false;
            menuAction.SetActive(false);
            buttonEndTurn.SetActive(false);
        }
    }

    public void VerificaNumCartaHandPlayer()
    {
        if ((Player.GetComponent<PhotonView>().IsMine == true))
        {
            if (Player.transform.childCount <= 10)
            {
                buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
                buttonEndTurn.GetComponent<Button>().onClick.AddListener(this.GetComponent<GameManager>().buttonChangeTurn );
                Lixeira.GetComponent<DropZone>().enabled = false;
            }
        }
    }

    public void DefineTurno()
    {
        if((Player.GetComponent<PhotonView>().IsMine == true && PhotonNetwork.LocalPlayer.IsMasterClient == true))
        {
            turnPlayer = TurnState.PLAYERTURN;
            TurnControl();
        }else if ((Player.GetComponent<PhotonView>().IsMine == true && PhotonNetwork.LocalPlayer.IsMasterClient == false))
        {
            turnPlayer = TurnState.ENEMYTURN;
            DesativaActions();
        }
        TurnControl();
    }
    /// <summary>
    /// FIM BLOCO DE CONFIGURAÇÃO INICIAL DO JOGO
    /// </summary>
    /// 

    /// <summary>
    /// INICIO BLOCO DE CONFIGURAÇÃO DE MENU
    /// </summary>
    /// 

    public void ConfigurationPressed()
    {
        menuShow.SetActive(true);
    }

    public void HideMenuSairjogo()
    {
        menuShow.SetActive(false);
    }


    /// <summary>
    /// FIM BLOCO DE CONFIGURAÇÃO DE MENU
    /// </summary>
    /// 







    /// <summary>
    /// INICIO BLOCO ACTION COMPRAR CARTAS
    /// </summary>
    /// 

    //ACTION COMPRAR CARTA DO BARALHO
    public void ButtonComprarCartas()
    {
        if (Player.transform.GetComponent<PhotonView>().IsMine)
        {
            menuAction.SetActive(false);
            buttonComprarCartaDoMonte.GetComponent<Button>().enabled = true;
            buttonComprarCartaVirada.GetComponent<Button>().enabled = true;

        }
    }

    //COMPRAR DUAS CARTAS DO BARALHO ALEATÓRIAS
    public void ComprarCartaBaralho()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            Player.GetComponent<PlayerOnline>().TakingCard(buttonComprarCartaDoMonte.name,2,"");
            //Player.GetComponent<PlayerOnline>().SetCardsHandPlayer(2);
            buttonComprarCartaDoMonte.GetComponent<Button>().enabled = false;
            buttonComprarCartaVirada.GetComponent<Button>().enabled = false;

            buttonEndTurn.SetActive(true);
            buttonEndTurn.GetComponent<Button>().onClick.AddListener(this.gameObject.GetComponent<GameManager>().buttonChangeTurn);

            /*
            string textAlert = "Você deve descartar uma carta antes de executar essa ação!!!";
            buttonEndTurn.SetActive(true);
            buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
            buttonEndTurn.GetComponent<Button>().onClick.AddListener(delegate { SendMessageAlert(textAlert); });

            Lixeira.GetComponent<DropZone>().enabled = true;
            */
        }
    }

    //GERA CARTA VIRADA PARA COMPRA
    public void GeraCartaViradaBaralho()
    {
        if(localCartaVirada.transform.childCount >= 1)
        {
            PhotonNetwork.Destroy(localCartaVirada.transform.GetChild(0).GetComponent<PhotonView>());
        }

        int indiceSort = UnityEngine.Random.Range(0, 7);
        GameObject cartaSorteadaParaSerGerada = PhotonNetwork.Instantiate(CartasDoBaralho[indiceSort].name, Vector3.zero,Quaternion.identity);
        cartaSorteadaParaSerGerada.GetComponent<PhotonView>().RPC("RPC_SetCardLocal", RpcTarget.All, localCartaVirada.name);
        cartaSorteadaParaSerGerada.GetComponent<PhotonView>().RPC("RPC_SetFlipCardForAll", RpcTarget.All);

        CartaGeradaParaPlayersComprar = CartasDoBaralho[indiceSort].name;
        GerarNovaCarta = false;
    }

    //COMPRA UMA CARTA VIRADA 
    public void ComprarCartaVirada()
    {

        PhotonView pV = Player.transform.GetComponent<PhotonView>();

        //SE FOR O PLAYER QUE ACIONOU A AÇÃO ELE COMPRA A CARTA
        if (pV.IsMine)
        {
            Player.GetComponent<PlayerOnline>().TakingCard(buttonComprarCartaVirada.name, 1, CartaGeradaParaPlayersComprar);
            //Player.GetComponent<PlayerOnline>().ComprandoCartaVirada();
            buttonComprarCartaDoMonte.GetComponent<Button>().enabled = false;
            buttonComprarCartaVirada.GetComponent<Button>().enabled = false;
            buttonEndTurn.SetActive(true);
            varPhotonView.RPC("RPC_SetGerarNovaCarta",RpcTarget.All);

            buttonEndTurn.SetActive(true);
            buttonEndTurn.GetComponent<Button>().onClick.AddListener( this.gameObject.GetComponent<GameManager>().buttonChangeTurn );

            /*
            string textAlert = "Você deve descartar uma carta antes de executar essa ação!!!";
            buttonEndTurn.SetActive(true);
            buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
            buttonEndTurn.GetComponent<Button>().onClick.AddListener(delegate { SendMessageAlert(textAlert); });
            Lixeira.GetComponent<DropZone>().enabled = true;
            */
        }
    }

    [PunRPC]
    public void RPC_SetGerarNovaCarta()
    {
        GerarNovaCarta = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CartaGeradaParaPlayersComprar);
            stream.SendNext(GerarNovaCarta);
            stream.SendNext(turns);
        }
        else if (stream.IsReading)
        {
            CartaGeradaParaPlayersComprar = (string)stream.ReceiveNext();
            GerarNovaCarta = (bool)stream.ReceiveNext();
            turns = (Int32)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// FIM BLOCO ACTION COMPRAR CARTAS
    /// </summary>
    /// 

    //==============================================================================


    /// <summary>
    /// CONTROLANDO OS TURNOS
    /// </summary>
    /// 
    public void TurnControl()
    {
        /*
        if (turns == 1)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player.GetComponent<PlayerOnline>().ChangeTurnForPlayer();
                StartCoroutine(ShowMenuActions());
            }
            else
            {
                Player.GetComponent<PlayerOnline>().ChangeTurnForEnemy();
            }

        }
        else if (turns == 2)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player.GetComponent<PlayerOnline>().ChangeTurnForEnemy();

            }
            else
            {
                Player.GetComponent<PlayerOnline>().ChangeTurnForPlayer();
                StartCoroutine(ShowMenuActions());
            }

        }
        */
        if(turnPlayer == TurnState.PLAYERTURN)
        {
            StartCoroutine(ShowMenuActions());
            Player.GetComponent<PlayerOnline>().ChangeTurnForPlayer();
        }

    }

    public IEnumerator ShowMenuActions()
    {        
        yield return new WaitForSeconds(0.5f);
        menuAction.SetActive(true);
    }

    public void buttonChangeTurn()
    {
        if (Player.transform.GetComponent<PhotonView>().IsMine)
        {            
            varPhotonView.RPC("RPC_ChangeTurn", RpcTarget.Others);
           // Debug.Log("turns" +turns);
           Lixeira.GetComponent<DropZone>().enabled = false;
           DesativaActions();

            turnPlayer = TurnState.ENEMYTURN;
            Player.GetComponent<PlayerOnline>().ChangeTurnForEnemy();
        }

        Debug.Log("turnPlayer" + turnPlayer);

    }

    [PunRPC]
    public void RPC_ChangeTurn()
    {
        turnPlayer = TurnState.PLAYERTURN;
        Debug.Log("turnPlayer" + turnPlayer);
        /*
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (turns == 1)
            {
                turns = 2;
            }
            else if (turns == 2)
            {
                turns = 1;
            }

        }
        */
        TurnControl();
    }

    public void SendMessageAlert(string textMessage)
    {
        GameObject boxAlert = Instantiate(PrefabAlertMessage, GameObject.Find("Canvas").transform, false);
        boxAlert.GetComponent<AlertMessage>().MessageText.text = textMessage;

    }


    /// <summary>
    /// FIM BLOCO ACTION COMPRAR CARTAS
    /// </summary>
    /// 

    //==============================================================================


    /// <summary>
    /// INICIO BLOCO DE ALGORITMOS 
    /// </summary>
    /// 

    //RECEBENDO ALGORITMO
    public void RecebeAlgoritmos(int qtd )
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            List<int> vectorNumSort = new List<int>();
            GameObject CartaParaObjetivo;
            int numSorteado = 0;
            int verificaCartaSorteada = 0;
            for (int i = 0; i < qtd; i++)
            {
                verificaCartaSorteada = 1;
                while (verificaCartaSorteada == 1)
                {
                    verificaCartaSorteada = 0;
                    numSorteado = UnityEngine.Random.Range(13, 28);
                    for (int j = 0; j < vectorNumSort.Count; j++)
                    {
                        if (numSorteado == vectorNumSort[j])
                        {
                            verificaCartaSorteada = 1;
                        }
                    }

                    if (verificaCartaSorteada == 0)
                    {
                        vectorNumSort.Add(numSorteado);
                    }
                }


                string cartaEscolhida = "CartaObjetivo";
                cartaEscolhida = cartaEscolhida + numSorteado.ToString();
                Debug.Log(cartaEscolhida);
                string localCarta = "CartasObjetivos/" + cartaEscolhida;
                CartaParaObjetivo = Instantiate(Resources.Load(localCarta), listObjetivos.transform) as GameObject;
                CartaParaObjetivo.GetComponent<Carta>().cartaVirada = true;
                CartaParaObjetivo.GetComponent<Carta>().VirarCarta();
                CartaParaObjetivo.AddComponent<EscolheAlgoritmo>();
               
                if (i == 0)
                {
                    if (CartaObjetivoEscolhida.transform.childCount == 0)
                    {
                        GameObject cartaEscolhidaParaObjetivo = Instantiate(CartaParaObjetivo, CartaObjetivoEscolhida.transform);
                        cartaEscolhidaParaObjetivo.GetComponent<Carta>().cartaVirada = true;
                        cartaEscolhidaParaObjetivo.GetComponent<Carta>().VirarCarta();
                        var componentChose = cartaEscolhidaParaObjetivo.GetComponent<EscolheAlgoritmo>();
                        Destroy(componentChose);
                    }
                }
            }

        }

    }

    public void ButtonViewObjetivos()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            viewObjetivo.SetActive(true);

        }
    }

    public void ExitPanelObjetivos()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            viewObjetivo.SetActive(false);
        }
    }

    public void PressedbuttonVerificarAlgoritmoPronto()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            string algoritmoParaVerificar;
            string algoritmoMontado = "";
            int i;
            int pontos = 0;

            algoritmoParaVerificar = CartaObjetivoEscolhida.transform.GetChild(0).GetComponent<Carta>().atributo;

            //MONTANDO ALGORITMO PARA VERIFICAR
            for (i = 0; i < localAlgoritmo.transform.childCount; i++)
            {
                algoritmoMontado = algoritmoMontado + localAlgoritmo.transform.GetChild(i).GetComponent<Carta>().atributo;
                if (i != localAlgoritmo.transform.childCount - 1)
                {
                    algoritmoMontado = algoritmoMontado + "+";
                }
            }

            if (i == localAlgoritmo.transform.childCount)
            {
                string[] objetivos = algoritmoParaVerificar.Split('/');
                bool certo = false;
                for(int z=0; z < objetivos.Length; z++)
                {
                    //VERIFICA SE O ALGORITMO ESTÁ CORRETO
                    if (algoritmoMontado == objetivos[z])
                    {
                        certo = true;
                        Player.GetComponent<PlayerOnline>().DestruindoCartasDoAlgoritmoCerto();
                        if (CartaObjetivoEscolhida.transform.GetChild(0).GetComponent<Carta>().type == "bronze")
                        {
                            pontos = 5;
                            PlayerInformation.GetComponent<PlayerInformations>().AtualizaPontos(pontos);
                        }
                        else if (CartaObjetivoEscolhida.transform.GetChild(0).GetComponent<Carta>().type == "prata")
                        {
                            pontos = 10;
                            PlayerInformation.GetComponent<PlayerInformations>().AtualizaPontos(pontos);
                        }
                        else if (CartaObjetivoEscolhida.transform.GetChild(0).GetComponent<Carta>().type == "ouro")
                        {
                            pontos = 15;
                            PlayerInformation.GetComponent<PlayerInformations>().AtualizaPontos(pontos);
                        }

                        Player.GetComponent<PlayerOnline>().MostraPontosCorretos(pontos);

                        for (int x = 0; x < listaAlgoritmosMao.transform.childCount; x++)
                        {
                            string[] subs = CartaObjetivoEscolhida.transform.GetChild(0).name.Split('(');
                            string[] nomeObj = listaAlgoritmosMao.transform.GetChild(x).name.Split('(');
                            if (subs[0] == nomeObj[0])
                            {
                               PhotonNetwork.Destroy(listaAlgoritmosMao.transform.GetChild(x).gameObject);
                            }
                            subs = null;
                            nomeObj = null;
                        }

                        Destroy(CartaObjetivoEscolhida.transform.GetChild(0).gameObject);


                        //GERA CARTA OBJETIVO NO LUGAR DA CARTA DESTRUIDA
                        GameObject cartaEscolhidaParaObjetivo = Instantiate(listaAlgoritmosMao.transform.GetChild(listaAlgoritmosMao.transform.childCount - 1).gameObject, CartaObjetivoEscolhida.transform);
                        cartaEscolhidaParaObjetivo.GetComponent<Carta>().cartaVirada = true;
                        cartaEscolhidaParaObjetivo.GetComponent<Carta>().VirarCarta();
                        var componentChose = cartaEscolhidaParaObjetivo.GetComponent<EscolheAlgoritmo>();
                        Destroy(componentChose);
                    }
                    break;
                }
               
                if(certo == false)
                {
                    Player.GetComponent<PlayerOnline>().MostraPontosErrados();
                    Player.GetComponent<PlayerOnline>().RecebeCartasDoAlgoritmoErrado();
                    PlayerInformation.GetComponent<PlayerInformations>().AtualizaPontos(-5);
                }
            }
        }

        buttonVerificarAlgoritmoPronto.SetActive(false);
        buttonEndTurn.SetActive(true);
        buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonEndTurn.GetComponent<Button>().onClick.AddListener(buttonChangeTurn);
    }

    public void ButtonEscolheAlgoritmo()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            menuAction.SetActive(false);
            Player.GetComponent<PlayerOnline>().ActionConfereAlgoritmo();
            //Player.GetComponent<PlayerOnline>().RPC_EnviaAlgoritmoMesa();
            localAlgoritmo.GetComponent<Button>().enabled = true;
        }
    }

    public void ButtonRecebeAlgoritmoMesa()
    {
        PhotonView pV = Player.transform.GetComponent<PhotonView>();
        if (pV.IsMine)
        {
            Player.GetComponent<PlayerOnline>().EnviandoCartasParaCentroAlgoritmo();
            localAlgoritmo.GetComponent<Button>().enabled = false;
            buttonVerificarAlgoritmoPronto.SetActive(true);

        }
    }

    public void ButtonComprarAlgoritmo()
    {
        var PanelObj = GameObject.Find("PanelObjetivos");
        if (Player.transform.GetComponent<PhotonView>().IsMine)
        {
            if(listaAlgoritmosMao.transform.childCount < 3)
            {
                menuAction.SetActive(false);
                RecebeAlgoritmos(1);
                buttonEndTurn.SetActive(true);
                buttonEndTurn.GetComponent<Button>().onClick.RemoveAllListeners();
                buttonEndTurn.GetComponent<Button>().onClick.AddListener(this.GetComponent<GameManager>().buttonChangeTurn);
            }
            else
            {
                string mensagem = "Você possui o número máximo de objetivos em mãos";
                SendMessageAlert(mensagem);
            }
            
        }
    }

    /// <summary>
    /// FIM BLOCO DE ALGORITMOS 
    /// </summary>
    /// 

}
