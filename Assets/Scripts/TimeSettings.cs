using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace QuestAppLauncher
{

    public class TimeSettings : MonoBehaviour
    {

    private TextMeshProUGUI textGui;

       void Start()
       {
            this.textGui = GetComponent<TextMeshProUGUI>();
       }
       void Update()
       {
            DateTime dt = DateTime.Now;

          //  this.textGui.text = string.Format("{0}", dt.ToString("h:mm tt"));
            
       }
    }

}