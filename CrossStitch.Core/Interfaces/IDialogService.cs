using System;

namespace CrossStitch.Core.Interfaces
{
    public interface IDialogService
    {
        void ShowError(Exception error, string title);

        void ShowError(string message, string title);

        void ShowInfo(string message, string title);

        void ShowMessage(string message, string title);

        bool ShowQuestion(string message, string title);

        void ShowWarning(string message, string title);
    }
}