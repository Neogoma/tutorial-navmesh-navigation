using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.Neogoma.Stardust.Navigation;
using com.Neogoma.Stardust.Graph;
using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;

namespace Neogoma.Stardust.Demo.NavMeshDemo
{
    /// <summary>
    /// Robot panel view class, responsible for handling everything from the panel viewer
    /// </summary>
    public class RobotPanelView : MonoBehaviour
    {
        /// <summary>
        /// panel that holds Dialogue and Target option.
        /// </summary>
        [SerializeField]
        private GameObject mainNavPanel;

        /// <summary>
        /// panel show hint about destination.
        /// </summary>
        [SerializeField]
        private GameObject hintNavPanel;

        /// <summary>
        /// paused nav panel.
        /// </summary>
        [SerializeField]
        private GameObject pausedNavPanel;

        /// <summary>
        /// Navigation Panel message.
        /// </summary>
        [SerializeField]
        private TMP_Text hintPanelMessage;

        /// <summary>
        /// target selection dropdown
        /// </summary>
        [SerializeField]
        private Dropdown targetSelectionDropDown;

        /// <summary>
        /// Target list containing target data transform and hint message.
        /// </summary>
        [SerializeField]
        private List<TargetData> targetList;

        /// <summary>
        /// Current target
        /// </summary>
        private Transform currentTarget;

        /// <summary>
        /// Current Target getter
        /// </summary>
        public Transform CurrentTarget
        {
            get { return currentTarget; }
        }

        // Start is called before the first frame update
        void Start()
        {
            targetSelectionDropDown.onValueChanged.AddListener(OnTargetSelected);
            PopulateTargetList(targetList);
        }

        /// <summary>
        /// Show Hide Main Panel
        /// </summary>
        /// <param name="value"></param>
        public void ShowHideMainPanel(bool value)
        {
            mainNavPanel.SetActive(value);
        }

        /// <summary>
        /// Show Hide Hint Panel
        /// </summary>
        /// <param name="value"></param>
        public void ShowHideHintPanel(bool value)
        {
            hintNavPanel.SetActive(value);
        }
        /// <summary>
        /// Show Hide Paused Panel 
        /// </summary>
        /// <param name="value"></param>
        public void ShowHidePausedPanel(bool value)
        {
            pausedNavPanel.SetActive(value);
        }

        /// <summary>
        /// Hide All Panels
        /// </summary>
        public void HideAllPanels()
        {
            mainNavPanel.SetActive(false);
            hintNavPanel.SetActive(false);
            pausedNavPanel.SetActive(false);
        }

        /// <summary>
        /// Populate Target List
        /// </summary>
        /// <param name="allTargets"></param>
        private void PopulateTargetList(List<TargetData> allTargets)
        {
            targetSelectionDropDown.ClearOptions();
            List<string> allTargetNames = new List<string>();
            for (int i = 0; i < allTargets.Count; i++)
            {
                string targetName = allTargets[i].targetTransform.name;
                allTargetNames.Add(targetName);
            }
            targetSelectionDropDown.AddOptions(allTargetNames);
            //init default values
            currentTarget = targetList[0].targetTransform;
            hintPanelMessage.text = targetList[0].targetHint;
        }

        /// <summary>
        /// Called when target selected. Otherwise dont show the go button.
        /// </summary>
        /// <param name="val">index of target selected</param>
        public void OnTargetSelected(int val)
        {
            currentTarget = targetList[val].targetTransform;
            hintPanelMessage.text = targetList[val].targetHint;
            Debug.Log("Target selected: " + currentTarget.name + " -> Target Hint Message: " + hintPanelMessage.text);
        }
    }

    /// <summary>
    /// Target Data class, holds data for use in the list of targets and hints.
    /// </summary>
    [System.Serializable]
    public class TargetData
    {
        public Transform targetTransform;
        public string targetHint;
    }
}
