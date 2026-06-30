using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;
    
    [SerializeField]
    private AudioMixer mixer;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        
        if (mixer.GetFloat("MasterVolume", out var volume))
        {
            Debug.Log(Mathf.Pow(10, volume / 20));
            volumeSlider.value = Mathf.Pow(10, volume / 20);
        }
        
        volumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
    }

    private void UpdateMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
