using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.Models
{
    public class SmartHomeState : INotifyPropertyChanged
    {
        // Существующие свойства (температура, освещение, безопасность, энергия, режимы)
        private double _temperature = 22.0;
        private double _targetTemperature = 22.0;
        private double _humidity = 45.0;
        private bool _heatingEnabled = false;
        private bool _acEnabled = false;

        private bool _livingRoomLight = false;
        private bool _kitchenLight = false;
        private bool _bedroomLight = false;
        private double _lightIntensity = 70.0;

        private bool _frontDoorLocked = true;
        private bool _backDoorLocked = true;
        private bool _alarmArmed = false;
        private bool _motionDetected = false;

        private double _energyConsumption = 2.5;
        private bool _energySavingMode = true;

        private string _currentMode = "Нормальный";
        private bool _awayMode = false;
        private bool _nightMode = false;

        // Новая коллекция устройств
        private ObservableCollection<Device> _devices;

        public ObservableCollection<Device> Devices
        {
            get => _devices;
            set { _devices = value; OnPropertyChanged(); }
        }

        // Существующие свойства
        public double Temperature { get => _temperature; set { _temperature = value; OnPropertyChanged(); } }
        public double TargetTemperature { get => _targetTemperature; set { _targetTemperature = value; OnPropertyChanged(); } }
        public double Humidity { get => _humidity; set { _humidity = value; OnPropertyChanged(); } }
        public bool HeatingEnabled { get => _heatingEnabled; set { _heatingEnabled = value; OnPropertyChanged(); } }
        public bool AcEnabled { get => _acEnabled; set { _acEnabled = value; OnPropertyChanged(); } }
        public bool LivingRoomLight { get => _livingRoomLight; set { _livingRoomLight = value; OnPropertyChanged(); } }
        public bool KitchenLight { get => _kitchenLight; set { _kitchenLight = value; OnPropertyChanged(); } }
        public bool BedroomLight { get => _bedroomLight; set { _bedroomLight = value; OnPropertyChanged(); } }
        public double LightIntensity { get => _lightIntensity; set { _lightIntensity = value; OnPropertyChanged(); } }
        public bool FrontDoorLocked { get => _frontDoorLocked; set { _frontDoorLocked = value; OnPropertyChanged(); } }
        public bool BackDoorLocked { get => _backDoorLocked; set { _backDoorLocked = value; OnPropertyChanged(); } }
        public bool AlarmArmed { get => _alarmArmed; set { _alarmArmed = value; OnPropertyChanged(); } }
        public bool MotionDetected { get => _motionDetected; set { _motionDetected = value; OnPropertyChanged(); } }
        public double EnergyConsumption { get => _energyConsumption; set { _energyConsumption = value; OnPropertyChanged(); } }
        public bool EnergySavingMode { get => _energySavingMode; set { _energySavingMode = value; OnPropertyChanged(); } }
        public string CurrentMode { get => _currentMode; set { _currentMode = value; OnPropertyChanged(); } }
        public bool AwayMode { get => _awayMode; set { _awayMode = value; OnPropertyChanged(); } }
        public bool NightMode { get => _nightMode; set { _nightMode = value; OnPropertyChanged(); } }

        public SmartHomeState()
        {
            // Инициализация коллекции устройств
            Devices = new ObservableCollection<Device>
            {
                DeviceFactory.CreateThermostat("Гостиная"),
                DeviceFactory.CreateLight("Основной свет", "Гостиная"),
                DeviceFactory.CreateLight("Кухонный свет", "Кухня"),
                DeviceFactory.CreateLight("Прикроватный свет", "Спальня"),
                DeviceFactory.CreateSmartLock("Передняя дверь"),
                DeviceFactory.CreateSmartLock("Задняя дверь"),
                DeviceFactory.CreateMotionSensor("Коридор"),
                DeviceFactory.CreateCamera("Вход"),
                DeviceFactory.CreateSmartPlug("Телевизор", "Гостиная"),
                DeviceFactory.CreateSmartPlug("Кофеварка", "Кухня"),
                DeviceFactory.CreateBlinds("Гостиная")
            };

            // Установим начальные состояния для некоторых устройств
            GetDevice("Умный термостат")?.Enable();
            GetDevice("Замок Передняя дверь")?.Enable();
            GetDevice("Замок Задняя дверь")?.Enable();
        }

        // Вспомогательные методы для работы с устройствами
        public Device GetDevice(string name)
        {
            return Devices.FirstOrDefault(d => d.Name == name);
        }

        public IEnumerable<Device> GetDevicesByType(string type)
        {
            return Devices.Where(d => d.Type == type);
        }

        public IEnumerable<Device> GetDevicesByLocation(string location)
        {
            return Devices.Where(d => d.Location == location);
        }

        public double GetTotalPowerConsumption()
        {
            return Devices.Sum(d => d.PowerConsumption);
        }

        public int GetOnlineDevicesCount()
        {
            return Devices.Count(d => d.IsOnline);
        }

        public void UpdateDevicePowerConsumption()
        {
            // Обновляем потребление энергии на основе состояний устройств
            foreach (var device in Devices)
            {
                if (device.IsEnabled && device.IsOnline)
                {
                    // Базовая логика расчета потребления
                    double power = 0;
                    switch (device.Type)
                    {
                        case "Lighting":
                            power = 0.1 * (LightIntensity / 100);
                            break;
                        case "Climate":
                            power = HeatingEnabled || AcEnabled ? 2.0 : 0.1;
                            break;
                        case "Security":
                            power = 0.05;
                            break;
                        case "Plug":
                            power = 0.3;
                            break;
                        default:
                            power = 0.05;
                            break;
                    }
                    device.UpdatePowerConsumption(power);
                }
                else
                {
                    device.UpdatePowerConsumption(0);
                }
            }

            // Обновляем общее потребление
            EnergyConsumption = GetTotalPowerConsumption();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}