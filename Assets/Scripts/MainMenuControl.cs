using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    #region Fields

    const string SOUND_ON_TEXT = "Sound ON";
    const string SOUND_OFF_TEXT = "Sound OFF";
    const float VOLUME_OFF = 0f;
    const float VOLUME_ON = 1f;

    [SerializeField]
    private Text soundText;

    bool isSoundPlayMode;

    #endregion


    #region Unity LifeCycle

    void Start()
    {
        isSoundPlayMode = true;
    }

    #endregion

    #region Stick Interact

    /// <summary>
    /// Изменение режима аудио на противоположный
    /// </summary>
    public void ChangeSoundMode()
    {
        if (isSoundPlayMode)
        {
            soundText.text = SOUND_OFF_TEXT;
            AudioListener.volume = VOLUME_OFF;
        }
        else
        {
            soundText.text = SOUND_ON_TEXT;
            AudioListener.volume = VOLUME_ON;
        }
        isSoundPlayMode = !isSoundPlayMode;
    }


    /// <summary>
    /// Выход из игры
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    #endregion
}
