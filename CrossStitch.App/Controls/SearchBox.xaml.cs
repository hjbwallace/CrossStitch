using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossStitch.App.Controls
{
    public partial class SearchBox : UserControl
    {
        public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.Register(
                    "EnterCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(object), typeof(SearchBox), new PropertyMetadata(""));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
                            "Watermark", typeof(object), typeof(SearchBox), new PropertyMetadata("Watermark"));

        public SearchBox()
        {
            InitializeComponent();
            Grid.DataContext = this;
        }

        public ICommand EnterCommand
        {
            get => (ICommand)GetValue(EnterCommandProperty);
            set => SetValue(EnterCommandProperty, value);
        }

        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public object Watermark
        {
            get => (object)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
    }
}