using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaDatabase : MonoBehaviour
{ 
    public int quantidadeCarta;
    public int quantidadeCartaDiferente;
    public GameObject localBaralho;
    public GameObject comprarCartaVirada;
    public List<GameObject> baralho = new List<GameObject>();
    public GameObject[] listaCartas = new GameObject[7];
    private float x = 0;
    


    public void Start()
    {
        criaBaralho();

    }

    private void Update()
    {
    }

    void criaBaralho()
    {
        //Define o número de cartas diferentes que deve ser gerada
        for (int i = 0; i < quantidadeCartaDiferente; i++)
        {
            //Define quantas cartas iguais deve ser gerada
            for (int j = 0; j < quantidadeCarta; j++)
            {
                //Adiciona as cartas no baralho
                baralho.Add(listaCartas[i]);
            }

        }

        Embaralhar();

    }

    public void Embaralhar()
    {
        int tamanhovetor = baralho.Count;
        GameObject cartaSorteada;

        //Embaralha as cartas do baralho
        for (int i=0; i<tamanhovetor-1; i++)
        {
            int x = Random.Range(0, tamanhovetor);
            cartaSorteada = baralho[x];
            baralho[x] = baralho[i];
            baralho[i] = cartaSorteada;

        }


        CriaBaralhoNaTela();

    }

    public void CriaBaralhoNaTela()
    {
        DestroyBaralho();
        //Instacia as cartas do baralho na tela
        for (int i = 0; i < baralho.Count; i++)
        {

            GameObject geraCartaAtual = Instantiate(baralho[i], new Vector3(x, 0, 0), Quaternion.identity);
            geraCartaAtual.transform.SetParent(localBaralho.transform, false);
            Vector3 novaPosicao = geraCartaAtual.transform.position;
            novaPosicao.x = (novaPosicao.x) + x;
            geraCartaAtual.transform.position = novaPosicao;
            x = x + 0.25f;
        }

    }

    //Exclui o baralho antigo e gera um novo
    public void DestroyBaralho()
    {
        if(localBaralho.transform.childCount != 0) 
        {
            for (int i=0; i< localBaralho.transform.childCount -1; i++)
            {
                Destroy(localBaralho.transform.GetChild(i).gameObject);
            }
        }
        x = 0;

    }

}
