using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform leftHandObj = null;
    private GameObject currentInstance;
    public GameObject itemPrefab;
    public bool iKActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandlePrefab();
    }

    void HandlePrefab()
    {
        if (iKActive && itemPrefab != null)
        {
            if (currentInstance == null)
            {
                currentInstance = Instantiate(itemPrefab, rightHandObj.position, rightHandObj.rotation);
                currentInstance.transform.SetParent(rightHandObj);
            }
        }
        else
        {
            if (currentInstance != null)
            {
                Destroy(currentInstance);
            }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (iKActive)
            {
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
