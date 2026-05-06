using UnityEngine;
using UnityEngine.Assertions;
namespace voe{
    public class StoneRepresentator : MonoBehaviour
    {
        private static float delta=1.0f;
        private static int rows = 2;
        private static int cols = 3;
        private static float scale = 0.25f;
        private int current_idx = 0;
        public void set_stones(stone_quant sq){
            clear();
            draw_stones(sq);
        }
        void draw_stones(stone_quant sq){
            for(int j = 0; j < 3; ++j){
                for(int i = 0; i < sq.s[j];++i){
                    draw_stone((stone_type)j);
                }
            }
        }
        void draw_stone(stone_type st){
            GameObject childOb = new GameObject("stone");
            childOb.transform.SetParent(transform);
            childOb.AddComponent<StoneComponent>().set_up_type(st);
            Assert.IsTrue(current_idx/rows < cols);
            float x_delta = delta* (current_idx/rows);
            float y_delta = delta* (current_idx%rows);
            childOb.transform.position = transform.position + 
                x_delta*Vector3.right +
                y_delta*Vector3.down;
            childOb.transform.localScale = new Vector3(scale, scale, scale);

            ++current_idx;
        }
        void clear(){
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            current_idx=0;
        }
    }   
}