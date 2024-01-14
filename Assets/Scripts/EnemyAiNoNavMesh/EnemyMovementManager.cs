using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YesserOthmene
{
    public class EnemyMovementManager : MonoBehaviour
    {
        EnemyAnimatorManagerNoNavMesh enemyAnimatorManager;
        public CharacterStats currentTarget;
        [HideInInspector]
        public Rigidbody enemyRB;

        [SerializeField] Quaternion playerRotation;

        bool isPreformingAction;

        public float distanceFromTarget;

        [Header("Ai Settings")]
        [Space]
        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] LayerMask obstacleLayerMask;
        [Space]
        [SerializeField] float stoppingDistance;
        [SerializeField] float rotationSpeed;
        [SerializeField] float maxDist; //For max detection range
        [SerializeField] float detectionRadius; // For the size of the detection area (Circle radius)
        [SerializeField] float obstacleDetectionLength;
        [Range(0f, 90f), SerializeField] float tiltAngle; //used for the angle of the left and right Rays
        [Space]
        public bool useVelocityChange;

        private void Awake()
        {
            enemyAnimatorManager= GetComponent<EnemyAnimatorManagerNoNavMesh>();
            enemyRB = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            playerRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            if (currentTarget == null)
            {
                DetectPlayer();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }

        private void DetectPlayer()
        {
            Ray ray = new Ray(transform.position - (transform.forward * detectionRadius), transform.forward);

            if (Physics.SphereCast(ray, detectionRadius, out RaycastHit hit, maxDist, targetLayerMask))
            {
                currentTarget = hit.collider.GetComponent<CharacterStats>();
                Debug.Log("hit " + hit.collider.name);
            }
            else
            {
                Debug.Log("No player in range");
            }
        }
        private void MoveTowardsPlayer()
        {
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);

            //If the enemy is preforming an action, stop the movement !
            if (isPreformingAction)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    //TO DO! Hit Player
                }
            }
            RotateTowardsTarget();
        }

        private void RotateTowardsTarget()
        {
            Quaternion rotationRight = Quaternion.Euler(0f, tiltAngle, 0f);
            Quaternion rotationLeft = Quaternion.Euler(0f, -tiltAngle, 0f);

            Ray rayForward = new Ray(transform.position, transform.forward);
            Ray rayRight = new Ray(transform.position, rotationRight * transform.forward);
            Ray rayLeft = new Ray(transform.position, rotationLeft * transform.forward);

            bool forward = Physics.Raycast(rayForward, obstacleDetectionLength, obstacleLayerMask);
            bool right = Physics.Raycast(rayRight, obstacleDetectionLength, obstacleLayerMask);
            bool left = Physics.Raycast(rayLeft, obstacleDetectionLength, obstacleLayerMask);

            if ((forward || right || left) && obstacleDetectionLength <= distanceFromTarget)
            {
                Debug.Log("Rotating Auto");

                if (!right && left)
                {
                    Debug.Log("Left Ray =" + left + " , Right Ray =" + left);
                    Quaternion rotation = Quaternion.Euler(0f, 90f + playerRotation.y, 0f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed / Time.deltaTime);
                }
                else //if (right && !left)
                {
                    Debug.Log("Left Ray =" + left + " , Right Ray =" + left);
                    Quaternion rotation = Quaternion.Euler(0f, -90f - playerRotation.y, 0f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed / Time.deltaTime);
                }
                //else
                //{
                //    transform.rotation = Quaternion.Slerp(transform.rotation, rotationLeft, rotationSpeed / Time.deltaTime);
                //}
            }
            else
            {
                Debug.Log("Rotating Manual");
                //Rotate the enemy towards the player

                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}