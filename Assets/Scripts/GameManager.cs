using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject loseTextUI;
    public GameObject winTextUI;

    [Header("Задержки")]
    public float loseRestartDelay = 3f;
    public float winRestartDelay = 3f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;
        if (loseTextUI != null) loseTextUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(RestartAfterDelay(loseRestartDelay));
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        if (winTextUI != null) winTextUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(RestartAfterDelay(winRestartDelay));
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}