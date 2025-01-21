using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    UIDocument PauseMenu;
    Scene mainMenu;

    // Pause Panel
    VisualElement pause_Container;
    Button pause_resumeButton, pause_restartButton, pause_settingsButton, pause_returnButton;

    // Options Panel
    VisualElement options_Instance, options_Scrim, options_Container;
    Button options_musicButton, options_soundButton, options_exitButton;

    private void OnEnable() 
    {
        PauseMenu = this.GetComponent<UIDocument>();
        VisualElement root = PauseMenu.rootVisualElement;

        // Pause Panel Queries
        pause_Container = root.Q<VisualElement>(className:"pause_container");
        pause_Container.RemoveFromClassList("pause_container--down-deactivate");
        pause_resumeButton = root.Q<Button>("btnResume");
        pause_restartButton = root.Q<Button>("btnRestart");
        pause_settingsButton = root.Q<Button>("btnSettings");
        pause_returnButton = root.Q<Button>("btnReturn");
        // Callbacks
        pause_resumeButton.RegisterCallback<ClickEvent>(ClosePausePanel);
        pause_restartButton.RegisterCallback<ClickEvent>(ResetLevel);
        pause_settingsButton.RegisterCallback<ClickEvent>(OpenOptionsPanel);
        pause_returnButton.RegisterCallback<ClickEvent>(ReturnTitle);
        

        // Options Panel Queries
        options_Instance = root.Q<VisualElement>(className:"options_instance");
        options_Scrim = root.Q<VisualElement>(className:"options_scrim");
        options_Container = root.Q<VisualElement>(className:"options_container");
        options_musicButton = root.Q<Button>("btnMusic");
        options_soundButton = root.Q<Button>("btnSound");
        options_exitButton = root.Q<Button>(className:"options_btn-exit");
        // Callbacks
        options_musicButton.RegisterCallback<ClickEvent>(ActivateMusic);
        options_soundButton.RegisterCallback<ClickEvent>(ActivateSound);
        options_exitButton.RegisterCallback<ClickEvent>(CloseOptionsPanel);
    }

    private void CloseOptionsPanel(ClickEvent evt)
    {
        options_Instance.AddToClassList("options_instance--deactivate");
        options_Scrim.AddToClassList("options_scrim--deactivate");
        options_Container.AddToClassList("options_container--down-deactivate");
    }

    private void ActivateSound(ClickEvent evt)
    {
        Debug.Log("aun no se puede modificar los sonidos");
    }

    private void ActivateMusic(ClickEvent evt)
    {
        Debug.Log("aun no se puede modificar la musica");
    }

    private void ReturnTitle(ClickEvent evt)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        this.gameObject.SetActive(false);
    }

    private void OpenOptionsPanel(ClickEvent evt)
    {
        options_Instance.RemoveFromClassList("options_instance--deactivate");
        options_Scrim.RemoveFromClassList("options_scrim--deactivate");
        options_Container.RemoveFromClassList("options_container--down-deactivate");
    }

    private void ResetLevel(ClickEvent evt)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.gameObject.SetActive(false);
    }

    private void ClosePausePanel(ClickEvent evt)
    {
        Time.timeScale = 1f;
        pause_Container.AddToClassList("pause_container--down-deactivate");
        this.gameObject.SetActive(false);
    }
}
