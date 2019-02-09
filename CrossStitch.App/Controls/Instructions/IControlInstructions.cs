using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace CrossStitch.App.Controls.Instructions
{
    public interface IControlInstructions
    {
        bool CanCreate(PropertyInfo property);

        Control Create(Input input, PropertyInfo property, Binding binding, string display, string description);
    }
}