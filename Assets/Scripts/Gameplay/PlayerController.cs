using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //public GameObject baralho;
    public GameObject handPlayer;
    public GameObject baralho;
    GameObject cartaComprada;
    int ultimaCarta;

    // Start is called before the first frame update
    void Start()
    {
        DistribuiCarta();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DistribuiCarta()
    {
        for (int i=1; i<4; i++)
        {
            ultimaCarta = baralho.transform.childCount - 1;
            cartaComprada = Instantiate(baralho.transform.GetChild(ultimaCarta).gameObject, handPlayer.transform, false);
            cartaComprada.GetComponent<Carta>().cartaVirada = true;
            cartaComprada.GetComponent<Carta>().VirarCarta();
            Destroy(baralho.transform.GetChild(ultimaCarta).gameObject);

        }
        
    }

    public void CompraCarta()
    {
        ultimaCarta = baralho.transform.childCount-1;
        
        cartaComprada = Instantiate( baralho.transform.GetChild(ultimaCarta).gameObject , handPlayer.transform,false);
        cartaComprada.GetComponent<Carta>().cartaVirada = true;
        cartaComprada.GetComponent<Carta>().VirarCarta();
        Destroy(baralho.transform.GetChild(ultimaCarta).gameObject);



    }

}
