using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using Neogoma.Stardust.Demo.NavMeshDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Demo for a relocation use case
    /// </summary>
    public class RelocationDemo:MonoBehaviour
    {
        /// <summary>
        /// Text used to show the download picture status.
        /// </summary>
        public Text downloadingData;
        /// <summary>
        /// Text used to show the results of the server matching.
        /// </summary>
        public Text matchingResult;
        /// <summary>
        /// Dropdown to select the map.
        /// </summary>
        public Dropdown mapSelectionDropDown;
        /// <summary>
        /// Name of the selected session.
        /// </summary>
        public Text sessionName;
        /// <summary>
        /// Button to locate the user.
        /// </summary>
        public Button locateMeButton;
        /// <summary>
        /// Main Navigation Prefab Parent
        /// </summary>
        public GameObject mainNavigationPrefab;
        /// <summary>
        /// Event triggers when we show relocation result.
        /// </summary>
        public UnityEvent showResultsEvent = new UnityEvent();
        /// <summary>
        /// Session controller
        /// </summary>
        private SessionController sessionController;
        /// <summary>
        /// Relocation Manager reference
        /// </summary>
        private MapRelocationManager relocationManager;
        /// <summary>
        /// index to session dictionary
        /// </summary>
        private Dictionary<int, Session> indexToSession = new Dictionary<int, Session>();
        /// <summary>
        /// current selected session
        /// </summary>
        private Session selectedSession;

        public void Awake()
        {
            sessionController = SessionController.Instance;            
            mapSelectionDropDown.onValueChanged.AddListener(OnMapSelected);
            sessionController.onAllSessionsRetrieved.AddListener(MapListDownloaded);
            RequireMapList();

            relocationManager = MapRelocationManager.Instance;
            relocationManager.onMapDownloadedSucessfully.AddListener(OnMapDownloaded);
            relocationManager.onMapDownloadStarted.AddListener(OnMapStartDownloading);
            relocationManager.onPositionFound.AddListener(OnPositionMatched);
            relocationManager.onLocationNotFound.AddListener(OnPositionMatchFailed);
        }
        
        /// <summary>
        /// On Map Downloaded
        /// </summary>
        /// <param name="session"></param>
        /// <param name="map"></param>
        private void OnMapDownloaded(Session session,GameObject map)
        {
            downloadingData.gameObject.SetActive(false);
            locateMeButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// On Map Start Downloading
        /// </summary>
        private void OnMapStartDownloading()
        {
            downloadingData.gameObject.SetActive(true);
        }

        /// <summary>
        /// On Position Matched
        /// </summary>
        /// <param name="positionMatched"></param>
        /// <param name="newCoords"></param>
        private void OnPositionMatched(RelocationResults positionMatched,CoordinateSystem newCoords)
        {
            ShowMatchResults("Located sucessfully!", Color.green);
            locateMeButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// On Position Match Failed
        /// </summary>
        private void OnPositionMatchFailed()
        {
            ShowMatchResults("Failed to locate", Color.red);
            locateMeButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Map List Downloaded
        /// </summary>
        /// <param name="allSessions"></param>
        private void MapListDownloaded(Session[] allSessions)
        {
            indexToSession.Clear();
            List<string> mapListDatas = new List<string>();
            mapListDatas.Add("NONE");

            for (int i = 0; i < allSessions.Length; i++)
            {
                Session currentSession = allSessions[i];
                mapListDatas.Add(currentSession.name);


                indexToSession.Add(i+1, currentSession);

            }

            mapSelectionDropDown.AddOptions(mapListDatas);
            mapSelectionDropDown.gameObject.SetActive(true);
        }

        /// <summary>
        /// Show Match Results
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        private void ShowMatchResults(string text, Color color)
        {
            matchingResult.color = color;
            matchingResult.text = text;
            matchingResult.gameObject.SetActive(true);
            StartCoroutine(HideTextCoroutine());
            showResultsEvent.Invoke();
        }

        /// <summary>
        /// Require Map List
        /// </summary>
        private void RequireMapList()
        {
            sessionController.GetAllSessionsReady();
        }

        /// <summary>
        /// On Map Selected 
        /// </summary>
        /// <param name="val"></param>
        private void OnMapSelected(int val)
        {
            selectedSession = indexToSession[val];
            
            if (selectedSession != null)
            {
                mapSelectionDropDown.gameObject.SetActive(false);
                relocationManager.GetDataForMap(selectedSession);
                sessionName.text = selectedSession.name;
            }

            foreach (Transform trans in mainNavigationPrefab.transform)
            {
                if (selectedSession.uuid == trans.GetComponent<NavMeshController>().mapUUID)
                {
                    trans.gameObject.SetActive(true);
                    Debug.Log("For the map with UUID: " + selectedSession.uuid + " the Prefab name is: " + trans.name);
                }
                else
                {
                    trans.gameObject.SetActive(false);
                }
            }
        }        

        /// <summary>
        /// Hide Text Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideTextCoroutine()
        {
            yield return new WaitForSeconds(2);
            matchingResult.gameObject.SetActive(false);
        }

    }
}