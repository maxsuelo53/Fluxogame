using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnviaCartaParaCentro : MonoBehaviour, IPointerDownHandler
{
    public GameObject handPlayer;
    public GameObject descarte;
    private Vector3 posicaoPadrao;
    public bool escolhida = false;

    public GameObject cartaSelecionada;
    int indiceCarta;

    private void Start()
    {
        posicaoPadrao = this.gameObject.transform.position;
        this.GetComponent<DragDrop>().enabled = false;
        handPlayer = GameObject.Find("HandPlayer");
        descarte = GameObject.Find("MesaAlgoritmo");

    }

    private void Update()
    {
        indiceCarta = descarte.GetComponent<RecebeAlgoritmo>().GetIndiceAtualLista(this.gameObject);
    }

    //LEVANTA A CARTA SELECIONADA E ENVIA PARA A LISTA DE ALGORITMO MONTADO
    public void OnPointerDown(PointerEventData eventData)
    {
        escolhida = !escolhida;
        
        if (escolhida)
        {
            //this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y+50, this.gameObject.transform.position.z);
            descarte.GetComponent<RecebeAlgoritmo>().MontandoAlgoritmo(this.gameObject, "add");
            SelecionandoCartas();

        }
        else
        {
            //this.gameObject.transform.position = posicaoPadrao;
            descarte.GetComponent<RecebeAlgoritmo>().MontandoAlgoritmo(this.gameObject, "remove");
        }
    }

    public void SelecionandoCartas()
    {
        
    }

}
