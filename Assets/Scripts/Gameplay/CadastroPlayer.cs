using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CadastroPlayer : MonoBehaviour
{

    public string salvaNome;

    public Text inputNome;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNome()
    {        

        salvaNome = inputNome.text;
        PlayerPrefs.SetString("nome", salvaNome);
        Player.setNomePlayer(salvaNome);
        
    }

    public void salvaDados()
    {
        SetNome();
        SceneManager.LoadSceneAsync("Gameplay");
    }
}
