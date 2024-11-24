using UnityEngine;

public class ForestEvent01 : MonoBehaviour
{
    public Collider collider => GetComponent<Collider>();
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (enemy == null || !enemy.isActiveAndEnabled) { return; }
            enemy.StartCoroutine(enemy.DoBreakAndDash());
            collider.enabled = false;
        }
    }
}
