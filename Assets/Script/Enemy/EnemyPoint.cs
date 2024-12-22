using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    private UI_BossStatus uibossStatus;
    private bool isActiveAlter;
    private bool isClear;
    public List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> firstEnemies = new List<Enemy>();
    public bool isAmbushActive;
    public List<Enemy> hideEnemies = new List<Enemy>();
    public bool isAmbush2Active;
    public List<Enemy> hideEnemies2 = new List<Enemy>();
    public bool isBoss;
    public GameObject activeEvent;
    public ParticleSystem eventFX;

    private void Start()
    {
        foreach (Enemy enemy in hideEnemies)
        {
            enemy.gameObject.SetActive(false); 
        }
        foreach (Enemy enemy in hideEnemies2)
        {
            enemy.gameObject.SetActive(false);
        }
        isAmbushActive = false;
        isAmbush2Active = false;
        firstEnemies = GetComponentsInChildren<Enemy>().ToList();
    }

    private void LateUpdate()
    {
        ActiveAlter();
        Ambush();
        Ambush2();
        CheckAreClear();
    }

    private void ActiveAlter()
    {
        if (isActiveAlter) { return; }
        if (firstEnemies.Where(x => x.IsAlerting).ToList().Count() > 0)
        {
            foreach (Enemy enemy in firstEnemies)
            {
                if (enemy.gameObject.activeSelf)
                { enemy.IsAlerting = true; }
                if (enemy.Data.isBoos)
                {
                    uibossStatus = enemy.uiBossStatus.SetUIStateActive(true, enemy);
                }
            }
            isActiveAlter = true;
            foreach (Enemy enemy in hideEnemies)
            {
                enemy.gameObject.SetActive(true);
            }
            foreach (Enemy enemy in hideEnemies2)
            {
                enemy.gameObject.SetActive(true);
            }
            enemies = GetComponentsInChildren<Enemy>().ToList();
        }
    }

    private void Ambush()
    {
        if (!isActiveAlter || isClear || isAmbushActive) { return; }
        if (firstEnemies.Where(x => x != null).ToList().Count() <= 1)
        {
            foreach (Enemy enemy in hideEnemies)
            {
                enemy.IsAlerting = true;
                enemy.IsAmbushDash = true;
            }
            isAmbushActive = true;
        }
    }
    private void Ambush2()
    {
        if (!isActiveAlter || isClear || isAmbush2Active) { return; }
        if (isAmbushActive && 
            (firstEnemies.Where(x => x != null).ToList().Count() + hideEnemies.Where(x => x != null).ToList().Count()) <= 1)
        {
            foreach (Enemy enemy in hideEnemies2)
            {
                enemy.IsAlerting = true;
                enemy.IsAmbushDash = true;
            }
            isAmbush2Active = true;
        }
    }

    private void CheckAreClear()
    {
        if (!isActiveAlter || isClear) { return; }
        if (enemies.Where(x => x != null).ToList().Count() == 0)
        {
            isClear = true;
            if (uibossStatus != null)
            {
                uibossStatus.SetUIStateActive(false);
            }
            if (activeEvent != null)
            {
                activeEvent.SetActive(true);
            }
            if (eventFX != null)
            {
                eventFX.Stop();
            }
        }
    }
}
