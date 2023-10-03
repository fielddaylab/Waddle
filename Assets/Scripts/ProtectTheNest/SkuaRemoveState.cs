using FieldDay.Processes;
using UnityEngine;

public class SkuaRemoveState : SkuaStateBase
{
    public override void Handle(Process process, SkuaController sc)
	{
		Object.Destroy(sc.gameObject);
    }
}
