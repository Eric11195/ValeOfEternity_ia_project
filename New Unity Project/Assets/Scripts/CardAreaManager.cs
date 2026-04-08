using UnityEngine;

using System.Collections.Generic;

public class CardAreaManager : MonoBehaviour
{
    [SerializeField]
    protected Transform upper_left;
    [SerializeField]
    protected Transform down_right;
    [SerializeField]
    protected Vector2 card_size;

    protected int rows = 1;
    protected int cols = 100;

    private List<GameObject> card_list;

    private float max_left;
    private float min_left;
    private float max_up;
    private float min_down;

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
            float x_percentage = (i%cols)/(float)card_list.Count;
            float x_pos = Mathf.Lerp(min_left, max_left, x_percentage);

            float y_percentage = (i&rows)/(float)card_list.Count;
            float y_pos = Mathf.Lerp(min_down, max_up, y_percentage);

            GameObject obj = card_list[i];
            obj.transform.position = new Vector3(x_pos, y_pos, 0);
        }
    }
}
