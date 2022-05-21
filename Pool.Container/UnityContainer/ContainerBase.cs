using Unity;

namespace Pool.Container;

public abstract class ContainerBase
	: IRegister
{
	protected IUnityContainer Container;

	public ContainerBase(IUnityContainer unityContainer)
	{
		Container = unityContainer;
	}

	public abstract void Register();

	public virtual void Initialize() { }
}