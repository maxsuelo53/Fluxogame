using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    public Toggle toggleSom;
    public Toggle toggleMusica;
    public Slider sliderVolumeSom;
    public Slider sliderVolumeMusica;


    // Start is called before the first frame update
    void Start()
    {
        if (AplicationController.isFirstTime())
        {
            AplicationController.SetDefaultConfig();
        }

        toggleSom.isOn = AplicationController.IsMuttedSom();
        toggleMusica.isOn = AplicationController.IsMuttedMusica();
        sliderVolumeSom.value = AplicationController.GetVolumeSom();
        sliderVolumeMusica.value = AplicationController.GetVolumeMusica();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSom()
    {
        if (toggleSom.isOn)
        {
            AplicationController.EnableSom();
        }
        else
        {
            AplicationController.DisableSom();
        }            

    }

    public void SetMusica()
    {
        if (toggleMusica.isOn)
        {
            AplicationController.EnableMusica();
        }
        else
        {
            AplicationController.DisableMusica();
        }

    }

    public void SetVolumeSom()
    {
        AplicationController.SetVolumeSom(sliderVolumeSom.value);

    }

    public void SetVolumeMusica()
    {
        AplicationController.SetVolumeMusica(sliderVolumeMusica.value);
    }
}
