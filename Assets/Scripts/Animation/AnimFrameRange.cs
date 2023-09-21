using System;
using UnityEngine;

namespace Waddle {
    [Serializable]
    public struct AnimFrameRange {
        public int FrameBegin;
        public int FrameEnd;

        public bool InRange(AnimatorStateInfo stateInfo, AnimationClip clip) {
            float currentFrame = (stateInfo.normalizedTime % 1) * clip.frameRate * clip.length;
            return currentFrame >= FrameBegin && currentFrame < FrameEnd;
        }

        public bool InRange(AnimatorStateInfo stateInfo, int totalFrameCount) {
            float currentFrame = (stateInfo.normalizedTime % 1) * totalFrameCount;
            return currentFrame >= FrameBegin && currentFrame < FrameEnd;
        }

        public bool InRange(AnimatorStateInfo stateInfo, AnimationClip clip, out float lerp) {
            float currentFrame = (stateInfo.normalizedTime % 1) * clip.frameRate * clip.length;
            float rawLerp = (currentFrame - FrameBegin) / (FrameEnd - FrameBegin);
            bool inRange = rawLerp >= 0 && rawLerp < 1;
            lerp = Mathf.Clamp01(rawLerp);
            return inRange;
        }

        public bool InRange(AnimatorStateInfo stateInfo, int totalFrameCount, out float lerp) {
            float currentFrame = (stateInfo.normalizedTime % 1) * totalFrameCount;
            float rawLerp = (currentFrame - FrameBegin) / (FrameEnd - FrameBegin);
            bool inRange = rawLerp >= 0 && rawLerp < 1;
            lerp = Mathf.Clamp01(rawLerp);
            return inRange;
        }

        static public bool InRange(AnimatorStateInfo stateInfo, AnimFrameRange[] ranges, int frameCount) {
            for (int i = 0, len = ranges.Length; i < len; i++) {
                if (ranges[i].InRange(stateInfo, frameCount)) {
                    return true;
                }
            }
            return false;
        }
    }
}