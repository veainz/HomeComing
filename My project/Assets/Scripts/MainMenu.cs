using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
public class MainMenu : MonoBehaviour
{

   [SerializeField] GameObject PauseMenu;
   [SerializeField] GameObject GameOverMenu;

   public void Playgame()
   {
      SceneManager.LoadSceneAsync(1);
   }
   public void QuitGame()
   {
      Application.Quit();

#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#endif
   }

   public void PauseWithPauseMenu()
   {
      PauseMenu.SetActive(true);
      Time.timeScale = 0;
   }
   public void ExitWithPauseMenu()
   {
      SceneManager.LoadScene("MainMenu");
      Time.timeScale = 1;

   }
   public void RestartWithPauseMenu()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      Time.timeScale = 1;

   }
   public void ReseumWithPauseMenu()
   {
      PauseMenu.SetActive(false);
      Time.timeScale = 1;
   }
   public void GameOver()
   {
      GameOverMenu.SetActive(true);
     
   }
   public void PlayAgain()
   {
       Debug.Log("Restarting game...");
      GameOverMenu.SetActive(false);
   
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
   public void BacktoMenu(){
      SceneManager.LoadScene("MainMenu");
   }
   


}
