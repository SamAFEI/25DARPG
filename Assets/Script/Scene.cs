using System.Linq;
using UnityEngine;

public class Scene : MonoBehaviour
{
    int[] numbers = { 7, 99, 5, 66, 4 };
    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log("----Group()----");
        Group();
        Debug.Log("----Where()----");
        Where();*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Group()
    {
        /*var query = from n in numbers
                    group n by n % 2 != 0 into odd
                    select odd;*/
        var query = numbers.GroupBy(n => n % 2 != 0);
        foreach (var item in query)
        {
            Debug.Log(item.Key); //Group 分組 依是否為奇數 True False 分為兩組
            foreach (var number in item)
            {
                Debug.Log(number); //組別的內容
            }
        }
    }

    private void Where()
    {
        /*var query = from n in numbers
                    where n % 2 != 0
                    select n;*/
        var query = numbers.Where(n => n % 2 != 0);
        //只select 奇數
        foreach (var item in query)
        {
            Debug.Log(item);
        }
    }
}
