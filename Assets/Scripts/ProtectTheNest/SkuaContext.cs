using System.Collections;
using System.Collections.Generic;

public interface ISkuaState
{
	void Handle(SkuaController sc);
}

public class SkuaContext
{
    // Start is called before the first frame update
    public ISkuaState CurrentState
	{
		get; set;
	}
	
	private readonly SkuaController _skuaController;
	
	public SkuaContext(SkuaController sc)
	{
		_skuaController = sc;
	}
	
	public void Transition()
	{
		CurrentState.Handle(_skuaController);
	}
	
	public void Transition(ISkuaState state)
	{
		CurrentState = state;
		CurrentState.Handle(_skuaController);
	}
}
