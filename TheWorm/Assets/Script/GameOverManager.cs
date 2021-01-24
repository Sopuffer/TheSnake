using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
   public void OnClickRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
