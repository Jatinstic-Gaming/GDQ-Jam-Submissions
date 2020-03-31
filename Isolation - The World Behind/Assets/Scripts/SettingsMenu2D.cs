using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu2D : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown RDropdown;

    Resolution[] Resolutions;

    void Start()
    {
        Resolutions = Screen.resolutions;

        RDropdown.ClearOptions();

        int CRI = 0;
        List<string> Options = new List<string>();

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string Option = Resolutions[i].width + "x" + Resolutions[i].height;
            Options.Add(Option);

            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                CRI = i;
            }
        }

        RDropdown.AddOptions(Options);
        RDropdown.value = CRI;
        RDropdown.RefreshShownValue();
    }

    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat("volume", Volume);
    }

    public void SetQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void SetFullScreen(bool FullScreened)
    {
        Screen.fullScreen = FullScreened;
    }

    public void SetResolution(int RI)
    {
        Resolution resolution = Resolutions[RI];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
