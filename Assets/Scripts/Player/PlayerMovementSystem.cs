using FieldDay.Debugging;
using FieldDay.SharedState;
using FieldDay.Systems;
using UnityEngine;

namespace Waddle
{
    [SysUpdate(FieldDay.GameLoopPhase.Update, 1000)]
    public class PlayerMovementSystem : SharedStateSystemBehaviour<PlayerMovementState, PlayerHeadState>
    {
        #region Inspector

        [SerializeField] private float m_MoveSpeedMultiplier = 1f / 90f;

        #endregion // Inspector

        public override void ProcessWork(float deltaTime)
        {
            if (!m_StateA.Queued) {
                return;
            }

            Vector3 originalPos = m_StateB.PositionRoot.position;
            PlayerMoveResult result = TryMove(m_StateA, m_StateB);
            Vector3 finalPos = m_StateB.PositionRoot.position;

            m_StateA.ConsecutiveSteps++;

            DebugDraw.AddLine(originalPos, finalPos, Color.green, 1, 1, false);

            m_StateA.Queued = false;
        }

        private PlayerMoveResult TryMove(PlayerMovementState moveState, PlayerHeadState headState) {
            Vector3 footStart = headState.FootRoot.position;
            Vector3 headStart = headState.HeadRoot.position;
            Vector3 bodyStart = headState.PositionRoot.position;

            Ray footCast = new Ray(footStart, moveState.MoveDirection);
            Ray headCast = new Ray(headStart, moveState.MoveDirection);
            Ray bodyCast = new Ray(bodyStart, moveState.MoveDirection);

            // TODO: collisions

            Vector3 targetPos = bodyCast.GetPoint(moveState.MoveSpeed * m_MoveSpeedMultiplier);
            headState.PositionRoot.position = targetPos;
            return PlayerMoveResult.Allowed;

            //Vector3 potentialPos = _positionTransform.transform.position + _rotationTransform.transform.forward * _speed;
            //Vector3 startFromOffset = _rotationTransform.transform.forward * 0.3f;
            //Vector3 checkPos = _centerEye.transform.position + startFromOffset + _rotationTransform.transform.forward * _speed;
            //Vector3 heightPos = _centerEye.transform.position + startFromOffset;
            //float d = 0f;
            //bool validMove = true;

            //for (int i = 0; i < _worldColliders.Length; ++i) {
            //    //check from height of person...

            //    Collider c = _worldColliders[i].GetComponent<Collider>();
            //    if (c != null) {
            //        if (c.bounds.Contains(checkPos)) {
            //            Ray r = new Ray();
            //            r.origin = heightPos;
            //            r.direction = Vector3.Normalize(checkPos - heightPos);

            //            if (c.bounds.IntersectRay(r, out d)) {
            //                if (d > 0.25f) {
            //                    //Debug.Log("Intersects ray!");
            //                    potentialPos = _positionTransform.transform.position + (d) * r.direction;// r.origin + (d) * r.direction;
            //                                                                                             //potentialPos = potentialPos - r.direction * 0.3f;
            //                                                                                             //potentialPos.y -= 0.28f;
            //                } else {
            //                    validMove = false;
            //                }
            //                break;
            //            }
            //            //potentialPos = c.ClosestPointOnBounds(potentialPos);
            //            //Debug.Log(potentialPos.ToString("F4"));

            //        }
            //    }
            //}

            //if (validMove) {
            //    PenguinAnalytics.Instance.LogMove(_positionTransform.transform.position, potentialPos, _rotationTransform.transform.rotation, _wasRight);

            //    _positionTransform.transform.position = potentialPos;
            //    AudioSource audioClip = GetComponent<AudioSource>();
            //    if (audioClip != null) {
            //        audioClip.Play();
            //    }
            //} else {
            //    AudioSource audioClip = GetComponent<AudioSource>();
            //    if (audioClip != null) {
            //        if (_collideClip != null) {
            //            audioClip.PlayOneShot(_collideClip);
            //        }
            //    }
            //}
            return PlayerMoveResult.Allowed;
        }
    }

    public enum PlayerMoveResult {
        Allowed,
        Blocked_Solid,
        Blocked_Invisible
    }
}