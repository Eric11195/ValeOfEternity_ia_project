using NUnit.Framework;
using UnityEngine;
using TMPro;

namespace voe
{
    public class Logger : MonoBehaviour
    {
        [SerializeField]
        Transform scroll_pan_content;
        GameObject txt_prefab;
        GameObject h_prefab;

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
        }

        public static void Log(string s)
        {
            Log(s, Color.white);
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