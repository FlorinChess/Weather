using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Weather.Commands
{
    public class RelayCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private readonly Action _action;

        private readonly Func<Task> _func;

        #endregion

        #region Public Events

        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayCommand(Action action)
        {
            _action = action;
        }

        public RelayCommand(Func<Task> func)
        {
            _func = func;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter) => true;

        /// <summary>
        /// Executes the commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object? parameter)
        {
            _action?.Invoke();

            if (_func == null) return;

            await _func?.Invoke();
        } 

        #endregion
    }
}
