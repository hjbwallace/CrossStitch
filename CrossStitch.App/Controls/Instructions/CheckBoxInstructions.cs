using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CrossStitch.App.Controls.Instructions
{
    public class CheckBoxInstructions : IControlInstructions
    {
        public bool CanCreate(PropertyInfo property)
        {
            return property.PropertyType == typeof(bool)
                || property.PropertyType == typeof(bool?);
        }

        public Control Create(Input input, PropertyInfo property, Binding binding, string display, string description)
        {
            input.Label = null;

            var checkBox = new CheckBox
            {
                Content = display,
                ToolTip = description,
                IsThreeState = property.PropertyType == typeof(bool?),
                IsEnabled = !input.IsReadOnly
            };

            BindingOperations.SetBinding(checkBox, ToggleButton.IsCheckedProperty, binding);

            return checkBox;
        }
    }
}