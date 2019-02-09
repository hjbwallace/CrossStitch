using CrossStitch.App.Controls.Instructions;
using CrossStitch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace CrossStitch.App.Controls
{
    public class Input : ContentControl
    {
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(Input), new PropertyMetadata(false));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(Input), new PropertyMetadata(null));

        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(int), typeof(Input), new PropertyMetadata(120));

        public static readonly DependencyProperty SourceProperty;
        private static readonly Action _emptyDelegate = delegate { };

        private static readonly IEnumerable<IControlInstructions> _instructions;
        private static readonly DependencyProperty _previousFieldBindingProperty = DependencyProperty.RegisterAttached("PreviousFieldBinding", typeof(Binding), typeof(Input), new UIPropertyMetadata(null));

        static Input()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Input), new FrameworkPropertyMetadata(typeof(Input)));

            SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(Input),
                                                      new FrameworkPropertyMetadata(new object(), SourcePropertySet)
                                                      {
                                                          DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                                      });

            _instructions = new IControlInstructions[]
            {
                new CheckBoxInstructions(),
                new ComboBoxInstructions(),
                new TextBoxInstructions(),
            };
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public int LabelWidth
        {
            get { return (int)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public void InvalidateField()
        {
            if (BindingOperations.GetBinding(this, SourceProperty) == null)
                return;

            Invalidate();
            Content = null;
            Configure();
        }

        private static Binding GetPreviousFieldBinding(DependencyObject obj)
        {
            return (Binding)obj.GetValue(_previousFieldBindingProperty);
        }

        private static void SetPreviousFieldBinding(DependencyObject obj, Binding value)
        {
            obj.SetValue(_previousFieldBindingProperty, value);
        }

        private static void SourcePropertySet(DependencyObject @this, DependencyPropertyChangedEventArgs e)
        {
            var field = (Input)@this;
            field.Configure();
        }

        private void Configure()
        {
            Dispatcher.Invoke(DispatcherPriority.Render, _emptyDelegate);

            var binding = BindingOperations.GetBinding(this, SourceProperty);

            if (binding == null)
                return;

            var previousBinding = GetPreviousFieldBinding(this);
            if (previousBinding == binding)
                return;

            SetPreviousFieldBinding(this, binding);

            var bindingEx = BindingOperations.GetBindingExpression(this, SourceProperty);

            if (bindingEx == null)
                return;

            var type = bindingEx.ResolvedSource.GetType();
            var property = type.GetProperty(bindingEx.ResolvedSourcePropertyName);

            var input = GenerateInputField(property, binding, bindingEx);

            Content = new AdornerDecorator { Child = input };
        }

        private Control GenerateControlFromInstructions(PropertyInfo property, Binding binding, string display, string description)
        {
            foreach (var instruction in _instructions)
            {
                if (instruction.CanCreate(property))
                    return instruction.Create(this, property, binding, display, description);
            }

            throw new ArgumentException("Cannot create input field for binding");
        }

        private Control GenerateInputField(PropertyInfo property, Binding binding, BindingExpression bindingExpression)
        {
            var uiAttribute = property.GetCustomAttribute<UiAttribute>();
            var display = uiAttribute?.Display ?? bindingExpression.ResolvedSourcePropertyName;
            var tooltip = uiAttribute?.Tooltip;

            Label = display + ":";

            var control = GenerateControlFromInstructions(property, binding, display, tooltip);

            if (control is TextBox)
            {
                control.MouseLeftButtonDown += OnMouseButtonDown;
                control.GotKeyboardFocus += OnGotKeyboardFocus;
            }

            return control;
        }

        private void HighlightField(object sender, RoutedEventArgs e)
        {
            var textbox = (sender as TextBox);
            textbox?.SelectAll();
        }

        private void Invalidate()
        {
            this.ClearValue(_previousFieldBindingProperty);
        }

        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
            => HighlightField(sender, e);

        private void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);

            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
            => HighlightField(sender, e);
    }
}