using System.Windows.Input;

namespace Pool.Control;

public class RelayCommand
	: ICommand
{
	public event EventHandler? CanExecuteChanged;
	readonly Func<bool>? canExecute;
	readonly Action execute;

	public RelayCommand(Action execute)
	{
		this.execute = execute;
	}

	public RelayCommand(
		Func<bool> canExecute
		, Action execute)
	{
		this.canExecute = canExecute;
		this.execute = execute;
	}

    public void InvokeCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

	public bool CanExecute(object? parameter) => canExecute == null || canExecute.Invoke();

	public void Execute(object? parameter) => execute();
}