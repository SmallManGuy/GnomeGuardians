using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    //[SerializeField] private ScreenTransition screenTransition;
    // Start is called before the first frame update
    public void Play()
    {
        //screenTransition.FadeToColor("GameScene");
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
