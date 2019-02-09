using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace CrossStitch.App.Controls.Instructions
{
    public class TextBoxInstructions : IControlInstructions
    {
        public bool CanCreate(PropertyInfo property) => true;

        public Control Create(Input input, PropertyInfo property, Binding binding, string display, string description)
        {
            var textBox = new TextBox
            {
                MinWidth = 200,
                IsEnabled = !input.IsReadOnly,
                ToolTip = description,
            };

            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);

            return textBox;
        }
    }
}