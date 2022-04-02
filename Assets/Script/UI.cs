using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void ExitGame(){
        Application.Quit();
    }

    public void ChangeScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
