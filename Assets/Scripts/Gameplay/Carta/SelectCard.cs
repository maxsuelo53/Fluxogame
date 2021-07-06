using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    public Text posicaoselecionada;
    public int IndicePosicaoSelecionada;


    public void SetIndicePosicaoSelecionada(int posicaoSelecionada)
    {
        IndicePosicaoSelecionada = posicaoSelecionada+1;
        posicaoselecionada.text = IndicePosicaoSelecionada.ToString();
    }

}



