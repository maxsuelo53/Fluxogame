using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public string nomejogador;
    public Text carregaNome;

    public Text scoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {        
        
    }

    // Update is called once per frame
    void Update()
    {

        GetNomePlayer();

        
    }

    private void GetNomePlayer()
    {
        nomejogador = PlayerPrefs.GetString("name", "nome");
        carregaNome.text = nomejogador;
        
    }

    public static void setNomePlayer(string nome) 
    {
        PlayerPrefs.SetString("name", nome);

    }

    public  void AdicionaScore(int pontos) {
        score += pontos;
        scoreText.text = score.ToString();
    }

    public void SubtraiScore(int pontos) {
        score -= pontos;
        scoreText.text = score.ToString();
    }

}
