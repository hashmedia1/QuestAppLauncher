using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QuestAppLauncher
{
    public class ChangeNameHandler : MonoBehaviour
    {
        // Referenced gameobjects
        public GameObject panelContainer;
        public GameObject closeRenameButton;
        public GameObject renamePanelContainer;
        public GameObject renameButton;
        public GameObject errorLabel;
       
        public TextMeshProUGUI renameLabel;
        public InputField inputField;

        // Whether the rename grid has already been populated
        private bool isPopulated = false;

        // App entry of the app to rename
        private AppEntry appToRename;

        /// <summary>
        /// Opens the rename panel.
        /// </summary>
        /// <param name="appToRename">App to rename</param>
        public async void OpenRenamePanel(AppEntry appToRename)
        {
            Debug.Log("Open Rename Panel");
            this.panelContainer.SetActive(false);
            this.renamePanelContainer.SetActive(true);

            this.errorLabel.SetActive(false);

            


            this.appToRename = appToRename;

           


            
            this.renameLabel.text = string.Format("Add New Application Name For '{0}'", this.appToRename.appName);

            this.inputField.text=this.appToRename.appName;

            GameObject.Find("Debug").GetComponent<Text>().text=GameObject.Find("Debug").GetComponent<Text>().text+"\n"+this.inputField.text;
           
        }

        /// <summary>
        /// Opens the rename dialog
        /// </summary>
        /// <param name="entry"></param>
        public async void Rename()
        {
            //Debug.Assert(null != this.appToRename, "App to rename is null");

            if (!string.IsNullOrEmpty(this.inputField.text))
            {
                var filePath = appToRename.externalIconPath;
                bool cleanupFile = false;

                // If icon path is null, extract icon from apk
                // if (null == filePath)
                // {
                //     filePath = Path.Combine(AssetsDownloader.GetOrCreateDownloadPath(), this.appToRename.packageId + ".jpg");
                //     cleanupFile = true;

                //     var bytes = appTarget.sprite.GetComponent<Image>().sprite.texture.EncodeToJPG();
                //     using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None,
                //         bufferSize: 4096, useAsync: true))
                //     {
                //         await fileStream.WriteAsync(bytes, 0, bytes.Length);
                //     };
                // }

                await Task.Run(() =>
                {
                    AndroidJNI.AttachCurrentThread();

                    try
                    {
                        // Add to json file
                        AddToRenameJsonFile(appToRename.packageId, inputField.text);

                    }
                    finally
                    {
                        AndroidJNI.DetachCurrentThread();
                    }
                });

                if (cleanupFile && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Reload the scene
                await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }else{

                errorLabel.SetActive(true);

            }
        }

        /// <summary>
        /// Closes the rename panel
        /// </summary>
        public void CloseRenamePanel()
        {
            Debug.Log("Close Rename Panel");
            this.panelContainer.SetActive(true);
             this.renamePanelContainer.SetActive(false);
        }

        /// <summary>
        /// Adds app to rename to json file
        /// </summary>
        /// <param name="packageId">Package id of app</param>
        /// <param name="appName">App name</param>
        private void AddToRenameJsonFile(string packageId, string appName)
        {
            var renameJsonFilePath = Path.Combine(UnityEngine.Application.persistentDataPath, AppProcessor.AppNameFile);
            Dictionary<string, AppProcessor.JsonAppNamesEntry> jsonAppNames = null;

            GameObject.Find("Debug").GetComponent<Text>().text=GameObject.Find("Debug").GetComponent<Text>().text+"\n"+"Test";
            if (File.Exists(renameJsonFilePath))
            {
                // Read rename json file
                try
                {
                    var json = File.ReadAllText(renameJsonFilePath, Encoding.UTF8);
                    jsonAppNames = JsonConvert.DeserializeObject<Dictionary<string, AppProcessor.JsonAppNamesEntry>>(json);
                }
                catch (Exception e)
                {
                    // On exception, we'll keep going & just overwrite existing file contents
                    Debug.LogFormat("Failed to process json rename app file: {0}", e.Message);
                }
            }

            // Add entry
            if (null == jsonAppNames)
            {
                jsonAppNames = new Dictionary<string, AppProcessor.JsonAppNamesEntry>();
            }
            
            GameObject.Find("Debug").GetComponent<Text>().text=GameObject.Find("Debug").GetComponent<Text>().text+"\n->"+appName+"->"+packageId+"->"+GridPopulation.currentCategory;
            jsonAppNames[packageId] = new AppProcessor.JsonAppNamesEntry { Name = appName, Category = GridPopulation.currentCategory };

            // Persist
            try
            {
                File.WriteAllText(renameJsonFilePath, JsonConvert.SerializeObject(jsonAppNames, Formatting.Indented));
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Failed to write json rename app file: {0}", e.Message));
            }
        }
    }
}