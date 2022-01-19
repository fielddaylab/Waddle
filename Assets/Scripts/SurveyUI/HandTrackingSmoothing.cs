using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauUtil;

public class HandTrackingSmoothing : MonoBehaviour
{
    private RingBuffer<Vector3> queue;
    private RingBuffer<Quaternion> rotQueue;
    private Transform target;
    private OVRCameraRig m_CameraRig;
    private OVRInput.Controller active;
    private LaserPointer m_Pointer;
    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Quaternion[] rotationOffsets;
    //private 
    [SerializeField] private int frameHistory;
    // Start is called before the first frame update
    void Start()
    {
        queue = new RingBuffer<Vector3>(frameHistory, RingBufferMode.Overwrite);
        rotQueue = new RingBuffer<Quaternion>(frameHistory, RingBufferMode.Overwrite);
        m_CameraRig = FindObjectOfType<OVRCameraRig>();
        m_Pointer = FindObjectOfType<LaserPointer>();
        FindActiveTarget();
    }

    private void FindActiveTarget() {
        active = OVRInput.GetActiveController();
        switch (active) {
            case OVRInput.Controller.LHand:
            case OVRInput.Controller.LTouch:
                target = m_CameraRig.leftHandAnchor;
                break;
            case OVRInput.Controller.RHand:
            case OVRInput.Controller.Hands:
            case OVRInput.Controller.RTouch:
            case OVRInput.Controller.Touch:
            default:
                target = m_CameraRig.rightHandAnchor;
                break;
        }
    }
    // Update is called once per frame
    
    void FixedUpdate() {
        if (active != OVRInput.GetActiveController())
            FindActiveTarget();

        queue.PushFront(target.transform.position);
        rotQueue.PushFront(target.transform.rotation);

        Vector3 totalPos = Vector3.zero;
        Vector4 cumulative = Vector4.zero;
        Quaternion finRot = rotQueue.PeekFront();
        foreach (var pos in queue) {
            totalPos += pos;
        }
        foreach (var rot in rotQueue) {
            finRot = QuaternionAvg.AverageQuaternion(ref cumulative, rot, finRot, rotQueue.Count);
        }


        totalPos *= 1f / queue.Count;

        transform.position = totalPos;

        if (active != OVRInput.Controller.LTouch && active != OVRInput.Controller.RTouch && active != OVRInput.Controller.Touch) {
            foreach (var rot in rotationOffsets)
                finRot *= rot;
            transform.position += (finRot * positionOffset);
            indicator.color = Color.yellow;
        } else {
            indicator.color = Color.magenta;
        }

        transform.rotation = finRot;

    }
}
