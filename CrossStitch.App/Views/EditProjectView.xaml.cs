using CrossStitch.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CrossStitch.App.Views
{
    /// <summary>
    /// Interaction logic for EditProjectView.xaml
    /// </summary>
    public partial class EditProjectView : Page
    {
        private EditProjectViewModel _viewModel;

        public EditProjectView()
        {
            InitializeComponent();

            _viewModel = ViewModelLocator.Get<EditProjectViewModel>();
        }

        private void OnPatternCompletedChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateOrderCommand.Execute(null);
        }
    }
}