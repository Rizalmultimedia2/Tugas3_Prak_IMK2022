using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI : MonoBehaviour
{
    private Controls _controls;
    public GameObject InstruksiMenu; 
    public bool instruksi;
    public void ExitGame(){
        Application.Quit();
    }

    public void ChangeScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

	private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }    
    public void InstruksiOut()
    {
		Cursor.lockState = CursorLockMode.None;
        InstruksiMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void InstruksiIn()
    {
		Cursor.lockState = CursorLockMode.None;
        InstruksiMenu.SetActive(true);
        Time.timeScale = 0f;
    }    

}
