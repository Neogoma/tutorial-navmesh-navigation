using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Neogoma.Stardust.Demo.NavMeshDemo
{
    /// <summary>
    /// Robot controller class. Responsible for logic related to the robot
    /// </summary>
    public class RobotController : MonoBehaviour
    {
        /// <summary>
        /// rotation speed of guide bot.
        /// </summary>
        public float rotSpeed;

        /// <summary>
        /// Main Camera
        /// </summary>
        private Camera cam;

        /// <summary>
        /// Robot Agent
        /// </summary>
        private NavMeshAgent robotAgent;

        /// <summary>
        /// Robot panel view reference
        /// </summary>
        [SerializeField]
        private RobotPanelView robotPanelView;

        private void Start()
        {
            cam = Camera.main;
            robotAgent = GetComponent<NavMeshAgent>();
            robotAgent.transform.position = cam.transform.position + cam.transform.forward * 1.1f;
            robotAgent.transform.LookAt(cam.transform);
        }
        // Update is called once per frame
        void Update()
        {
            //if robot within range. rotate towards the camera.
            float distance = Vector3.Distance(transform.position, cam.transform.position);
            if (distance <= 2)
            {
                Debug.Log("ROBOT: close enough to the user. Im now looking at him");
                RotateToTarget(cam.transform.position);
            }
            else if (distance >= 4)
            {
                RotateToTarget(cam.transform.position);
                PauseRobot();
            }
        }

        /// <summary>
        /// Rotates prefab object to face the target parameter only on 1 axis.
        /// </summary>
        /// <param name="target">Target position.</param>
        private void RotateToTarget(Vector3 target)
        {
            Vector3 targetPostition = new Vector3(target.x, transform.position.y, target.z) - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(targetPostition);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
        }
        /// <summary>
        /// Checks if robot has reached destination
        /// </summary>
        /// <returns></returns>
        private bool HasReachedDestination()
        {
            // Check if we've reached the destination
            if (!robotAgent.pathPending)
            {
                if (robotAgent.remainingDistance <= robotAgent.stoppingDistance)
                {
                    if (!robotAgent.hasPath || robotAgent.velocity.sqrMagnitude == 0f)
                    {
                        //reached
                        return true;
                    }
                }
            }

            //Not reached
            return false;
        }

        /// <summary>
        /// Check distance from target
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckDistanceFromTarget()
        {
            for (; ; )
            {
                if (HasReachedDestination())
                {
                    robotPanelView.ShowHideHintPanel(true);
                    yield break;
                }
                yield return new WaitForSeconds(.1f);
            }
        }

        /// <summary>
        /// Starts the robot movement to the selected target in the panel dropDown. Also initiates check distance from targe.
        /// </summary>
        public void StartRobot()
        {
            robotAgent.isStopped = false;
            robotPanelView.HideAllPanels();
            RotateToTarget(robotPanelView.CurrentTarget.position);
            robotAgent.SetDestination(robotPanelView.CurrentTarget.position);
            StartCoroutine(CheckDistanceFromTarget());
        }

        /// <summary>
        /// Pauses the robot logic if the distance between the robot and the user is greater than the defined limit.
        /// </summary>
        private void PauseRobot()
        {
            //stop the robot
            robotAgent.isStopped = true;
            StopCoroutine("CheckDistanceFromTarget");

            //enable the paused panel
            robotPanelView.HideAllPanels();
            robotPanelView.ShowHidePausedPanel(true);
        }

        /// <summary>
        /// Resets guide bot.
        /// </summary>
        public void QuitGuideRobot()
        {
            StopCoroutine("CheckDistanceFromTarget");
            robotPanelView.HideAllPanels();
            robotPanelView.ShowHideMainPanel(true);
        }
    }
}
