using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    UIDocument mainMenu;
    GameObject previousScreen;
    public string previousScreenName;

    // Main Menu
    VisualElement spaceshipImage;
    Button playButton, optionsButton, exitButton;

    //Option Panel
    VisualElement options_Instance, options_Scrim, options_Container;
    Button options_musicButton, options_soundButton, options_exitButton;

    private void OnEnable() 
    {
        mainMenu = this.GetComponent<UIDocument>();
        VisualElement root = mainMenu.rootVisualElement;

        // Main Menu Queries
        spaceshipImage = root.Q<VisualElement>("imgSpaceship");
        playButton = root.Q<Button>("btnPlay");
        optionsButton = root.Q<Button>("btnOptions");
        exitButton = root.Q<Button>("btnExit");
        // Callbacks
        playButton.RegisterCallback<ClickEvent>(OpenLevelSelector);
        optionsButton.RegisterCallback<ClickEvent>(OpenOptionsPanel);
        exitButton.RegisterCallback<ClickEvent>(ReturnTitle);

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

        // Previous Screen
        previousScreen = GameObject.Find(previousScreenName);
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
        previousScreen.SetActive(true);
    }

    private void OpenOptionsPanel(ClickEvent evt)
    {
        options_Instance.RemoveFromClassList("options_instance--deactivate");
        options_Scrim.RemoveFromClassList("options_scrim--deactivate");
        options_Container.RemoveFromClassList("options_container--down-deactivate");
    }

    private void OpenLevelSelector(ClickEvent evt)
    {
        Invoke(nameof(DeactivateScene), .3f);
    }
    private void DeactivateScene()
    {
        this.gameObject.SetActive(false);
    }
}
