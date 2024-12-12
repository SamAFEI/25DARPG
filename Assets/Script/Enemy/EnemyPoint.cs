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
    public float ambushTime = 0f;
    public List<Enemy> hideEnemies = new List<Enemy>();
    public float ambush2Time = 0f;
    public List<Enemy> hideEnemies2 = new List<Enemy>();
    public bool isBoss;
    public GameObject activeEvent;
    public ParticleSystem eventFX;

    private void Start()
    {
        enemies = GetComponentsInChildren<Enemy>().ToList();
        foreach (Enemy enemy in hideEnemies)
        {
            enemy.gameObject.SetActive(false); 
        }
        foreach (Enemy enemy in hideEnemies2)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        ActiveAlter();
        CheckAreClear();
    }

    private void ActiveAlter()
    {
        if (isActiveAlter) { return; }
        if (enemies.Where(x => x.IsAlerting).ToList().Count() > 0)
        {
            StartCoroutine(Ambush());
            StartCoroutine(Ambush2());
            foreach (Enemy enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                { enemy.IsAlerting = true; }
                if (enemy.Data.isBoos)
                {
                    uibossStatus = enemy.uiBossStatus.SetUIStateActive(true, enemy);
                }
            }
            isActiveAlter = true;
        }
    }

    private IEnumerator Ambush()
    {
        yield return new WaitForSeconds(ambushTime);
        foreach (Enemy enemy in hideEnemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.IsAlerting = true;
        }
    }
    private IEnumerator Ambush2()
    {
        yield return new WaitForSeconds(ambush2Time);
        foreach (Enemy enemy in hideEnemies2)
        {
            enemy.gameObject.SetActive(true);
            enemy.IsAlerting = true;
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
