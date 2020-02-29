using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public GameObject bird;
    public Vector3 spawnPosition;
    public CharacterController controller;
    public float speed = 12f;
    private int guardCount;
    public bool alive;
    public bool win;
    public AudioSource audioData;
    public AudioClip footsteps;

    // Start is called before the first frame update
    void Start()
    {
        guardCount = 0;
        alive = true;
        win = false;
        audioData.clip = footsteps;
    }

    // Update is called once per frame
    void Update()
    {
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (x == 0 && z == 0 || Time.timeScale == 0)
        {
            audioData.Pause();
        }
        else if(audioData.isPlaying == false)
            audioData.Play();

        if (win == true) { 
        SceneManager.LoadScene("VictoryScreen");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (guardCount <= 0) {
            if (other.tag == "BirdSpawn")
            {
                Instantiate(bird, spawnPosition, Quaternion.identity);
                guardCount += 1;
            }
        }

        if (other.tag == "BirdKill")
        {
            Destroy(GameObject.FindGameObjectWithTag("Bird"));
            GameObject.Find("UIManager").GetComponent<UIManager>().spotted = false;
        }

        if (other.tag == "Eggs")
            win = true;

        if (other.tag == "Bird")
        {
            alive = false;
            Time.timeScale = 0;
        }
    }
}
