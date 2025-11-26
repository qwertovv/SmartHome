using SmartHome.Models;
using SmartHome.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.ViewModels
{
    public class SecurityViewModel : INotifyPropertyChanged
    {
        private readonly SmartHomeState _state;
        private readonly ContractValidator _contractValidator;
        private readonly LogicEngine _logicEngine;
        private string _securityStatus;
        private string _threatLevel;
        private bool _isSecurityValid;
        private string _securityLogic;

        public string SecurityStatus
        {
            get => _securityStatus;
            set { _securityStatus = value; OnPropertyChanged(); }
        }

        public string ThreatLevel
        {
            get => _threatLevel;
            set { _threatLevel = value; OnPropertyChanged(); }
        }

        public bool IsSecurityValid
        {
            get => _isSecurityValid;
            set { _isSecurityValid = value; OnPropertyChanged(); }
        }

        public string SecurityLogic
        {
            get => _securityLogic;
            set { _securityLogic = value; OnPropertyChanged(); }
        }

        public SecurityViewModel(SmartHomeState state, ContractValidator contractValidator, LogicEngine logicEngine)
        {
            _state = state;
            _contractValidator = contractValidator;
            _logicEngine = logicEngine;
            UpdateSecurityStatus();
        }

        // Постановка на охрану с проверкой контракта (ЛР1)
        public void ArmAlarmSystem()
        {
            var validationResult = _contractValidator.ValidateAlarmArming(_state);

            if (validationResult.isValid)
            {
                _state.AlarmArmed = true;
                SecurityStatus = "Система поставлена на охрану";
                ThreatLevel = "Низкий";
                IsSecurityValid = true;

                // Проверка постусловий (ЛР1)
                if (!_state.AlarmArmed)
                {
                    SecurityStatus = "Ошибка: система не поставлена на охрану";
                    IsSecurityValid = false;
                }
            }
            else
            {
                SecurityStatus = $"Не удалось поставить на охрану: {validationResult.errorMessage}";
                ThreatLevel = "Высокий";
                IsSecurityValid = false;
            }

            UpdateSecurityLogic();
        }

        // Снятие с охраны
        public void DisarmAlarmSystem()
        {
            _state.AlarmArmed = false;
            SecurityStatus = "Система снята с охраны";
            ThreatLevel = "Низкий";
            IsSecurityValid = true;
            UpdateSecurityLogic();
        }

        // Блокировка всех дверей
        public void LockAllDoors()
        {
            _state.FrontDoorLocked = true;
            _state.BackDoorLocked = true;

            // Обновление устройств-замков
            var lockDevices = _state.Devices.Where(d => d.Type == "Security" && d.Name.Contains("Замок"));
            foreach (var lockDevice in lockDevices)
            {
                lockDevice.Enable();
            }

            SecurityStatus = "Все двери заблокированы";
            UpdateSecurityLogic();
        }

        // Разблокировка всех дверей
        public void UnlockAllDoors()
        {
            if (_state.AlarmArmed)
            {
                SecurityStatus = "Сначала снимите систему с охраны";
                return;
            }

            _state.FrontDoorLocked = false;
            _state.BackDoorLocked = false;

            var lockDevices = _state.Devices.Where(d => d.Type == "Security" && d.Name.Contains("Замок"));
            foreach (var lockDevice in lockDevices)
            {
                lockDevice.Disable();
            }

            SecurityStatus = "Все двери разблокированы";
            UpdateSecurityLogic();
        }

        // Активация режима "Нет дома" (использует ЛР1 и ЛР4)
        public void ActivateAwayMode()
        {
            var validationResult = _contractValidator.ValidateAwayMode(_state);

            if (validationResult.isValid)
            {
                _state.AwayMode = true;
                _state.AlarmArmed = true;
                LockAllDoors();

                // Выключение ненужных устройств
                var lights = _state.Devices.Where(d => d.Type == "Lighting");
                foreach (var light in lights)
                {
                    light.Disable();
                }

                SecurityStatus = "Режим 'Нет дома' активирован";
                ThreatLevel = "Средний";
                IsSecurityValid = true;
            }
            else
            {
                SecurityStatus = $"Не удалось активировать режим: {validationResult.errorMessage}";
                ThreatLevel = "Высокий";
                IsSecurityValid = false;
            }

            UpdateSecurityLogic();
        }

        // Деактивация режима "Нет дома"
        public void DeactivateAwayMode()
        {
            _state.AwayMode = false;
            SecurityStatus = "Режим 'Нет дома' деактивирован";
            ThreatLevel = "Низкий";
            UpdateSecurityLogic();
        }

        // Проверка безопасности (использует ЛР4)
        public void PerformSecurityCheck()
        {
            var securityCheck = _logicEngine.CalculateSecurityLogic(_state);
            SecurityStatus = securityCheck;

            if (securityCheck.Contains("угроза"))
            {
                ThreatLevel = "Критический";
                IsSecurityValid = false;
            }
            else
            {
                ThreatLevel = "Низкий";
                IsSecurityValid = true;
            }

            UpdateSecurityLogic();
        }

        // Проверка инвариантов безопасности (ЛР3)
        public void CheckSecurityInvariants()
        {
            var invariantChecker = new InvariantChecker();
            var result = invariantChecker.CheckSecurityInvariant(_state);

            SecurityStatus = result.message;
            IsSecurityValid = result.holds;
            ThreatLevel = result.holds ? "Низкий" : "Высокий";
        }

        // Обновление булевой логики безопасности (ЛР4)
        private void UpdateSecurityLogic()
        {
            SecurityLogic = _logicEngine.CalculateSecurityLogic(_state);
        }

        // Обновление статуса безопасности
        private void UpdateSecurityStatus()
        {
            if (_state.AlarmArmed && _state.FrontDoorLocked && _state.BackDoorLocked)
            {
                SecurityStatus = "Система безопасности активна";
                ThreatLevel = "Низкий";
                IsSecurityValid = true;
            }
            else if (_state.MotionDetected && _state.AwayMode)
            {
                SecurityStatus = "⚠️ Обнаружена потенциальная угроза!";
                ThreatLevel = "Высокий";
                IsSecurityValid = false;
            }
            else
            {
                SecurityStatus = "Система безопасности неактивна";
                ThreatLevel = "Средний";
                IsSecurityValid = false;
            }

            UpdateSecurityLogic();
        }

        // Симуляция события безопасности
        public void SimulateSecurityEvent(string eventType)
        {
            switch (eventType)
            {
                case "motion":
                    _state.MotionDetected = true;
                    SecurityStatus = "⚠️ Обнаружено движение!";
                    ThreatLevel = "Высокий";
                    break;
                case "door":
                    _state.FrontDoorLocked = false;
                    SecurityStatus = "⚠️ Передняя дверь открыта!";
                    ThreatLevel = "Высокий";
                    break;
                case "normal":
                    _state.MotionDetected = false;
                    _state.FrontDoorLocked = true;
                    SecurityStatus = "Все системы в норме";
                    ThreatLevel = "Низкий";
                    break;
            }

            UpdateSecurityLogic();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}