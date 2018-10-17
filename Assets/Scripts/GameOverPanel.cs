using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject scoreControl;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text recordText;

    AudioSource sound;

    GameScore gameScoreControl;

    #endregion

    #region Panel Interact
    
    /// <summary>
    /// Отобразить панель
    /// </summary>
    public void Show()
    {
        if(gameScoreControl == null)
            gameScoreControl = scoreControl.GetComponent<GameScore>();

        gameObject.SetActive(true);
        if (sound == null)
            sound = GetComponent<AudioSource>();
        sound.Play();

        gameScoreControl.UpdateScore();

        scoreText.text = gameScoreControl.Score.ToString();
        recordText.text = gameScoreControl.Record.ToString();

        gameScoreControl.ResetScore();
    }


    /// <summary>
    /// Скрыть панель
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
