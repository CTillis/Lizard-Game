using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject[] pauseObjects;
    GameObject[] finishObjects;
    PlayerController playerController;
    public bool spotted;
    GameObject HUDSpotted;
    GameObject HUDHidden;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");           
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        HUDSpotted = GameObject.Find("HUDSpotted");
        HUDHidden = GameObject.Find("HUDHidden");

        HidePaused();
        HideFinished();
        HUDSpotted.SetActive(false);

        if (SceneManager.GetActiveScene().name == "SampleScene")
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        spotted = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (spotted == true)
        {
            HUDSpotted.SetActive(true);
            HUDHidden.SetActive(false);
        }
        else if (spotted == false)
        {
            HUDSpotted.SetActive(false);
            HUDHidden.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {
            if(Time.timeScale == 1 && playerController.alive == true)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ShowPaused();
            }
            else if(Time.timeScale == 0 && playerController.alive == true)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                HidePaused();
            }
        }

        if (Time.timeScale == 0 && playerController.alive == false)
        {
            ShowFinished();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ShowPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HidePaused();
        }
    }

    public void ShowPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void HidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void ShowFinished()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnFinish tag
    public void HideFinished()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(false);
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
        
}


