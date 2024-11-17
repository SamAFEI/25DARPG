using UnityEngine;

public class ForestEvent01 : MonoBehaviour
{
    public Collider collider => GetComponent<Collider>();
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.StartCoroutine(enemy.DoBreakAndDash());
            collider.enabled = false;
        }
    }
}
