using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button Play;
    [SerializeField] private Button Exit;
    [SerializeField] private InputField NameInputField;
    [SerializeField] private Slider Red;
    [SerializeField] private Slider Green;
    [SerializeField] private Slider Blue;

    [SerializeField] private TextMesh PlayerName;
    [SerializeField] private SpriteRenderer PlayerRenderer;

    private void OnRedSliderValueChanged(float value)
    {
        var color = new Color(value, PlayerRenderer.color.g, PlayerRenderer.color.b);
        GameManager.MyColor = color;
        PlayerRenderer.color = color;
    }
    private void OnGreenSliderValueChanged(float value)
    {
        var color = new Color(PlayerRenderer.color.r, value, PlayerRenderer.color.b);
        GameManager.MyColor = color;
        PlayerRenderer.color = color;
    }
    private void OnBlueSliderValueChanged(float value)
    {
        var color = new Color(PlayerRenderer.color.r, PlayerRenderer.color.g, value);
        GameManager.MyColor = color;
        PlayerRenderer.color = color;
    }

    private void OnQuitButtonClick() 
    {
        Application.Quit();
    }

    private void OnNameChanged(string name)
    {
        PlayerName.text = name;
        GameManager.MyName = name;
    }

    void Awake() 
    {
        Red.onValueChanged.AddListener(OnRedSliderValueChanged);
        Green.onValueChanged.AddListener(OnGreenSliderValueChanged);
        Blue.onValueChanged.AddListener(OnBlueSliderValueChanged);
        NameInputField.onValueChanged.AddListener(OnNameChanged);
        Exit.onClick.AddListener(OnQuitButtonClick);
    }

    void OnDestroy() 
    {
        Red.onValueChanged.RemoveListener(OnRedSliderValueChanged);
        Green.onValueChanged.RemoveListener(OnGreenSliderValueChanged);
        Blue.onValueChanged.RemoveListener(OnBlueSliderValueChanged);
        NameInputField.onValueChanged.RemoveListener(OnNameChanged);
        Exit.onClick.AddListener(OnQuitButtonClick);
    }

    public void SetUIStatus(bool value) 
    {
        gameObject.SetActive(value);
        PlayerRenderer.gameObject.SetActive(value);
    }
}
