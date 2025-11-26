using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.Models
{
    public class Device : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _type;
        private string _location;
        private bool _isOnline;
        private bool _isEnabled;
        private double _powerConsumption;
        private string _status;
        private string _icon;

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public bool IsOnline
        {
            get => _isOnline;
            set { _isOnline = value; OnPropertyChanged(); }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; OnPropertyChanged(); }
        }

        public double PowerConsumption
        {
            get => _powerConsumption;
            set { _powerConsumption = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public string Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(); }
        }

        // Конструктор
        public Device(string id, string name, string type, string location, string icon = "🔘")
        {
            Id = id;
            Name = name;
            Type = type;
            Location = location;
            Icon = icon;
            IsOnline = true;
            IsEnabled = false;
            PowerConsumption = 0;
            Status = "Отключено";
        }

        // Методы для управления устройством
        public void Enable()
        {
            IsEnabled = true;
            Status = "Включено";
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(Status));
        }

        public void Disable()
        {
            IsEnabled = false;
            Status = "Отключено";
            PowerConsumption = 0;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(PowerConsumption));
        }

        public void UpdatePowerConsumption(double power)
        {
            PowerConsumption = power;
            OnPropertyChanged(nameof(PowerConsumption));
        }

        public void SetOffline()
        {
            IsOnline = false;
            Status = "Оффлайн";
            OnPropertyChanged(nameof(IsOnline));
            OnPropertyChanged(nameof(Status));
        }

        public void SetOnline()
        {
            IsOnline = true;
            Status = IsEnabled ? "Включено" : "Отключено";
            OnPropertyChanged(nameof(IsOnline));
            OnPropertyChanged(nameof(Status));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Name} ({Location}) - {Status}";
        }
    }

    // Статический класс для создания предустановленных устройств
    public static class DeviceFactory
    {
        public static Device CreateThermostat(string location = "Гостиная")
        {
            return new Device("thermostat_1", "Умный термостат", "Climate", location, "🌡️");
        }

        public static Device CreateLight(string name, string location)
        {
            return new Device($"light_{name.ToLower()}", name, "Lighting", location, "💡");
        }

        public static Device CreateSmartLock(string location)
        {
            return new Device($"lock_{location.ToLower()}", $"Замок {location}", "Security", location, "🔒");
        }

        public static Device CreateMotionSensor(string location)
        {
            return new Device($"sensor_{location.ToLower()}", $"Датчик движения {location}", "Sensor", location, "📡");
        }

        public static Device CreateCamera(string location)
        {
            return new Device($"camera_{location.ToLower()}", $"Камера {location}", "Security", location, "📹");
        }

        public static Device CreateSmartPlug(string name, string location)
        {
            return new Device($"plug_{name.ToLower()}", name, "Plug", location, "🔌");
        }

        public static Device CreateBlinds(string location)
        {
            return new Device($"blinds_{location.ToLower()}", $"Жалюзи {location}", "Window", location, "🪟");
        }
    }
}