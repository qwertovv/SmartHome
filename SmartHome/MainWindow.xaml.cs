using System.Windows;
using System.Windows.Controls;
using SmartHome.ViewModels;

namespace SmartHome
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void CheckSecurity_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            // Логика проверки безопасности использует ЛР1 и ЛР4
            MessageBox.Show("Проверка безопасности выполнена", "Безопасность",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ToggleNightMode_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.HomeState.NightMode = !vm.HomeState.NightMode;
            // Использует ЛР1 для проверки условий и ЛР4 для логики
        }

        private void ToggleAwayMode_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            // Использует ЛР1 для валидации контракта
            var validator = new Services.ContractValidator();
            var result = validator.ValidateAwayMode(vm.HomeState);

            if (result.isValid)
            {
                vm.HomeState.AwayMode = !vm.HomeState.AwayMode;
                MessageBox.Show("Режим изменен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(result.errorMessage, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OptimizeScenario_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            // Использует ЛР2 для расчета оптимального сценария
            vm.DashboardVM.CalculateOptimalScenario();
            MessageBox.Show("Оптимизация выполнена", "Оптимизация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}