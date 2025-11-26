using UnityEngine;
using UnityEngine.UI;

public class SoundManagerTest : MonoBehaviour
{
    Slider somSlider; 
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ControlarVolume()
    {
        AudioListener.volume = somSlider.value;
    }
}
