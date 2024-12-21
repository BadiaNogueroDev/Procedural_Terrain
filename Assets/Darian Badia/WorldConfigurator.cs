using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldConfigurator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _chunkAmountInputField;
    [SerializeField] private TMP_InputField _chunkSizeXInputField;
    [SerializeField] private TMP_InputField _chunkSizeZInputField;
    [SerializeField] private TMP_InputField _seedInputField;
    [SerializeField] private Toggle _useSeedToggle;
    [SerializeField] private Button _generateButton;
    
    private WorldGenerator _worldGenerator;

    private int _chunkSizeMin = 3;
    private int _chunkSizeMax = 33;
    private int _seed;

    private void Start()
    {
        _worldGenerator = GetComponent<WorldGenerator>();
        _useSeedToggle.onValueChanged.AddListener(UseSeedToggleChanged);
        _generateButton.onClick.AddListener(SetParametersAndGenerateWorld);
    }

    private void SetParametersAndGenerateWorld()
    {
        SetSeed();
        
        if (!string.IsNullOrWhiteSpace(_chunkSizeXInputField.text) && !string.IsNullOrWhiteSpace(_chunkSizeZInputField.text))
            _worldGenerator.SetChunkSize(Mathf.Clamp(int.Parse(_chunkSizeXInputField.text), _chunkSizeMin, _chunkSizeMax), Mathf.Clamp(int.Parse(_chunkSizeZInputField.text), _chunkSizeMin, _chunkSizeMax));
        
        if (!string.IsNullOrWhiteSpace(_chunkAmountInputField.text))
            _worldGenerator.SetChunkAmount(int.Parse(_chunkAmountInputField.text));
        
        _worldGenerator.DeleteWorld();
        _worldGenerator.GenerateWorld();
    }

    private void UseSeedToggleChanged(bool value)
    {
        _seedInputField.interactable = value;
    }

    private void SetSeed()
    {
        if (_useSeedToggle.isOn)
            SetDefinedSeed();
        else
            SetRandomSeed();
    }
    
    private void SetDefinedSeed()
    {
        if (!string.IsNullOrWhiteSpace(_seedInputField.text))
        {
            _seed = int.Parse(_seedInputField.text);
            Random.InitState(_seed);
        }
        else
        {
            SetRandomSeed();
        }
    }

    private void SetRandomSeed()
    {
        _seed = Random.Range(1, 1000000);
        Random.InitState(_seed);
        _seedInputField.text = _seed.ToString();
    }
}