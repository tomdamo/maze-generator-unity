using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Grid : MonoBehaviour
{
    [SerializeField] private Slider _widthSlider;
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private TMPro.TextMeshProUGUI _widthValueText;
    [SerializeField] private TMPro.TextMeshProUGUI _heightValueText;
    [SerializeField] private MazeGenerator _mazeGenerator;
    // Start is called before the first frame update
    void Start()
    {
        //set default values if slider aint touched
        _widthSlider.value = 10;
        _heightSlider.value = 10;
        _mazeGenerator.SetWidthValue((int)_widthSlider.value);
        _mazeGenerator.SetHeightValue((int)_heightSlider.value);
        _widthValueText.text = _widthSlider.value.ToString();
        _heightValueText.text = _heightSlider.value.ToString();

        _widthSlider.onValueChanged.AddListener((v) => 
        {
            _widthValueText.text = v.ToString();
            _mazeGenerator.SetWidthValue((int)v); // Send the integer value to MazeGenerator script
        });
        _heightSlider.onValueChanged.AddListener((v) => 
        {
            _heightValueText.text = v.ToString();
            _mazeGenerator.SetHeightValue((int)v); // Send the integer value to MazeGenerator script
        });
    }

    public void ResetGrid()
    {
        _widthSlider.value = 10;
        _heightSlider.value = 10;
        _mazeGenerator.SetWidthValue((int)_widthSlider.value);
        _mazeGenerator.SetHeightValue((int)_heightSlider.value);
        _widthValueText.text = _widthSlider.value.ToString();
        _heightValueText.text = _heightSlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
