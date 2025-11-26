using SmartHome.Models;
using SmartHome.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.ViewModels
{
    public class AutomationViewModel : INotifyPropertyChanged
    {
        private readonly SmartHomeState _state;
        private readonly LogicEngine _logicEngine;
        private AutomationScenario _selectedScenario;
        private string _automationLogic;
        private string _logicResult;
        private string _truthTable;

        public ObservableCollection<AutomationScenario> Scenarios { get; set; }

        public AutomationScenario SelectedScenario
        {
            get => _selectedScenario;
            set
            {
                _selectedScenario = value;
                OnPropertyChanged();
                UpdateScenarioLogic();
            }
        }

        public string AutomationLogic
        {
            get => _automationLogic;
            set { _automationLogic = value; OnPropertyChanged(); }
        }

        public string LogicResult
        {
            get => _logicResult;
            set { _logicResult = value; OnPropertyChanged(); }
        }

        public string TruthTable
        {
            get => _truthTable;
            set { _truthTable = value; OnPropertyChanged(); }
        }

        public AutomationViewModel(SmartHomeState state, LogicEngine logicEngine)
        {
            _state = state;
            _logicEngine = logicEngine;
            InitializeScenarios();
        }

        private void InitializeScenarios()
        {
            Scenarios = new ObservableCollection<AutomationScenario>
            {
                new AutomationScenario
                {
                    Name = "Утренний режим",
                    Description = "Плавное пробуждение: свет, температура, музыка",
                    Icon = "🌅",
                    Color = "#F39C12",
                    IsActive = false
                },
                new AutomationScenario
                {
                    Name = "Рабочий режим",
                    Description = "Энергосбережение, безопасность, комфорт",
                    Icon = "💼",
                    Color = "#3498DB",
                    IsActive = false
                },
                new AutomationScenario
                {
                    Name = "Вечерний отдых",
                    Description = "Расслабляющая атмосфера, приглушенный свет",
                    Icon = "🌙",
                    Color = "#9B59B6",
                    IsActive = false
                },
                new AutomationScenario
                {
                    Name = "Ночной режим",
                    Description = "Безопасность, энергосбережение, тишина",
                    Icon = "😴",
                    Color = "#2C3E50",
                    IsActive = false
                },
                new AutomationScenario
                {
                    Name = "Режим отпуска",
                    Description = "Имитация присутствия, максимальная безопасность",
                    Icon = "✈️",
                    Color = "#E74C3C",
                    IsActive = false
                }
            };
        }

        // Активация сценария автоматизации (использует ЛР4)
        public void ActivateScenario(AutomationScenario scenario)
        {
            if (scenario == null) return;

            // Деактивируем все остальные сценарии
            foreach (var s in Scenarios)
            {
                s.IsActive = false;
            }

            scenario.IsActive = true;
            ApplyScenarioLogic(scenario);

            LogicResult = $"Сценарий '{scenario.Name}' активирован";
        }

        private void ApplyScenarioLogic(AutomationScenario scenario)
        {
            // Применение булевой логики (ЛР4) для сценариев
            switch (scenario.Name)
            {
                case "Утренний режим":
                    _state.TargetTemperature = 22.0;
                    _state.LightIntensity = 80.0;
                    _state.HeatingEnabled = true;
                    _state.AcEnabled = false;
                    EnableDevicesByType("Lighting");
                    break;

                case "Рабочий режим":
                    _state.TargetTemperature = 21.0;
                    _state.LightIntensity = 60.0;
                    _state.EnergySavingMode = true;
                    _state.AlarmArmed = true;
                    break;

                case "Вечерний отдых":
                    _state.TargetTemperature = 23.0;
                    _state.LightIntensity = 40.0;
                    _state.NightMode = true;
                    break;

                case "Ночной режим":
                    _state.TargetTemperature = 19.0;
                    _state.LightIntensity = 10.0;
                    _state.AlarmArmed = true;
                    _state.NightMode = true;
                    DisableDevicesByType("Lighting");
                    break;

                case "Режим отпуска":
                    _state.AwayMode = true;
                    _state.AlarmArmed = true;
                    _state.EnergySavingMode = true;
                    _state.TargetTemperature = 18.0;
                    break;
            }

            // Обновление логики для отображения
            UpdateScenarioLogic();
        }

        private void EnableDevicesByType(string type)
        {
            var devices = _state.Devices.Where(d => d.Type == type);
            foreach (var device in devices)
            {
                device.Enable();
            }
        }

        private void DisableDevicesByType(string type)
        {
            var devices = _state.Devices.Where(d => d.Type == type);
            foreach (var device in devices)
            {
                device.Disable();
            }
        }

        private void UpdateScenarioLogic()
        {
            if (SelectedScenario != null)
            {
                // Генерация булевых выражений для сценария (ЛР4)
                switch (SelectedScenario.Name)
                {
                    case "Утренний режим":
                        AutomationLogic = "Утро = (Время ∈ [6:00, 8:00]) ∧ (Движение_в_спальне ∨ Будильник)";
                        break;
                    case "Рабочий режим":
                        AutomationLogic = "Работа = (Время ∈ [9:00, 17:00]) ∧ (Нет_движения_30мин ∨ Режим_отпуска)";
                        break;
                    case "Вечерний отдых":
                        AutomationLogic = "Вечер = (Время ∈ [19:00, 23:00]) ∧ ¬Режим_отпуска ∧ Движение_в_гостиной";
                        break;
                    case "Ночной режим":
                        AutomationLogic = "Ночь = (Время ∈ [23:00, 6:00]) ∨ Режим_отпуска";
                        break;
                    case "Режим отпуска":
                        AutomationLogic = "Отпуск = (Активирован_пользователем) ∧ (Все_окна_закрыты ∧ Все_двери_закрыты)";
                        break;
                }
            }
        }

        // Методы для работы с булевой логикой (ЛР4)
        public void CalculateScenarioLogic()
        {
            if (SelectedScenario != null)
            {
                var result = _logicEngine.GetEnergyOptimizationLogic();
                TruthTable = result.table;
                LogicResult = $"Логика сценария '{SelectedScenario.Name}' рассчитана";
            }
        }

        public void GenerateDNFKNF()
        {
            var result = _logicEngine.GetEnergyOptimizationLogic();
            LogicResult = $"ДНФ: {result.dnf}\nКНФ: {result.knf}";
        }

        public void CheckLogicEquivalence()
        {
            bool isEquivalent = _logicEngine.CheckEquivalence(
                "Утро ∧ Вечер",
                "Вечер ∧ Утро"
            );
            LogicResult = isEquivalent ?
                "Выражения эквивалентны" :
                "Выражения не эквивалентны";
        }

        // Создание пользовательского сценария
        public void CreateCustomScenario(string name, string description, string logic)
        {
            var newScenario = new AutomationScenario
            {
                Name = name,
                Description = description,
                Icon = "⭐",
                Color = "#27AE60",
                IsActive = false
            };

            Scenarios.Add(newScenario);
            LogicResult = $"Пользовательский сценарий '{name}' создан";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}