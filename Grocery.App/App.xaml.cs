using Grocery.App.ViewModels;
using Grocery.App.Views;

namespace Grocery.App
{
    public partial class App : Application
    {
        public App(LoginViewModel viewModel) // ← Stap 1: /* en */ verwijderd → parameter actief
        {
            InitializeComponent();

            //MainPage = new AppShell();           ← Stap 2: uitgeschakeld met //
            MainPage = new LoginView(viewModel);   // ← Stap 2: // verwijderd → actief
        }
    }
}
