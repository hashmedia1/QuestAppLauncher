using UnityEngine;
using System.Collections;

namespace QuestAppLauncher
{
    /// <summary>
    /// Simple class that holds state that survives scene reloads.
    /// Follows Unity's DontDestroyOnLoad singleton pattern.
    /// </summary>
    public class DragUI : MonoBehaviour
    {
       public Transform pointer;
       public Camera mainCamera;

       public bool x;
       public bool y;
       public bool z;

       public void Drag(){

            float newX= x? pointer.position.x : transform.position.x;
            float newY= y? pointer.position.y : transform.position.y;
            float newZ= z? pointer.position.z : transform.position.z;
            transform.position= new Vector3(newX,newY,newZ);
            




            transform.LookAt(mainCamera.transform.position);
            transform.Rotate(0,-180,0) ;
       }
    }
}