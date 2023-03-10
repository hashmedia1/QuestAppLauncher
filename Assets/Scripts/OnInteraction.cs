using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace QuestAppLauncher
{
    public class OnInteraction : MonoBehaviour
    {
        // Hide app handler
        public HideAppHandler hideAppHandler;
        public GameObject scrollContainer;
        public GameObject downloadStatus;
        public GameObject topTabs;
        public GameObject leftTabs;
        public GameObject rightTabs;
        public GameObject topBar;
        public GameObject canvas;
        public Camera mainCamera;
        // Rename app handler
        public RenameHandler renameHandler;
        public ChangeNameHandler changeNameHandler;

        public void OnHoverEnter(Transform t)
        {
            var appEntry = t.gameObject.GetComponent("AppEntry") as AppEntry;
            if (null != appEntry)
            {
                // Enable border
                EnableBorder(t, true);
            }
        }

        public void OnHoverExit(Transform t)
        {
            var appEntry = t.gameObject.GetComponent("AppEntry") as AppEntry;
            if (null != appEntry)
            {
                // Disable border
                EnableBorder(t, false);
            }
        }

        public void Minimize()
        {
            scrollContainer.SetActive(false);
            downloadStatus.SetActive(false);
            topTabs.SetActive(false);
            leftTabs.SetActive(false);
            rightTabs.SetActive(false);
            topBar.SetActive(false);
        }
        public void Maximize()
        {
            scrollContainer.SetActive(true);
            downloadStatus.SetActive(true);
            topTabs.SetActive(true);
            leftTabs.SetActive(true);
            rightTabs.SetActive(true);
            topBar.SetActive(true);
        }

        public async void OnSelected(Transform t)
        {
            var appEntry = t.gameObject.GetComponent("AppEntry") as AppEntry;
            if (null != appEntry)
            {
                if (appEntry.isRenameMode)
                {
                    this.renameHandler.Rename(appEntry);
                }
                else
                {
                    await Task.Run(() =>
                    {
                        AndroidJNI.AttachCurrentThread();

                        try
                        {
                            // Launch app
                            Debug.Log("Launching: " + appEntry.appName + " (package id: " + appEntry.packageId + ")");
                            AppProcessor.LaunchApp(appEntry.packageId);
                        }
                        finally
                        {
                            AndroidJNI.DetachCurrentThread();
                        }
                    });
                }
            }
        }

        public void OnSelectedPressedBorY(Transform t)
        {
            // var appEntry = t.gameObject.GetComponent("AppEntry") as AppEntry;
            // if (null != appEntry && !appEntry.isRenameMode)
            // {
            //     this.hideAppHandler.OnHideApp(appEntry);
            // }

                canvas.transform.position=new Vector3(1.2f,2.4f,7.3f);

               canvas.transform.rotation=  Quaternion.Euler(0f,7.59f,0f);
               // transform.LookAt(mainCamera.transform.position);
               // transform.Rotate(0,-180,0) ;

        }

        public void OnAddApplication(Transform t)
        {
           
                this.renameHandler.OpenRenamePanel();
            

        }

        public void OnSelectedPressedAorX(Transform t)
        {
            var appEntry = t.gameObject.GetComponent("AppEntry") as AppEntry;
            if (null != appEntry && !appEntry.isRenameMode)
            {
                this.changeNameHandler.OpenRenamePanel(appEntry);
            }
        }

        void EnableBorder(Transform t, bool enable)
        {
            var border = t.Find("Border");
            border?.gameObject.SetActive(enable);
        }
    }
}