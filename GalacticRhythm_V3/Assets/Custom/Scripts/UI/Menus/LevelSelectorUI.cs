using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectorUI : MonoBehaviour
{
    UIDocument levelSelector;
    Scene gameScene;
    GameObject previousScreen;
    public string previousScreenName;

    // Level Selector
    VisualElement levelSelectorScreen;
    Button backButton, startButton;
    
    // Loading Bar
    VisualElement loadScreen;
    ProgressBar loadingBar;

    private void OnEnable()
    {
        levelSelector = this.GetComponent<UIDocument>();
        VisualElement root = levelSelector.rootVisualElement;

        // Level Selector Queries
        levelSelectorScreen = root.Q<VisualElement>("ParentContainer");
        backButton = root.Q<Button>("btnBack");
        startButton = root.Q<Button>("btnStart");
        // Callbacks
        backButton.RegisterCallback<ClickEvent>(ReturnMainMenu);
        startButton.RegisterCallback<ClickEvent>(StartLevel);

        // Loading Bar Queries
        loadScreen = root.Q<VisualElement>("LoadScreenContainer");
        loadingBar = root.Q<ProgressBar>("loadingBar");

        // Previous Screen
        previousScreen = GameObject.Find(previousScreenName);
    }

    private void StartLevel(ClickEvent evt)
    {
        levelSelectorScreen.AddToClassList("parent-container--hidden");
        loadScreen.RemoveFromClassList("load_container--deactivate");
        StartCoroutine(nameof(LoadGame));
    }

    private void ReturnMainMenu(ClickEvent evt)
    {
        previousScreen.SetActive(true);
    }

    public IEnumerator LoadGame()
    {
        AsyncOperation loadingGame = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while(!loadingGame.isDone)
        {
            loadingBar.lowValue = loadingGame.progress;
            yield return null;
        }
    }
}
