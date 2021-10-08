//Copyright WID Virtual Environments Group 2018-Present
//Authors:Simon Smith, Ross Tredinnick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIDVE.Paths
{
    public interface IPathEvent
    {
        void Trigger(PathTrigger trigger);
    }
}