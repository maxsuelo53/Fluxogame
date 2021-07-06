using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuOptions;

    // Start is called before the first frame update
    void Start()
    {
        ActiveMenu (menuPrincipal);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hidemenu()
    {
        menuPrincipal.SetActive(false);
        menuOptions.SetActive(false);

    }

    public void ActiveMenu( GameObject menu)
    {
        Hidemenu();
        menu.SetActive(true);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Jogar()
    {
        SceneManager.LoadSceneAsync("MenuMultiplayer");
    }
}
