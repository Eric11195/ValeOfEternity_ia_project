using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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

        private static List<TextFilter> spawned_text_list;
        private static List<Toggle> filter_togglers;

        private static TextFilter.message_src current_filter;

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
            
            spawned_text_list = new List<TextFilter>(0);
            filter_togglers = new List<Toggle>(0);
            for(int i = 1; i <= 4; ++i)
            {
                filter_togglers.Add(GameObject.Find("MsgToggleP"+i).GetComponent<Toggle>());
            }
            filter_togglers.Add(GameObject.Find("MsgToggleSystem").GetComponent<Toggle>());
            SetFilter();
        }

        public static void Log(string s, TextFilter.message_src src)
        {
            Log(s, Color.white, src);
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
        public static void Log(string s, Color c, TextFilter.message_src src)
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;

            var tf = obj.AddComponent<TextFilter>();
            tf.set_src(src);
            tf.enforce_current_filter(current_filter);
            spawned_text_list.Add(tf);
        }
        public static void LogBold(string s, TextFilter.message_src src)
        {
            LogBold(s, Color.white, src);
        }
        public static void LogBold(string s, Color c, TextFilter.message_src src)
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;
            tmp.fontStyle = FontStyles.Bold; 

            var tf = obj.AddComponent<TextFilter>();
            tf.set_src(src);
            tf.enforce_current_filter(current_filter);
            spawned_text_list.Add(tf);
        }
        public static void LogHeader(string s, Color c, int size, TextFilter.message_src src)
        {
            GameObject obj = Instantiate(_instance.h_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = s;
            tmp.color = c;
            tmp.fontSize = size;

            var tf = obj.AddComponent<TextFilter>();
            tf.set_src(src);
            tf.enforce_current_filter(current_filter);
            spawned_text_list.Add(tf);
        }
        public static void LogH1(string s, Color c, TextFilter.message_src src)
        {
            LogHeader(s, c, 48,src);
        }
        public static void LogH1(string s, TextFilter.message_src src)
        {
            LogH1(s, Color.white,src);
        }
        public static void LogH2(string s, Color c, TextFilter.message_src src)
        {
            LogHeader(s, c, 32,src);
        }
        public static void LogH2(string s, TextFilter.message_src src)
        {
            LogH2(s, Color.white,src);
        }
        public static void LogH3(string s, Color c, TextFilter.message_src src)
        {
            LogHeader(s, c, 28,src);
        }
        public static void LogH3(string s, TextFilter.message_src src)
        {
            LogH3(s, Color.white,src);
        }
        public static void LogSpace()
        {
            GameObject obj = Instantiate(_instance.txt_prefab, _instance.scroll_pan_content);
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = "";
        }

        public static void SetFilter()
        {
            TextFilter.message_src src = (TextFilter.message_src)0;
            for (int i = 0; i < filter_togglers.Count; i++)
            {
                if (filter_togglers[i].isOn)
                    src = (TextFilter.message_src) ((int)src | (1 << i));
            }
            current_filter = src;
            SetFilter(src);
        }
        private static void SetFilter(TextFilter.message_src filter)
        {
            foreach(TextFilter tf in spawned_text_list)
            {
                tf.enforce_current_filter(filter);
            }
        }
    }

}