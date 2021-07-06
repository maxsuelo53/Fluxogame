using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public PlayerController player1;
    public GameObject baralho;
    public GameObject handPlayer;
    public GameObject botaoComprarCartas;
    public GameObject botaoJogarBug;
    public GameObject botaoConferirAlgoritmo;
    public GameObject botaoComprarCartaVirada;
    public GameObject botaoComprarCartaMonte;
    public GameObject botaoObjetivos;
    public GameObject mostraObjetivos;
    public GameObject mostraAcoes;
    public GameObject MesaAlgoritmo;
    public GameObject ButtonAnalisaAlgoritmo;
    public GameObject lixeira;
    public GameObject TurnPlayer;
    public GameObject currentScore;
    public GameObject buttonPassaTurno;
    public int currentTurn;
    bool jogadaPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        InicioDoJogo();
        instance = this;
        currentTurn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ConfereCartaVirada();


    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }


    /*
     * =====================================================================================================================
     * INICIO DO BLOCO PARA CONTROLAR O JOGO
     */
     public void InicioDoJogo()
    {
        botaoComprarCartaMonte.GetComponent<Button>().enabled = false;
        botaoComprarCartaVirada.GetComponent<Button>().enabled = false;
        mostraAcoes.SetActive(false);

    }


    /*
     * FIM DO BLOCO PARA CONTROLAR TURNO
     * =====================================================================================================================
     */






    /*
     * =====================================================================================================================
     * INICIO DO BLOCO PARA CONTROLAR TURNO
     */

    public void TurnGame()
    {
        jogadaPlayer = false;
        SetButtonsTurn();
        Debug.Log(jogadaPlayer);
    }

    public void SetButtonsTurn()
    {
       lixeira.GetComponent<Lixeira>().enabled = false;
       lixeira.GetComponent<DropZone>().enabled = false;
       buttonPassaTurno.SetActive(false);
    }

    public void CurrentTurn1()
    {
        currentTurn = 1;
        VerificaTurno();
    }

    public void CurrentTurn2()
    {
        currentTurn = 2;
        VerificaTurno();
    }
    public void AlteraTurno()
    {
        if(jogadaPlayer == false)
        {
            jogadaPlayer = true;
            currentTurn = 1;
        }
        else if (jogadaPlayer == true)
        {
            jogadaPlayer = false;
        }

        VerificaTurno();

    }
    public void VerificaTurno()
    {
        GameObject telaJogo = GameObject.Find("Tela de jogo");
        //TURNO DO JOGADOR
        if (currentTurn == 1)
        {
            //NÃO CONSEGUE JOGAR CARTA NO LIXO
            lixeira.GetComponent<Lixeira>().enabled = false;
            lixeira.GetComponent<DropZone>().enabled = false;

            //BOTAO PARA PASSAR TURNO FICA INATIVO
            buttonPassaTurno.SetActive(false);

            //ANIMAÇÃO DO TURNO NA TELA
            GameObject TurnPlayerAtual = Instantiate(TurnPlayer, new Vector3(0, 0, 0), Quaternion.identity, telaJogo.transform);
            TurnPlayerAtual.GetComponent<TurnPlay>().OneCurrentTurnPlayer();

            //ATIVA OS BOTÕES DE AÇÃO
            mostraAcoes.SetActive(true);

        }
        //TURNO DO INIMIGO
        else if (currentTurn == 2)
        {
            //ANIMAÇÃO DO TURNO NA TELA
            GameObject TurnPlayerAtual = Instantiate(TurnPlayer, new Vector3(0, 0, 0), Quaternion.identity, telaJogo.transform);
            TurnPlayerAtual.GetComponent<TurnPlay>().TwoCurrentTurnPlayer();

            //DESATIVA OS BOTÕES DE AÇÃO
            mostraAcoes.SetActive(false);

            //BOTAO PARA PASSAR TURNO FICA ATIVO
            buttonPassaTurno.SetActive(true);

            //CONSEGUE JOGAR CARTA NO LIXO
            lixeira.GetComponent<Lixeira>().enabled = true;
            lixeira.GetComponent<DropZone>().enabled = true;

        }
    }
    /*
     * FIM DO BLOCO PARA CONTROLAR TURNO
     * =====================================================================================================================
     */







    /*
     * =====================================================================================================================
     * INICIO DO BLOCO PARA CONTROLAR BARALHO
     */

    //GERA UMA CARTA DO LADO DO BARALHO
    public void GeraCartaVirada()
    {
        GameObject cartaTopo;

        //Pega a ultima do baralho
        cartaTopo = baralho.transform.GetChild(baralho.transform.childCount - 1).gameObject;

        //Gera essa carta ao lado do baralho
        cartaTopo = Instantiate(cartaTopo, botaoComprarCartaVirada.transform, false);
        cartaTopo.GetComponent<Carta>().cartaVirada = true;
        cartaTopo.GetComponent<Carta>().VirarCarta();
        cartaTopo.GetComponent<DragDrop>().enabled = false;
        cartaTopo.transform.position = botaoComprarCartaVirada.transform.position;

    }

    //SE NÃO HOUVER CARTA VIRADA IRÁ GERAR UMA CARTA
    public void ConfereCartaVirada()
    {
           
        if ((botaoComprarCartaVirada.transform.childCount) == 0)
        {
            GeraCartaVirada();

        }
    }

    /*
     * FIM DO BLOCO PARA CONTROLAR BARALHO
     * =====================================================================================================================
     */






    /*
    * =====================================================================================================================
    * INICIO DO BLOCO PARA CONTROLAR BOTAO DE COMPRA DE CARTAS
    */

    //BOTAO (COMPRA CARTAS)
    public void BotaoComprar()
    {

        //DESATIVA OS BOTÕES DE AÇÃO
        DesativaBotaoAcao();

        //ATIVA A COMPRA DE CARTAS
        botaoComprarCartaVirada.GetComponent<Button>().enabled = true;
        botaoComprarCartaMonte.GetComponent<Button>().enabled = true;

    }

    //BOTAO (COMPRAR CARTA DO MONTE)
    public void BotaoComparDoMonte()
    {
        GameObject cartaComprada;

        //PEGA DUAS CARTAS DO MONTE E ENVIA PARA MAO DO PLAYER
        for (int i = 2; i <= 3; i++)
        {
            cartaComprada = baralho.transform.GetChild(baralho.transform.childCount - i).gameObject;
            cartaComprada = Instantiate(cartaComprada, handPlayer.transform, false);
            cartaComprada.GetComponent<Carta>().cartaVirada = true;
            cartaComprada.GetComponent<Carta>().VirarCarta();

        }

        baralho.GetComponent<CartaDatabase>().Embaralhar();

        //DESATIVA BOTÃO DE COMPRAR CARTAS
        desativaBotaoCompraDoMonte();

        //PASSA PARA TURNO 2 DO JOGADOR
        CurrentTurn2();

    }

    //BOTAO (COMPRAR CARTA DO LADO DO BARALHO)
    public void BotaoComprarCartaVirada()
    {
        GameObject cartaTopo;
        cartaTopo = botaoComprarCartaVirada.transform.GetChild(botaoComprarCartaVirada.transform.childCount - 1).gameObject;
        //Passa a carta que está virada para a mao do player
        cartaTopo.transform.SetParent(handPlayer.transform, false);
        cartaTopo.GetComponent<DragDrop>().enabled = true;

        desativaBotaoCompraDoMonte();

        baralho.GetComponent<CartaDatabase>().Embaralhar();


        GeraCartaVirada();

        //PASSA PARA TURNO 2 DO JOGADOR
        CurrentTurn2();

    }

    //DESATIVA O BOTAO DE ACAO
    public void DesativaBotaoAcao()
    {
        mostraAcoes.SetActive(false);

    }
    
    //DESATIVA OS BOTOES DE PEGAR CARTA DO MONTE
    public void desativaBotaoCompraDoMonte()
    {
        botaoComprarCartaVirada.GetComponent<Button>().enabled = false;
        botaoComprarCartaMonte.GetComponent<Button>().enabled = false;
    }


    /*
    * FIM DO BLOCO PARA CONTROLAR BOTAO DE COMPRA DE CARTAS
    * =====================================================================================================================
    */






    /*
    * =====================================================================================================================
    * INICIO DO BLOCO PARA CONTROLAR BOTAO DE CONFERIR ALGORITMO
    */

    
    //BOTAO PARA CONFERIR ALGORITMO
    public void BotaoConferirAlgoritmo()
    {
        //MOSTRA OS OBJETIVOS
        MostrandoObjetivos();

        //ATIVA BOTAO PARA ANALISAR ALGORITMO
        ButtonAnalisaAlgoritmo.SetActive(true);

        //DESATIVA OPÇÃO DE ARRASTAR AS CARTAS
        handPlayer.GetComponent<DropZone>().enabled = false;    

        //ATIVA SCRIPT PARA SELECIONAR CARTA
        AtivaScriptSelecionaCarta();

        //PASSA PARA TURNO 2 DO JOGADOR
        CurrentTurn2();




    }

    //MOSTRA OS OBJETIVOS DO PLAYER
    public void MostrandoObjetivos()
    {
        //DESATIVA BOTOES DA TELA
        DesativaBotaoAcao();

        //MOSTRA OS ALGORITMOS PARA O PLAYER ESCOLHER
        mostraObjetivos.SetActive(true);

        //CONFERE QUANTAS CARTAS TEM NO ALGORITMO(SE MAIS DE 1 IRÁ REMOVER A CARTA QUE JÁ ESTÁ)
        if (botaoObjetivos.transform.childCount > 0)
        {
            Destroy(botaoObjetivos.transform.GetChild(0).gameObject);

        }        

    }


    //ATIVA O SCRIPT PARA MANDAR AS CARTAS PARA O CENTRO DA MESA
    public void AtivaScriptSelecionaCarta()
    {
        for (int i=0; i< handPlayer.transform.childCount; i++ )
        {
            handPlayer.transform.GetChild(i).gameObject.AddComponent<EnviaCartaParaCentro>();

        }
    }

    //CONFERE SE O ALGORITMO CRIADO É IGUAL DO OBJETIVO
    public void ConferirAlgoritmoMontado()
    {
        //RECEBE O ALGORITMO DE OBJETIVO
        string objetivo = botaoObjetivos.transform.GetChild(0).gameObject.GetComponent<Carta>().atributo;
        string montandoAlgoritmo = "";
        
        //MONTA O ALGORITMO COM AS CARTAS SELECIONADAS
        for (int i = 0; i < MesaAlgoritmo.transform.childCount; i++)
        {
            montandoAlgoritmo = montandoAlgoritmo + MesaAlgoritmo.transform.GetChild(i).gameObject.GetComponent<Carta>().atributo;

            if(i != MesaAlgoritmo.transform.childCount-1)
            {
                montandoAlgoritmo = montandoAlgoritmo + "+";
            }
            Debug.Log("Algoritmo: " +montandoAlgoritmo);

        }
        
        //MOSTRA SE O PLAYER GANHOU OU PERDEU PONTOS
        mostraResultado(montandoAlgoritmo, objetivo);

        //DESATIVA O BOTAO DE ANALISAR ALGORITMO
        ButtonAnalisaAlgoritmo.SetActive(false);

    }

    //GERENCIA OS PONTOS
    public void mostraResultado(string montandoAlgoritmo, string objetivo)
    {
        GameObject telaJogo = GameObject.Find("Tela de jogo");

        //RECEBE INCREMENTO NO SCORE
        if (montandoAlgoritmo.Equals(objetivo))
        {
            player1.GetComponent<Player>().AdicionaScore(5);
            MesaAlgoritmo.GetComponent<RecebeAlgoritmo>().DestruirCartasDaMesa();
        }
        //RECEBE UM DECREMENTO NO SCORE
        else
        {
            player1.GetComponent<Player>().SubtraiScore(5);
            Instantiate(currentScore, new Vector3(0, 0, 0), Quaternion.identity, telaJogo.transform);
            MesaAlgoritmo.GetComponent<RecebeAlgoritmo>().DevolveCartasParaHandPlayer();
        }

    }

    /*
    * FIM DO BLOCO PARA CONTROLAR BOTAO DE CONFERIR ALGORITMO
    * =====================================================================================================================
    */


}

