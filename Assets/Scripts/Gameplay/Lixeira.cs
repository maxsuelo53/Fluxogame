using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lixeira : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        DesativaLayoutCartaLixo();
        RecebeCartaLixo();
        
    }

    //DESTROI CARTAS DEIXANDO SOMENTE 1
    public void RecebeCartaLixo()
    {
        if (transform.childCount > 1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.name != "DescricaoLixeira" && transform.GetChild(i).gameObject.name != "New Game Object")
                {
                    transform.GetChild(i).gameObject.GetComponent<Carta>().cartaVirada = false;
                    transform.GetChild(i).gameObject.GetComponent<Carta>().VirarCarta();
                }
            }
        }

        if (transform.childCount == 5)
        {
            for (int i = 0; i < transform.childCount - 2; i++)
            {
                if (transform.GetChild(i).gameObject.name != "DescricaoLixeira" && transform.GetChild(i).gameObject.name != "New Game Object")
                {
                    Destroy(transform.GetChild(i).gameObject);
                    
                }
            }
        }
    }

    //POSICIONA AS CARTAS DE LIXO
    public void DesativaLayoutCartaLixo()
    {
        for (int i=0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.name != "DescricaoLixeira")
            {
                GameObject cartaRascunho = transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.transform.position = this.gameObject.transform.position;
                transform.GetChild(i).gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
            }       

        }

    }

}
