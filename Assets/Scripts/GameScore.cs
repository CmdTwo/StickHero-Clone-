using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    #region Fields / Properties

    const float SEC_FOR_WAIT = 1f;
    const string KEY_NAME = "player_record";

    [SerializeField]
    private Text scoreDisplay;
    [SerializeField]
    private Text bonusDisplay;

    AudioSource sound;

    int currentScore;
    int currentRecord;
    
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            sound.Play();
            currentScore = value;
            scoreDisplay.text = currentScore.ToString();
        }
    }

    public float Record
    {
        get
        {
            return currentRecord;
        }
    }

    #endregion

    #region Unity LifeCycle

    void Start ()
    {
        sound = GetComponent<AudioSource>();
        ResetScore();
    }

    #endregion

    #region Score Interact

    /// <summary>
    /// Обновление рекорда
    /// </summary>
    public void UpdateScore()
    {
        if (currentScore > currentRecord)
        {
            PlayerPrefs.SetInt(KEY_NAME, currentScore);
            currentRecord = currentScore;
        }
    }


    /// <summary>
    /// Сброс текущего количества очков
    /// </summary>
    public void ResetScore()
    {
        currentRecord = PlayerPrefs.GetInt(KEY_NAME);
        currentScore = 0;
        scoreDisplay.text = currentScore.ToString();
    }  
    

    /// <summary>
    /// Получение бонуса
    /// </summary>
    public void GetBonus()
    {
        Score++;
        StartCoroutine(ShowBonusFor3Sec());
    }


    /// <summary>
    /// Отображение бонус-оповещения на 3 секунды
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBonusFor3Sec()
    {
        bonusDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(SEC_FOR_WAIT);
        bonusDisplay.gameObject.SetActive(false);
    }

    #endregion
}
