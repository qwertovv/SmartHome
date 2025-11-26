using SmartHome.Models;
using SmartHome.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly SmartHomeState _state;
        private readonly WPCalculator _wpCalculator;
        private readonly InvariantChecker _invariantChecker;

        public string ComfortStatus { get; set; }
        public string SecurityStatus { get; set; }
        public string EnergyStatus { get; set; }
        public int VariantValue => _invariantChecker.GetVariantValue();
        public bool SystemStable => _invariantChecker.GetInvariantStatus();

        public DashboardViewModel(SmartHomeState state, WPCalculator wpCalculator, InvariantChecker invariantChecker)
        {
            _state = state;
            _wpCalculator = wpCalculator;
            _invariantChecker = invariantChecker;

            UpdateDashboard();
        }

        public void UpdateDashboard()
        {
            ComfortStatus = _wpCalculator.CalculateComfortScenarioWP(_state);
            SecurityStatus = _wpCalculator.CalculateSecurityScenarioWP(_state);
            EnergyStatus = $"Потребление: {_state.EnergyConsumption:F1} кВт·ч";

            OnPropertyChanged(nameof(ComfortStatus));
            OnPropertyChanged(nameof(SecurityStatus));
            OnPropertyChanged(nameof(EnergyStatus));
            OnPropertyChanged(nameof(VariantValue));
            OnPropertyChanged(nameof(SystemStable));
        }

        public void CalculateOptimalScenario()
        {
            var result = _wpCalculator.CalculateOptimalHeatingWP(_state);
            // Показать результаты пользователю
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}