using CrossStitch.Core.Attributes;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CrossStitch.App.Controls.Instructions
{
    public class ComboBoxInstructions : IControlInstructions
    {
        public bool CanCreate(PropertyInfo property)
        {
            return property.GetCustomAttribute<LookupAttribute>() != null;
        }

        public Control Create(Input input, PropertyInfo property, Binding binding, string display, string description)
        {
            var lookupProperty = property.GetCustomAttribute<LookupAttribute>();

            var comboBox = new ComboBox
            {
                ToolTip = description,
                ItemsSource = lookupProperty.LookupOptions,
                SelectedValuePath = "Value",
                DisplayMemberPath = "Display",
                MinWidth = 200,
                IsEnabled = !input.IsReadOnly
            };

            BindingOperations.SetBinding(comboBox, Selector.SelectedValueProperty, binding);

            return comboBox;
        }
    }
}