namespace Pool.Container;

public interface IOrder<TType>
{
	TType Order();
}