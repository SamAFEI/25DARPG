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
            Debug.Log(item.Key); //Group ���� �̬O�_���_�� True False �������
            foreach (var number in item)
            {
                Debug.Log(number); //�էO�����e
            }
        }
    }

    private void Where()
    {
        /*var query = from n in numbers
                    where n % 2 != 0
                    select n;*/
        var query = numbers.Where(n => n % 2 != 0);
        //�uselect �_��
        foreach (var item in query)
        {
            Debug.Log(item);
        }
    }
}
