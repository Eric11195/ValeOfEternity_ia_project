using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace voe
{
    public class Logger : MonoBehaviour
    {
        [SerializeField]
        Transform scroll_pan_content;
        [SerializeField]
        CanvasGroup log_pannel;

        GameObject txt_prefab;
        GameObject h_prefab;
        private bool active = false;
        private bool autoscroll = false;

        Scrollbar my_scrollbar;

        private static Logger _instance = null;
        public static Logger get_instance()
        {
            return _instance;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Assert.IsTrue(_instance == null);
            _instance = this;

            txt_prefab = Resources.Load("Prefabs/ScrollPanText") as GameObject;
            h_prefab = Resources.Load("Prefabs/ScrollPanHeader") as GameObject;

            log_pannel = this.GetComponent<CanvasGroup>();
            my_scrollbar = GameObject.Find("LoggerScrollBar").GetComponent<Scrollbar>();

            autoscroll = true;
        }

        public static void Log(string s)
        {
            Log(s, Color.white);
        }
        public static void toggle_visibility()
        {
            var l = _instance;
            l.active = !l.active;

            if (l.active) {
                l.log_pannel.alpha = 1;
            }
            else
            {
                l.log_pannel.alpha = 0;
            }
        }
        public static void toggle_autoscroll()
        {
            _instance.autoscroll = !_instance.autoscroll;
        }

        public void FixedUpdate()
        {
            if (autoscroll)
            {
                my_scrollbar.value = 0;
            }
        }

        public static string player_log(int idx, string s)
        {
            return "Player "+(idx+1)+": "+s;
        }
        public static void Log(string s, Color c)
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;
        }
        public static void LogBold(string s)
        {
            LogBold(s, Color.white);
        }
        public static void LogBold(string s, Color c)
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;
            tmp.fontStyle = FontStyles.Bold;
        }
        public static void LogHeader(string s, Color c, int size)
        {
            GameObject obj = Instantiate(_instance.h_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;
            tmp.fontSize = size;
        }
        public static void LogH1(string s, Color c)
        {
            LogHeader(s, c, 48);
        }
        public static void LogH1(string s)
        {
            LogH1(s, Color.white);
        }
        public static void LogH2(string s, Color c)
        {
            LogHeader(s, c, 32);
        }
        public static void LogH2(string s)
        {
            LogH2(s, Color.white);
        }
        public static void LogH3(string s, Color c)
        {
            LogHeader(s, c, 28);
        }
        public static void LogH3(string s)
        {
            LogH3(s, Color.white);
        }
        public static void LogSpace()
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = "";
        }
    }

}