using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SRPL.Debug
{
    public class DebugHudRenderer : MonoBehaviour
    {
        private Text text;

        void Start()
        {
            text = gameObject.AddComponent<Text>();
            text.text = "IT WORKS";
        }
        
        void Update()
        {

        }
    }
}