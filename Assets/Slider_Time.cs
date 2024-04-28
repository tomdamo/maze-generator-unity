using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Time : MonoBehaviour
{
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private TMPro.TextMeshProUGUI _timeValueText;
    [SerializeField] private MazeGenerator _mazeGenerator;
    // Start is called before the first frame update
    void Start()
    {
        // Set default values if slider aint touched
        _timeSlider.value = 0.01f;
        _mazeGenerator.SetTimeValue((int)_timeSlider.value);
        _timeValueText.text = _timeSlider.value.ToString("0.00");

        _timeSlider.onValueChanged.AddListener((v) => 
        {
            _timeValueText.text = v.ToString("0.00");
            _mazeGenerator.SetTimeValue((int)v); // Send the integer value to MazeGenerator script
        });
    }

    public void ResetTime()
    {
        _timeSlider.value = 0.01f;
        _mazeGenerator.SetTimeValue((int)_timeSlider.value);
        _timeValueText.text = _timeSlider.value.ToString("0.00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
