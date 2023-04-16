using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject player;
    private PlayerInput playerInput;

    public void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        playerInput = player.GetComponent<PlayerInput>();
    }

    public void Start()
    {
        SetMenuActive(true);
        playerInput.enabled = false;
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartScene()
    {
        SetMenuActive(false);
        playerInput.enabled = true;
    }

    public void PauseScene()
    {
        SetMenuActive(true);
        playerInput.enabled = false;

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsMenuVisible())
            {
                StartScene();

            }
            else
            {
                PauseScene();
            }
        }
    }


        public void SetMenuActive(bool active)
    {
        for (int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(active);
        }
    }

    public bool IsMenuVisible()
    {
        for (int a = 0; a < transform.childCount; a++)
        {
            if (transform.GetChild(a).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }


}
