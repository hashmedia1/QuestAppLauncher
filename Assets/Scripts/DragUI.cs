using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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

        public Canvas canvas;
       

       public bool x;
       public bool y;
       public bool z;

       void Start(){

                     
       }

       public void Drag(){

            float newX= x? pointer.position.x : canvas.transform.position.x;
            float newY= y? pointer.position.y : canvas.transform.position.y;
            float newZ= z? pointer.position.z : canvas.transform.position.z;
            canvas.transform.position= new Vector3(newX,newY,newZ);
            




            transform.LookAt(mainCamera.transform.position);
            transform.Rotate(0,-180,0) ;
       }
    }
}