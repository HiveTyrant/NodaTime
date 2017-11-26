using System.Windows.Controls;
using NodaTimeWpfApplication.ViewModels;

namespace NodaTimeWpfApplication.Views
{
    /// <summary>
    /// Interaction logic for MainViewUserControl.xaml
    /// </summary>
    public partial class MainViewUserControl : UserControl
    {
        public MainViewUserControl()
        {
            InitializeComponent();
        }

        public MainViewUserControl(MainViewViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
