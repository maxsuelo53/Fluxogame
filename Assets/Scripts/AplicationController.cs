using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplicationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static bool isFirstTime()
    {

        if (PlayerPrefs.GetString("FirstTime") != "jogoTCC")
            return true;

        return false;

    }

    public static void SetDefaultConfig()
    {
        PlayerPrefs.SetString("FirstTime", "jogoTCC");
        EnableSom();
        EnableMusica();
        SetVolumeMusica(1);
        SetVolumeSom(1);
    }



    // CONFIGURAÇÃO DO SOM //

    //SOM//

    public static float GetVolumeSom()
    {
        return PlayerPrefs.GetFloat("SomVolume");

    }
    public static void SetVolumeSom( float volume)
    {
        PlayerPrefs.SetFloat("SomVolume", volume);

    }

    public static void EnableSom()
    {
        PlayerPrefs.SetInt("Som", 1);

    }

    public static void DisableSom()
    {
        PlayerPrefs.SetInt("Som", 0);

    }

    public static bool IsMuttedSom()
    {
        if (PlayerPrefs.GetInt("Som") == 1) 
            return true;

        return false;

    }

    //MUSICA//

    public static float GetVolumeMusica()
    {
        return PlayerPrefs.GetFloat("MusicaVolume");
    }
    public static void SetVolumeMusica(float volume)
    {
        PlayerPrefs.SetFloat("MusicaVolume", volume);
    }
    public static void EnableMusica()
    {
        PlayerPrefs.SetInt("Musica", 1);
    }

    public static void DisableMusica()
    {
        PlayerPrefs.SetInt("Musica", 0);
    }

    public static bool IsMuttedMusica()
    {
        if (PlayerPrefs.GetInt("Musica") == 1)
            return true;

        return false;
    }

    // CONFIGURAÇÃO DO SOM //
}
