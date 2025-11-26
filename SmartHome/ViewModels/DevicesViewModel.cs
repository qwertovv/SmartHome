using SmartHome.Models;
using SmartHome.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.ViewModels
{
    public class DevicesViewModel : INotifyPropertyChanged
    {
        private readonly SmartHomeState _state;
        private readonly ContractValidator _contractValidator;
        private Device _selectedDevice;
        private string _deviceStatus;
        private bool _isDeviceOperationValid;

        public ObservableCollection<Device> Devices => _state.Devices;

        public Device SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
                UpdateDeviceStatus();
            }
        }

        public string DeviceStatus
        {
            get => _deviceStatus;
            set { _deviceStatus = value; OnPropertyChanged(); }
        }

        public bool IsDeviceOperationValid
        {
            get => _isDeviceOperationValid;
            set { _isDeviceOperationValid = value; OnPropertyChanged(); }
        }

        public DevicesViewModel(SmartHomeState state, ContractValidator contractValidator)
        {
            _state = state;
            _contractValidator = contractValidator;
            DeviceStatus = "Выберите устройство для управления";
        }

        // Включение устройства с проверкой контракта (ЛР1)
        public void EnableDevice(Device device)
        {
            if (device == null) return;

            // Проверка предусловий через контракт (ЛР1)
            var validationResult = ValidateDeviceOperation(device, true);

            if (validationResult.isValid)
            {
                device.Enable();
                DeviceStatus = $"Устройство '{device.Name}' включено";
                IsDeviceOperationValid = true;

                // Проверка постусловий (ЛР1)
                if (!device.IsEnabled)
                {
                    DeviceStatus = "Ошибка: устройство не включилось";
                    IsDeviceOperationValid = false;
                }
            }
            else
            {
                DeviceStatus = $"Не удалось включить: {validationResult.errorMessage}";
                IsDeviceOperationValid = false;
            }
        }

        // Выключение устройства
        public void DisableDevice(Device device)
        {
            if (device == null) return;

            var validationResult = ValidateDeviceOperation(device, false);

            if (validationResult.isValid)
            {
                device.Disable();
                DeviceStatus = $"Устройство '{device.Name}' выключено";
                IsDeviceOperationValid = true;
            }
            else
            {
                DeviceStatus = $"Не удалось выключить: {validationResult.errorMessage}";
                IsDeviceOperationValid = false;
            }
        }

        // Валидация операций с устройствами (ЛР1)
        private (bool isValid, string errorMessage) ValidateDeviceOperation(Device device, bool enable)
        {
            // Базовые проверки
            if (!device.IsOnline)
                return (false, "устройство оффлайн");

            // Специфичные проверки для разных типов устройств
            switch (device.Type)
            {
                case "Climate":
                    if (enable && _state.AwayMode)
                        return (false, "режим 'Нет дома' активен");
                    break;

                case "Security":
                    if (!enable && _state.AlarmArmed)
                        return (false, "сначала снимите с охраны");
                    break;

                case "Lighting":
                    if (enable && _state.EnergySavingMode && _state.LightIntensity > 80)
                        return (false, "энергосберегающий режим не позволяет высокую яркость");
                    break;
            }

            return (true, "условия выполнены");
        }

        // Групповые операции (используют ЛР2 для планирования)
        public void EnableAllLights()
        {
            var lights = Devices.Where(d => d.Type == "Lighting");
            foreach (var light in lights)
            {
                EnableDevice(light);
            }
            DeviceStatus = "Все осветительные приборы включены";
        }

        public void DisableAllLights()
        {
            var lights = Devices.Where(d => d.Type == "Lighting");
            foreach (var light in lights)
            {
                DisableDevice(light);
            }
            DeviceStatus = "Все осветительные приборы выключены";
        }

        public void SetOptimalClimate()
        {
            // Использование WP-калькулятора (ЛР2) для оптимальных настроек
            var optimalTemp = _state.EnergySavingMode ? 20.0 : 22.0;
            _state.TargetTemperature = optimalTemp;

            var climateDevices = Devices.Where(d => d.Type == "Climate");
            foreach (var device in climateDevices)
            {
                EnableDevice(device);
            }

            DeviceStatus = $"Оптимальный климат установлен: {optimalTemp}°C";
        }

        private void UpdateDeviceStatus()
        {
            if (SelectedDevice != null)
            {
                DeviceStatus = $"{SelectedDevice.Name}: {SelectedDevice.Status}, " +
                              $"Потребление: {SelectedDevice.PowerConsumption:F2} кВт";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}