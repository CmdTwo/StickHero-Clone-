using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    #region Fields

    const int MID_POS = 1;
    const float PLAYER_SIDE = 10f;
    const float OFFSET = 0.1f;
    const float DOWN_PLAYER = -50f;
    const float MOVE_PLAYED_SPEED = 400f;
    const float MOVE_SCENE_SPEED = 800f;
    const float FALL_DOWN_SPEED = 1300f;

    [SerializeField]
    private GameObject stickPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private TowerManager towersContol;
    [SerializeField]
    private GameObject scoreControl;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject scoreDisplay;

    GameObject stick;
    GameObject player;

    StickLogic stickScript;
    TowerInteract towerScript;
    GameScore scoreScript;
    GameOverPanel gameOverScript;

    RectTransform playerRect;

    Vector3 nextPlayerPosition;
    List<Vector3> towersPos;

    float moveValue;

    bool isStickFallDown;
    bool isStickOnTower;
    bool hasPlayerMoved;
    bool isGameOver;
    bool hasPlayerFallDown;
    bool isMoveSceneTime;
    bool isPlayableMode;

    #endregion

    #region Unity LifeCycle

    void Start()
    {
        towersPos = new List<Vector3>();

        nextPlayerPosition = Vector3.zero;        
        moveValue = 0;

        isStickFallDown = false;
        isStickOnTower = false;
        hasPlayerMoved = false;
        isGameOver = false;
        hasPlayerFallDown = false;
        isMoveSceneTime = false;
        isPlayableMode = true;

        CreateInstances(true);
    }


    void FixedUpdate()
    {
        if (isPlayableMode && !isGameOver)
        {
            MoveInstances();
            MoveScene();
            if (hasPlayerMoved && hasPlayerFallDown)
            {
                GameOver();
            }
        }
    }

    #endregion

    #region Game Logic

    /// <summary>
    /// Спавн и инициализаци объектов 
    /// </summary>
    /// <param name="isDisplay"></param>
    void CreateInstances(bool isDisplay = false)
    {
        stick = Instantiate(stickPrefab, transform);
        stickScript = stick.GetComponent<StickLogic>();
        stick.SetActive(!isDisplay);

        player = Instantiate(playerPrefab, transform);
        playerRect = player.GetComponent<RectTransform>();
        towersContol.GenerateTower();
    }

    
    /// <summary>
    /// Удаление и разрушение объектов
    /// </summary>
    void DestroyInstances()
    {
        Destroy(player);
        Destroy(stick);
        towersContol.RemoveAllTowers();
    }

    
    /// <summary>
    /// Обработка события падение палки
    /// </summary>
    void GameLogic_OnStickFallDown()
    {      
        isStickFallDown = true;

        towerScript = towersContol.GetTowers[MID_POS].GetComponent<TowerInteract>();
        isStickOnTower = towerScript.IsStickOnTower(stick, stickScript.GetRect);

        Vector3 playerPos = player.transform.localPosition;

        if (isStickOnTower)
        {
            Debug.Log("Stick fall down on tower");
            float towerRightEdgeX = towerScript.GetRightEdgeX();
            nextPlayerPosition = new Vector3((towerRightEdgeX - PLAYER_SIDE) -
                playerRect.sizeDelta.x / 2, playerPos.y);
        }
        else
        {
            Debug.Log("Stick fall down");
            nextPlayerPosition = new Vector3(stickScript.GetRect.sizeDelta.y +
                stick.transform.localPosition.x + PLAYER_SIDE, playerPos.y);
        }
    }


    /// <summary>
    /// Расчет и передвижение игрока с палкой
    /// </summary>
    void MoveInstances()
    {
        if (!isMoveSceneTime)
        {
            if (isStickFallDown && !hasPlayerMoved)
            {
                float moveSpeed = hasPlayerFallDown ? FALL_DOWN_SPEED : MOVE_PLAYED_SPEED;

                player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition,
                    nextPlayerPosition, moveSpeed * Time.deltaTime);

                if (player.transform.localPosition == nextPlayerPosition)
                    hasPlayerMoved = true;
            }
            if (hasPlayerMoved && isStickOnTower)
            {
                if (scoreScript == null)
                    scoreScript = scoreControl.GetComponent<GameScore>();
                scoreScript.Score++;

                if (towerScript.HasBonus(stick, stickScript.GetRect))
                {
                    Debug.Log("Player get bonus");
                    scoreScript.GetBonus();
                }

                moveValue = towersContol.GetTowers[MID_POS].transform.localPosition.x -
                    towerScript.GetRect.sizeDelta.x / 2;

                nextPlayerPosition = new Vector3(player.transform.localPosition.x -
                    moveValue, player.transform.localPosition.y);

                towersContol.GenerateTower();

                foreach (GameObject tower in towersContol.GetTowers)
                {
                    towersPos.Add(new Vector3(tower.transform.localPosition.x -
                        moveValue, tower.transform.localPosition.y));
                }

                isStickFallDown = false;
                isStickOnTower = false;
                hasPlayerMoved = false;
                isMoveSceneTime = true;
            }
            else if (hasPlayerMoved && !hasPlayerFallDown)
            {
                nextPlayerPosition = new Vector3(player.transform.localPosition.x, DOWN_PLAYER);
                hasPlayerMoved = false;
                hasPlayerFallDown = true;
            }
        }
    }


    /// <summary>
    /// Передвижение сцены и объектов
    /// </summary>
    void MoveScene()
    {
        if (isMoveSceneTime)
        {
            List<GameObject> towers = towersContol.GetTowers;
            if (towers.Count != 0)
            {
                if (Vector3.Distance(towers[0].transform.localPosition, towersPos[0]) > OFFSET)
                {
                    Vector3 targetPos;
                    for (int i = 0; i != towers.Count; i++)
                    {
                        targetPos = new Vector3(towersPos[i].x, towersPos[i].y);
                        towers[i].transform.localPosition = Vector3.MoveTowards(towers[i].transform.localPosition,
                            targetPos, MOVE_SCENE_SPEED * Time.deltaTime);
                    }
                }
            }

            player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition,
                nextPlayerPosition, MOVE_SCENE_SPEED * Time.deltaTime);

            stick.transform.localPosition = Vector3.MoveTowards(stick.transform.localPosition,
                new Vector3(stick.transform.localPosition.x - moveValue, stick.transform.localPosition.y),
                MOVE_SCENE_SPEED * Time.deltaTime);

            if (player.transform.localPosition == nextPlayerPosition)
            {
                stick.transform.localPosition =
                    new Vector3(towerScript.GetRightEdgeX(), stick.transform.localPosition.y);

                stickScript.ResetStick();

                towersContol.RemoveTowerAt(0);
                towersContol.GetTowers[0].GetComponent<TowerInteract>().SetActiveBonus(false);
                towersPos.Clear();

                isMoveSceneTime = false;
            }
        }
    }
    

    /// <summary>
    /// Установка режима конца игры
    /// </summary>
    void GameOver()
    {
        Debug.Log("Game Over");

        if (gameOverScript == null)
            gameOverScript = gameOverPanel.GetComponent<GameOverPanel>();
        gameOverScript.Show();

        isGameOver = true;
        scoreDisplay.SetActive(false);
    }

    #endregion

    #region Game Interact

    /// <summary>
    /// Запуск игровой сцены
    /// </summary>
    public void StartScene()
    {
        Debug.Log("Game started");

        stick.SetActive(true);
        scoreDisplay.SetActive(true);

        isPlayableMode = true;
        stickScript.OnStickFallDown += GameLogic_OnStickFallDown;

        towersContol.GenerateTower();
    }

    
    /// <summary>
    /// Сброс текущей игровой сцены
    /// </summary>
    /// <param name="isContinueGame"></param>
    public void RestartScene(bool isContinueGame = true)
    {
        Debug.Log("Scene restarted");

        gameOverScript.Hide();

        isPlayableMode = isContinueGame;

        stickScript.OnStickFallDown -= GameLogic_OnStickFallDown;

        DestroyInstances();
        CreateInstances(!isContinueGame);

        isStickFallDown = false;
        isStickOnTower = false;
        hasPlayerMoved = false;
        isGameOver = false;
        hasPlayerFallDown = false;
        isMoveSceneTime = false;

        if (isPlayableMode)
        {
            towersContol.GenerateTower();
            stickScript.OnStickFallDown += GameLogic_OnStickFallDown;
            scoreDisplay.SetActive(true);
        }
    }

    #endregion
}