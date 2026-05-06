using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    public GameObject winTextUI;
    public float restartDelay = 2f; // задержка перед перезапуском сцены

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f; // останавливаем игру
            if (winTextUI != null)
                winTextUI.SetActive(true); // показываем надпись

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        // ждём
        yield return new WaitForSecondsRealtime(restartDelay);
        Time.timeScale = 1f;
        // перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}