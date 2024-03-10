using UnityEngine;
using UnityEngine.UI;

public class VolumeManager: MonoBehaviour {

    [SerializeField] Slider volumeSlider;

    void Start() {
        if(!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
            Debug.Log("AudioListener volume " + AudioListener.volume);
            Debug.Log("volumeSlider volume " + volumeSlider.value);
        } else {
            Load();
            Debug.Log("In else:  AudioListener volume " + AudioListener.volume);
            Debug.Log("IN else: volumeSlider volume " + volumeSlider.value);
        }
    }
    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        Debug.Log("In change: AudioListener volume " + AudioListener.volume);
        Debug.Log("In change: volumeSlider volume " + volumeSlider.value);
        Save();
    }

    private void Save() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Load() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}