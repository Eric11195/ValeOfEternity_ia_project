using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;

using voe;
using System.Runtime.Versioning;

public class CardAreaManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject card_prefab;


    [SerializeField]
    protected Transform upper_left;
    [SerializeField]
    protected Transform down_right;
    static protected Vector2 card_size = new Vector2(1.8f,2.5f);
    [SerializeField]
    protected int rows = 1;
    [SerializeField]
    protected int cols = 100;

    private List<GameObject> card_list;

    private float max_left;
    private float min_left;
    private float max_up;
    private float min_down;

    void Awake(){
        card_prefab = Resources.Load("Prefabs/Card") as GameObject;
        Assert.IsTrue(card_prefab);
    
        card_list = new List<GameObject>(0);
    }

    void Start()
    {
        float half_cx = card_size.x/2;
        float half_cy = card_size.y/2;

        max_left = upper_left.position.x - half_cx;
        max_up = upper_left.position.y - half_cy;

        min_left = down_right.position.x + half_cx;
        min_down = down_right.position.y + half_cy;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < card_list.Count; ++i)
        {
                      
            int x_index = i%cols;
            int y_index = i/cols;

            float x_percentage = 0.5f;
            if(cols>1)
                x_percentage = x_index/(float)(cols-1);
            float x_pos = Mathf.Lerp(min_left, max_left, x_percentage);

            float y_percentage = 0.5f;
            if(rows>1)
                y_percentage = y_index/(float)(rows-1);
            float y_pos = Mathf.Lerp(min_down, max_up, y_percentage);

            GameObject obj = card_list[i];
            if(!obj) continue;
            //if(x_pos==x_pos && y_pos==y_pos)
                obj.transform.position = 
                    new Vector3(x_pos, y_pos, 0);
        }
    }

    public GameObject add(CardNameId cni){
        GameObject go = Instantiate(card_prefab, this.transform);
        go.GetComponent<CardComponent>().set_card(cni);
        card_list.Add(go);
        return go;
    }

    public void remove(GameObject go)
    {
        //CardNameId cni = go.GetComponent<CardComponent>().get_card_id();
        card_list.Remove(go);
        Destroy(go);
    }
    public void remove(CardNameId cni){
        GameObject card_representation = card_list.Find(x => cni==x.GetComponent<CardComponent>().get_card_id());
        Assert.IsTrue(card_representation != null);
        card_list.Remove(card_representation);
        Destroy(card_representation);
    }

    public void empty()
    {
        foreach(GameObject go in card_list)
        {
            Destroy(go);
        }
        card_list.Clear();
    }
}
