using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objetivos : MonoBehaviour
{
    public GameObject PanelObjetivos;
    public Text NumObjetivos;
    public GameObject ListaObjetivos;



    private void Update()
    {
        NumObjetivos.text = ListaObjetivos.transform.childCount.ToString();
    }




}
