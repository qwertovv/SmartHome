using SmartHome.Models;
using SmartHome.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public SmartHomeState HomeState { get; set; }
        public DashboardViewModel DashboardVM { get; set; }
        public DevicesViewModel DevicesVM { get; set; }
        public AutomationViewModel AutomationVM { get; set; }
        public SecurityViewModel SecurityVM { get; set; }

        private readonly ContractValidator _contractValidator;
        private readonly WPCalculator _wpCalculator;
        private readonly InvariantChecker _invariantChecker;
        private readonly LogicEngine _logicEngine;

        public MainViewModel()
        {
            HomeState = new SmartHomeState();

            _contractValidator = new ContractValidator();
            _wpCalculator = new WPCalculator();
            _invariantChecker = new InvariantChecker();
            _logicEngine = new LogicEngine();

            DashboardVM = new DashboardViewModel(HomeState, _wpCalculator, _invariantChecker);
            DevicesVM = new DevicesViewModel(HomeState, _contractValidator);
            AutomationVM = new AutomationViewModel(HomeState, _logicEngine);
            SecurityVM = new SecurityViewModel(HomeState, _contractValidator, _logicEngine);

            // Запуск фоновых процессов
            StartBackgroundChecks();
            StartPowerMonitoring();
        }

        private async void StartBackgroundChecks()
        {
            while (true)
            {
                _invariantChecker.CheckTemperatureInvariant(HomeState);
                _invariantChecker.CheckSecurityInvariant(HomeState);
                _invariantChecker.ExecuteClimateControlStep(HomeState);

                await Task.Delay(5000); // Проверка каждые 5 секунд
            }
        }

        private async void StartPowerMonitoring()
        {
            while (true)
            {
                HomeState.UpdateDevicePowerConsumption();
                await Task.Delay(3000); // Обновление каждые 3 секунды
            }
        }

        // Методы для управления устройствами
        public void ToggleDevice(string deviceName, bool enable)
        {
            var device = HomeState.GetDevice(deviceName);
            if (device != null)
            {
                if (enable)
                    device.Enable();
                else
                    device.Disable();
            }
        }

        public void SetDevicePower(string deviceName, double power)
        {
            var device = HomeState.GetDevice(deviceName);
            device?.UpdatePowerConsumption(power);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}