using UnityEngine;

public class ViewControl : MonoBehaviour
{
    #region Fields

    const float MOVE_POS_SPEED = 3f;
    const float MOVE_SCALE_SPEED = 0.95f;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject game;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject gameOverPanel;

    GameLogic gameLogicScript;
    GameOverPanel gameOverScript;

    Vector3 menuPos = new Vector3(-2.33f, 0, -1);
    Vector3 gamePos = new Vector3(0, 0, -1);

    Vector3 gameScale = new Vector3(1, 1, 1);
    Vector3 menuScale = new Vector3(1.4f, 1.4f, 1.4f);

    bool hasGameStarted = false;

    #endregion

    #region Unity LifeCycle

    void Start ()
    {
		cam.transform.localPosition = menuPos;
        gameLogicScript = game.GetComponent<GameLogic>();
        gameOverScript = gameOverPanel.GetComponent<GameOverPanel>();
    }

	
	void FixedUpdate()
    {
        if (hasGameStarted)
        {
            if (cam.transform.localPosition != gamePos)
            {
                cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, 
                    gamePos, MOVE_POS_SPEED * Time.deltaTime);
            }
            if (game.transform.localScale != gameScale)
            {
                game.transform.localScale = Vector3.MoveTowards(game.transform.localScale, 
                    gameScale, MOVE_SCALE_SPEED * Time.deltaTime);
            }
        }
	}

    #endregion

    #region View Interact

    /// <summary>
    /// Переход к игре
    /// </summary>
    public void SetGameMode()
    {
        hasGameStarted = true;
        gameLogicScript.StartScene();
        mainMenu.gameObject.SetActive(false);
    }


    /// <summary>
    /// Переход к главному меню
    /// </summary>
    public void SetDefault()
    {
        gameLogicScript.RestartScene(false);

        hasGameStarted = false;

        cam.transform.localPosition = menuPos;
        game.transform.localScale = menuScale;

        gameOverScript.Hide();
        mainMenu.gameObject.SetActive(true);
    }

    #endregion
}
