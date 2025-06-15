using System.Windows.Input;
using Core;

namespace Lastik.Helpers
{
    public class KeyboardHelper : ObservableObject
    {
        public static KeyboardHelper Instance { get; } = new KeyboardHelper();

        public bool IsKeyboardOpen
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        private ICommand? _openCommand;

        public ICommand OpenCommand => _openCommand ??= new RelayCommand(f =>
        {
            IsKeyboardOpen = true;
        });

        private ICommand? _closeCommand;

        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(f =>
        {
            IsKeyboardOpen = false;
        });
    }
}
