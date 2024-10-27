using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BT_Start : MonoBehaviour
{
    public Button bt_start { get; private set; }
    public TextMeshProUGUI tmp_Start { get; private set; }

    private void Awake()
    {
        bt_start = GetComponent<Button>();
        bt_start.onClick.AddListener(() => GameManager.Instance.LoadScene("DemoScene7"));
        tmp_Start = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadScene("DemoScene7");
        }
    }

    private void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            for (float alpha = 0.1f; alpha < 1f; alpha += Time.deltaTime)
            {
                tmp_Start.color = new Color(tmp_Start.color.r, tmp_Start.color.g, tmp_Start.color.b, alpha);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            for (float alpha = 1f; alpha > 0.1f; alpha -= Time.deltaTime)
            {
                tmp_Start.color = new Color(tmp_Start.color.r, tmp_Start.color.g, tmp_Start.color.b, alpha);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
