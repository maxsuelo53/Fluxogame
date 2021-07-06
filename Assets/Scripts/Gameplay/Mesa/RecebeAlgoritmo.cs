using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecebeAlgoritmo : MonoBehaviour, IPointerDownHandler

{ 
    public GameObject handPlayer;
    public List<GameObject> listaAlgoritmo;


    //LIMPA A LISTA DAS CARTAS
    public void LimparLista()
    {
        listaAlgoritmo = new List<GameObject>();
    }

    //ADICIONA AS CARTAS SELECIONADAS NA LISTA PARA ENVIAR PARA O CENTRO DA MESA
    public void MontandoAlgoritmo( GameObject cartaAtual, string acao)
    {
        if (acao.Equals("add"))
        {
            listaAlgoritmo.Add(cartaAtual);
        }
        else
        {
            listaAlgoritmo.Remove(cartaAtual);
        }
        
        

    }

    //MANDA AS CARTAS PARA O CENTRO DA MESA
    public void EnviaAlgoritmoDescarte()
    {
        for (int i=0; i<listaAlgoritmo.Count; i++)
        {
            listaAlgoritmo[i].GetComponent<DragDrop>().enabled = false;
            listaAlgoritmo[i].GetComponent<EnviaCartaParaCentro>().enabled = false;
            listaAlgoritmo[i].transform.SetParent(this.transform);
        }
    }

    //RECEBE AS CARTAS QUANDO CLICADO
    public void OnPointerDown(PointerEventData eventData)
    {
        EnviaAlgoritmoDescarte();
    }

    public void DevolveCartasParaHandPlayer()
    {
        int tamanhoLista = listaAlgoritmo.Count;


        for (int i = 0; i < tamanhoLista; i++)
        {
            listaAlgoritmo[i].GetComponent<DragDrop>().enabled = true;
            listaAlgoritmo[i].GetComponent<EnviaCartaParaCentro>().enabled = false;
            listaAlgoritmo[i].transform.SetParent(handPlayer.transform);
            
        }

        RetornaScriptCartaMao();       

        LimparLista();
    }

    //SE O ALGORITMO ESTIVER CERTO AS CARTAS SERÃO DESTRUIDAS
    public void DestruirCartasDaMesa()
    {
        for (int i = 0; i < listaAlgoritmo.Count; i++)
        {
            GameObject CartaCerta = listaAlgoritmo[i].gameObject;
            Destroy(CartaCerta);

        }
        RetornaScriptCartaMao();
        LimparLista();
    }

    //VOLTA OS SCRIPTS DA CARTA
    public void RetornaScriptCartaMao()
    {
        for (int i = 0; i < handPlayer.transform.childCount; i++)
        {
            handPlayer.transform.GetChild(i).GetComponent<DragDrop>().enabled = true;
            handPlayer.transform.GetChild(i).GetComponent<EnviaCartaParaCentro>().enabled = false;
        }

    }

    //RETORNA INDICE DA LISTA
    public int GetIndiceAtualLista(GameObject estaCartaSelecionada)
    {
        return listaAlgoritmo.IndexOf(estaCartaSelecionada);
    }

}
