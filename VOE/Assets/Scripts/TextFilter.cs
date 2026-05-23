using NUnit.Framework;
using UnityEngine;

namespace voe {
    public class TextFilter : MonoBehaviour
    {
        public enum message_src
        {
            p1 = 1 << 0,
            p2 = 1 << 1,
            p3 = 1 << 2,
            p4 = 1 << 3,
            general = 1 << 4
        }
        public static message_src get_p_idx_message_src(int idx)
        {
            Assert.IsTrue(idx < 4);
            return (message_src)(1 << idx);
        }

        message_src src;
        public void set_src(message_src _src)
        {
            src = _src;
        }
        public void enforce_current_filter(message_src filter)
        {
            if((filter & src) == 0)
            {
                hide();
            }
            else
            {
                show();
            }
        }
        private void hide()
        {
            gameObject.SetActive(false);
        }
        private void show()
        {

            gameObject.SetActive(true);
        }
    }
}