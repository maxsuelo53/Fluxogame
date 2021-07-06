using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaGanhador : MonoBehaviour
{
    public GameObject textGanhador;
    // Start is called before the first frame update
    public void setTextGanhador(string nome)
    {
        textGanhador.GetComponent<Text>().text = nome;
    }
}
