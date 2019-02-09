using CrossStitch.Core.Interfaces;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossStitch.Tests.Mocks
{
    public enum DialogType
    {
        Error, Info, Message, Question, Warning
    }

    public class TestDialogService : IDialogService
    {
        public class DialogInformation
        {
            public DialogInformation(DialogType type, string title)
            {
                Type = type;
                Title = title;
            }

            public DialogType Type { get; }
            public string Title { get; }
        }

        public List<DialogInformation> ShownDialogs = new List<DialogInformation>();

        public void HasDialogShown(DialogType type, string title = null)
        {
            ShownDialogs.Should().Contain(new DialogInformation(type, title));
        }

        public void LastDialogWas(object expected)
        {
            ShownDialogs.Last().Should().BeEquivalentTo(expected);
        }

        public void LastDialogWas(DialogType type, string title)
        {
            LastDialogWas(new DialogInformation(type, title));
        }

        public void NoDialogShown() => ShownDialogs.Should().BeEmpty();

        private void AddDialogInformation(DialogType type, string title)
        {
            ShownDialogs.Add(new DialogInformation(type, title));
        }

        public void ShowError(Exception error, string title)
        {
            AddDialogInformation(DialogType.Error, title);
        }

        public void ShowError(string message, string title)
        {
            AddDialogInformation(DialogType.Error, title);
        }

        public void ShowInfo(string message, string title)
        {
            AddDialogInformation(DialogType.Info, title);
        }

        public void ShowMessage(string message, string title)
        {
            AddDialogInformation(DialogType.Message, title);
        }

        public bool ShowQuestion(string message, string title)
        {
            AddDialogInformation(DialogType.Question, title);
            return true;
        }

        public void ShowWarning(string message, string title)
        {
            AddDialogInformation(DialogType.Warning, title);
        }
    }
}