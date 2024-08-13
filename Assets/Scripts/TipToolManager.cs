using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipToolManager : MonoBehaviour
{
    private string[] tips =
    {
        "Use double jumps to dodge enemy attacks and stay mobile.",
        "Shoot continuously to keep the area clear of enemies.",
        "Jump on enemies' heads to defeat them instantly",
        "Press Shift to dash and quickly reposition yourself in tight situations.",
        "Keep an eye on the timer! Maximize your score before time runs out.",
        "Combine jumps and dashes to stay unpredictable and avoid getting cornered.",
        "Watch enemy patterns and movements to plan your attacks effectively.",
        "Utilize every ability you have to survive and score higher.",
    };

    [SerializeField] TextMeshProUGUI tipText;

    private float tipChangeTime = 5f;
    private float timeElapsed;

    private void Start()
    {
        RandomTipGenerator();
    }

    // Update is called once per frame
    private void Update()
    {
        timeElapsed += Time.unscaledDeltaTime;
        if (timeElapsed > tipChangeTime)
        {
            RandomTipGenerator();
        }
    }

    private void RandomTipGenerator()
    {
        int index = Random.Range(0, tips.Length);
        tipText.text = tips[index];
        timeElapsed = 0;
    }
}
