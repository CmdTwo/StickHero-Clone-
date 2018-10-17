using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    #region Fields / Properties

    const float POS_MIN = 50f;
    const float POS_MAX = 230f;

    [SerializeField]
    private GameObject towerPrefab;

    List<GameObject> towers;

    public List<GameObject> GetTowers
    {
        get
        {
            return towers;
        }
    }

    #endregion

    #region Unity LifeCycle

    void Start ()
    {
        towers = new List<GameObject>();
    }

    #endregion

    #region Manager Interact

    /// <summary>
    /// Генерация одной башни.
    /// В случае наличия других башен - добавляется отступ от последней
    /// </summary>
    public void GenerateTower()
    {
        GameObject tower = Instantiate(towerPrefab, transform);
        TowerInteract interactScript = tower.GetComponent<TowerInteract>();
        if (towers.Count != 0)
        {
            float newSize = Random.Range(50, 200);
            interactScript.UpdateSize(newSize);
            tower.transform.localPosition = new Vector3(towers[towers.Count - 1].transform.localPosition.x + 
                Random.Range(newSize / 2 + POS_MIN, POS_MAX) + newSize, tower.transform.localPosition.y, 0);
        }
        else
        {
            interactScript.SetActiveBonus(false);
        }
        towers.Add(tower);
    }


    /// <summary>
    /// Удаление башни по индексу
    /// </summary>
    /// <param name="index"></param>
    public void RemoveTowerAt(int index)
    {
        GameObject tower = towers[index];
        Destroy(tower);
        towers.Remove(tower);
    }


    /// <summary>
    /// Удаление всех башен
    /// </summary>
    public void RemoveAllTowers()
    {
        foreach(GameObject tower in towers)
        {
            Destroy(tower);
        }
        towers.Clear();
    }

    #endregion
}
