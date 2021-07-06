
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelecionarCartaAlgoritmo : MonoBehaviour, IPointerDownHandler
{
    public bool cartaSelecionadaAlgoritmo = false;
    public GameObject mesaAlgoritmo;
    public GameObject Indice;

    private void Start()
    {
        mesaAlgoritmo = GameObject.Find("LocalAlgoritmo");
    }

    private void Update()
    {
        if (Indice != null)
        {
            Indice.GetComponent<Text>().text = mesaAlgoritmo.GetComponent<CriarAlgoritmoDeResposta>().GetIndiceAtualLista(this.gameObject).ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cartaSelecionadaAlgoritmo = !cartaSelecionadaAlgoritmo;
        if (cartaSelecionadaAlgoritmo == true)
        {
            CriarAlgoritmoDeResposta varCriarAlgoritmoDeResposta =  mesaAlgoritmo.GetComponent<CriarAlgoritmoDeResposta>();
            
            //CONTORNA A CARTA SELECIONADA
            var outline = gameObject.AddComponent<Outline>();
            outline.useGraphicAlpha = false;
            outline.effectColor = new Color (0,1,0,1);
            outline.effectDistance = new Vector2(6, 5);

            //GERA O NUMERO DO INDICE EM CIMA DA CARTA

            //ADICIONA A CARTA NA LISTA
            varCriarAlgoritmoDeResposta.MontandoAlgoritmo(this.gameObject, "add");
            //GERA O OBJETO PARA INDICAR O INDICE DA CARTA
            Vector3 posicaoCarta = transform.position;
            Indice = Instantiate(Resources.Load("IndiceCard"), 
                new Vector3( posicaoCarta.x, posicaoCarta.y+150, posicaoCarta.z), 
                Quaternion.identity) as GameObject;
            Indice.transform.SetParent(GameObject.Find("Canvas").transform);
            
            

        }
        else
        {
            var exp = GetComponent<Outline>();
            Destroy(exp);
            Indice.GetComponent<IndiceCardControl>().DestruindoIndiceCard();
            mesaAlgoritmo.GetComponent<CriarAlgoritmoDeResposta>().MontandoAlgoritmo(this.gameObject, "remove");
            
        }
    }



}
