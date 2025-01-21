using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    UIDocument title;
    
    // Main Function
    Button startButton;

    private void OnEnable()
    {
        title = this.GetComponent<UIDocument>();
        VisualElement root = title.rootVisualElement;

        //Title Queries
        startButton = root.Q<Button>("btnStart");
        //Callbacks
        startButton.RegisterCallback<ClickEvent>(OpenMenu);
    }

    private void OpenMenu(ClickEvent evt)
    {
        Invoke(nameof(DeactivateScene), .3f);
    }

    private void DeactivateScene()
    {
        this.gameObject.SetActive(false);
    }
}
