using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Animator _anim;

    public bool _isGameStarted = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void StartGame()
    {
        _anim.SetTrigger("StartGame");
        StartCoroutine(GameStartCouroutine());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<AudioManager>().Stop("Theme");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
        _isGameStarted = false;
    }

    private IEnumerator GameStartCouroutine()
    {
        yield return new WaitForSeconds(21f);
        FindObjectOfType<AudioManager>().PlayOneShot("Theme");
        _isGameStarted = true;
    }
}
