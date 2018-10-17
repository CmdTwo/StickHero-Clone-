using UnityEngine;

public class TowerInteract : MonoBehaviour
{
    #region Fields / Properties

    [SerializeField]
    private GameObject bonusCube;

    private RectTransform towerRect;
    private RectTransform bonusRect;

    public RectTransform GetRect
    {
        get
        {
            return towerRect;
        }
    }

    #endregion

    #region Unity LifeCycle

    void Awake()
    {
        towerRect = transform.GetComponent<RectTransform>();
        bonusRect = bonusCube.GetComponent<RectTransform>();
    }

    #endregion

    #region Tower Interact

    /// <summary>
    /// Проверка наличия палки на башне
    /// </summary>
    /// <param name="stick"></param>
    /// <param name="stickReck"></param>
    /// <returns></returns>
    public bool IsStickOnTower(GameObject stick, RectTransform stickReck)
    {
        Vector3 stickEnd = new Vector3(stickReck.sizeDelta.y, 0, 0) + 
            stick.transform.localPosition;

        float offset = transform.localPosition.x - stickEnd.x;
        float sideWidht = towerRect.sizeDelta.x / 2;

        return offset > 0 ? sideWidht - offset >= 0 : sideWidht + offset >= 0;
    }
    

    /// <summary>
    /// Проверка на наличие бонуса
    /// </summary>
    /// <param name="stick"></param>
    /// <param name="stickReck"></param>
    /// <returns></returns>
    public bool HasBonus(GameObject stick, RectTransform stickReck)
    {
        Vector3 stickEnd = new Vector3(stickReck.sizeDelta.y, 0, 0) + 
            stick.transform.localPosition;

        float offset = transform.localPosition.x - stickEnd.x;
        float sideWidht = bonusRect.sizeDelta.x / 2;

        return offset > 0 ? sideWidht - offset >= 0 : sideWidht + offset >= 0;
    }


    /// <summary>
    /// Обновление размера башни
    /// </summary>
    /// <param name="newWidth"></param>
    public void UpdateSize(float newWidth)
    {
        towerRect.sizeDelta = new Vector2(newWidth, towerRect.sizeDelta.y);
    }


    /// <summary>
    /// Установка режима отображения бонуса
    /// </summary>
    /// <param name="status"></param>
    public void SetActiveBonus(bool status)
    {
        bonusCube.SetActive(status);
    }


    /// <summary>
    /// Получение координат право-верхнего угла башни
    /// </summary>
    /// <returns></returns>
    public float GetRightEdgeX()
    {
        return towerRect.sizeDelta.x / 2 + transform.localPosition.x;
    }

    #endregion
}
