using System;
using System.Windows.Input;

namespace Pool.Control;

public class RelayCommand
	: ICommand
{
	public event EventHandler CanExecuteChanged;
	readonly Func<bool> _canExecute;
	readonly Action _execute;

	public RelayCommand(Action execute)
		: this(null, execute)
	{
	}

	public RelayCommand(
		Func<bool> canExecute
		, Action execute)
	{
		_canExecute = canExecute;
		_execute = execute;
	}

	public bool CanExecute(object parameter) => _canExecute == null || _canExecute.Invoke();

	public void Execute(object parameter) => _execute();
}