using UnityEngine;
using UnityEngine.Assertions;

namespace voe{

    public class stone_cuant{
        //stones
        public int[] s = new int[3];
        public stone_cuant(int _one, int _three, int _six){
            s[(int)stone_type.ST_one] =_one;
            s[(int)stone_type.ST_three] = _three;
            s[(int)stone_type.ST_six] = _six;
        }
    }

    public enum stone_type{
        ST_one=0,
        ST_three=1,
        ST_six=2
    }

    public class StoneManager
    {

        public int max_stones = 4;
        //stone amounts
        public stone_cuant sa = new stone_cuant(0,0,0);
        //stone values
        public stone_cuant sv = new stone_cuant(1,3,6);

        public int get_total_value(){
            return get_value(sa);
        }
        public int get_number_of_stones(){
            return sa.s[(int)stone_type.ST_one] + 
                sa.s[(int)stone_type.ST_three] + 
                sa.s[(int)stone_type.ST_six];
        }
        public int get_number_of_spaces_to_fill(){
            return max_stones - get_number_of_stones();
        }

        public int get_value(stone_cuant q){
            return sv.s[(int)stone_type.ST_one]*q.s[(int)stone_type.ST_one] + 
                sv.s[(int)stone_type.ST_three]*q.s[(int)stone_type.ST_three] + 
                sv.s[(int)stone_type.ST_six]*q.s[(int)stone_type.ST_six]
            ;
        }
        public int get_value(stone_type q){
            return sv.s[(int)q];
        }

        public bool check_valid_payment(stone_cuant q, int cost){
            int total_paid = get_value(q);
            if(cost <= total_paid) return false;
            
            int delta = total_paid - cost;
            if(delta >= sv.s[(int)stone_type.ST_one] && q.s[(int)stone_type.ST_one] > 0) return false;
            if(delta >= sv.s[(int)stone_type.ST_three] && q.s[(int)stone_type.ST_three] > 0) return false;
            if(delta >= sv.s[(int)stone_type.ST_six] && q.s[(int)stone_type.ST_six] > 0) return false;

            return true;
        }

        public stone_type get_highest_cost_stone(){
            stone_type type = stone_type.ST_six;
            while(sa.s[(int)type] == 0) --type;
            return type;
        }
        public stone_type extract_highest_cost_stone(){
            stone_type type = get_highest_cost_stone();
            add_stones(type, -1);
            return type;
        }

        public void add_stones(stone_type st, int quant_to_add){
            sa.s[(int)st] += quant_to_add;

            //Checks
            int stone_number = get_number_of_stones();
            Assert.IsTrue(stone_number <= max_stones && stone_number > 0);
        }

        public stone_type get_stone_from_value(int value){
            if(sv.s[(int)stone_type.ST_one]==value) return stone_type.ST_one;
            else if(sv.s[(int)stone_type.ST_three]==value) return stone_type.ST_three;
            else if(sv.s[(int)stone_type.ST_six]==value) return stone_type.ST_six;
            else{
                throw new UnityException("The other thing is null");
            }
        }

        public void add_stones(int stone_cuant, int quant_to_add){
            add_stones(
                get_stone_from_value(stone_cuant),
                quant_to_add
            );
        }
    }
}