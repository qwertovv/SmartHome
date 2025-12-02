using Xunit;
using System;
using System.Collections.Generic;

namespace SmartHome.Tests
{
    // Классы для тестирования (если их нет в основном проекте)
    public class SmartHomeState
    {
        public double Temperature { get; set; }
        public double TargetTemperature { get; set; }
        public int Humidity { get; set; }
        public bool AcEnabled { get; set; }
        public bool HeatingEnabled { get; set; }
        public bool HumidifierEnabled { get; set; }
        public bool AwayMode { get; set; }
        public bool AlarmArmed { get; set; }
        public bool FrontDoorLocked { get; set; }
        public bool BackDoorLocked { get; set; }
        public bool MotionDetected { get; set; }
        public bool NightMode { get; set; }
        public int LightIntensity { get; set; }
        public bool LivingRoomLight { get; set; }
        public bool KitchenLight { get; set; }
        public bool BedroomLight { get; set; }
        public double TotalPowerConsumption { get; set; }

        public void UpdateDevicePowerConsumption()
        {
            // Логика обновления энергопотребления
            TotalPowerConsumption = CalculateTotalPower();
        }

        private double CalculateTotalPower()
        {
            double total = 0;

            if (AcEnabled) total += 1500;
            if (HeatingEnabled) total += 2000;
            if (HumidifierEnabled) total += 50;
            if (LivingRoomLight) total += 100;
            if (KitchenLight) total += 100;
            if (BedroomLight) total += 80;

            return total;
        }

        public SmartDevice GetDevice(string deviceName)
        {
            // В реальной реализации здесь должна быть логика получения устройства
            return null;
        }
    }

    public class SmartDevice
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public double PowerConsumption { get; set; }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void UpdatePowerConsumption(double power)
        {
            PowerConsumption = power;
        }
    }

    // Модуль контрактного программирования
    public class ContractValidator
    {
        public (bool isValid, string errorMessage) ValidateHeatingOperation(SmartHomeState state)
        {
            // Pre-conditions для включения отопления
            if (state.Temperature >= 28)
                return (false, "Температура слишком высокая для отопления");

            if (state.AcEnabled)
                return (false, "Выключите кондиционер перед включением отопления");

            if (state.AwayMode)
                return (false, "Режим 'Нет дома' активен");

            return (true, "Условия выполнены");
        }

        public (bool isValid, string errorMessage) ValidateAlarmArming(SmartHomeState state)
        {
            // Pre-conditions для постановки на охрану
            if (!state.FrontDoorLocked || !state.BackDoorLocked)
                return (false, "Все двери должны быть закрыты");

            if (state.MotionDetected)
                return (false, "Обнаружено движение");

            return (true, "Можно ставить на охрану");
        }

        public (bool isValid, string errorMessage) ValidateAwayMode(SmartHomeState state)
        {
            // Pre-conditions для режима "Нет дома"
            if (state.LivingRoomLight || state.KitchenLight || state.BedroomLight)
                return (false, "Выключите свет перед активацией режима");

            if (!state.AlarmArmed)
                return (false, "Система должна быть на охране");

            return (true, "Режим 'Нет дома' активирован");
        }

        // Post-conditions проверяются после операций
        public bool VerifyHeatingPostConditions(SmartHomeState state)
        {
            return state.HeatingEnabled && state.Temperature <= state.TargetTemperature + 2;
        }
    }

    // WP-калькулятор
    public class WPCalculator
    {
        public string CalculateComfortScenarioWP(SmartHomeState state)
        {
            // Weakest precondition для сценария комфорта
            var conditions = new List<string>();

            if (state.Temperature < 18 || state.Temperature > 26)
                conditions.Add("Температура вне оптимального диапазона");

            if (!state.FrontDoorLocked)
                conditions.Add("Передняя дверь не закрыта");

            return conditions.Count == 0 ? "Условия выполнены" : string.Join(", ", conditions);
        }

        public string CalculateSecurityScenarioWP(SmartHomeState state)
        {
            // WP для сценария безопасности
            var conditions = new List<string>();

            if (state.MotionDetected)
                conditions.Add("Обнаружено движение");

            if (!state.FrontDoorLocked || !state.BackDoorLocked)
                conditions.Add("Не все двери закрыты");

            return conditions.Count == 0 ? "Безопасные условия" : $"Требуется: {string.Join(", ", conditions)}";
        }

        public (string steps, string result) CalculateOptimalHeatingWP(SmartHomeState state)
        {
            // Пошаговый расчет оптимального нагрева
            var steps = new List<string>
            {
                "Шаг 1: Проверка текущей температуры",
                "Шаг 2: Расчет разницы с целевой температурой",
                "Шаг 3: Учет энергосберегающего режима",
                "Шаг 4: Определение времени нагрева"
            };

            double diff = state.TargetTemperature - state.Temperature;
            string result = diff > 0 ?
                $"Требуется нагрев на {diff:F1}°C" :
                "Достигнута целевая температура";

            return (string.Join("\n", steps), result);
        }
    }

    // Проверка инвариантов
    public class InvariantChecker
    {
        private bool _invariantHeld = true;
        private int _variantValue = 100;

        public (bool holds, string message) CheckTemperatureInvariant(SmartHomeState state)
        {
            // Инвариант: температура в допустимых пределах
            bool invariant = state.Temperature >= 10 && state.Temperature <= 35;
            string message = invariant ?
                "Температура в норме" :
                "Температура вне допустимого диапазона!";

            _invariantHeld = invariant;
            return (invariant, message);
        }

        public (bool holds, string message) CheckSecurityInvariant(SmartHomeState state)
        {
            // Инвариант: если режим "Нет дома", то сигнализация включена и двери закрыты
            bool invariant = !state.AwayMode || (state.AlarmArmed && state.FrontDoorLocked && state.BackDoorLocked);
            string message = invariant ?
                "Система безопасности в норме" :
                "Нарушение безопасности: режим 'Нет дома' требует охраны!";

            return (invariant, message);
        }

        public void ExecuteClimateControlStep(SmartHomeState state)
        {
            // Вариант-функция: разница между целевой и текущей температурой
            _variantValue = (int)Math.Abs(state.TargetTemperature - state.Temperature);

            // Шаг цикла поддержания температуры
            if (state.HeatingEnabled && state.Temperature < state.TargetTemperature - 0.5)
            {
                state.Temperature += 0.5;
            }
            else if (state.AcEnabled && state.Temperature > state.TargetTemperature + 0.5)
            {
                state.Temperature -= 0.5;
            }
        }

        public int GetVariantValue() => _variantValue;
        public bool GetInvariantStatus() => _invariantHeld;
    }

    // Движок булевой логики
    public class LogicEngine
    {
        public string CalculateLightingLogic(SmartHomeState state)
        {
            // Булева логика для автоматического освещения
            bool autoLighting = (state.NightMode || state.LightIntensity < 30) &&
                               state.MotionDetected &&
                               !state.AwayMode;

            return autoLighting ?
                "Автоматическое освещение: ВКЛ" :
                "Автоматическое освещение: ВЫКЛ";
        }

        public string CalculateSecurityLogic(SmartHomeState state)
        {
            // Логика системы безопасности
            bool securityBreach = state.MotionDetected &&
                                 state.AwayMode &&
                                 !state.AlarmArmed;

            return securityBreach ?
                "⚠️ Обнаружена угроза безопасности!" :
                "Система безопасности: Норма";
        }

        public (string dnf, string knf, string table) GetEnergyOptimizationLogic()
        {
            // Генерация ДНФ/КНФ для оптимизации энергии
            string dnf = "(Ночной_режим ∧ Энергосбережение) ∨ (Режим_отпуска ∧ Все_устройства_выключены)";
            string knf = "(Ночной_режим ∨ Режим_отпуска) ∧ (Энергосбережение ∨ Все_устройства_выключены)";

            string truthTable =
                "Ночной | Отпуск | Энергосбережение | Результат\n" +
                "-------|--------|------------------|----------\n" +
                "   0   |   0    |        0         |     0\n" +
                "   1   |   0    |        1         |     1\n" +
                "   0   |   1    |        0         |     1\n" +
                "   1   |   1    |        1         |     1";

            return (dnf, knf, truthTable);
        }

        public bool CheckEquivalence(string expr1, string expr2)
        {
            // Упрощенная проверка эквивалентности булевых выражений
            return expr1.Replace(" ", "").Equals(expr2.Replace(" ", ""),
                   StringComparison.OrdinalIgnoreCase);
        }
    }

    // Тесты для модуля контрактного программирования
    public class ContractValidatorTests
    {
        [Fact]
        public void ValidateHeatingOperation_WhenTemperatureTooHigh_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                Temperature = 30.0,
                AcEnabled = false,
                AwayMode = false
            };

            // Act
            var result = validator.ValidateHeatingOperation(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Температура слишком высокая для отопления", result.errorMessage);
        }

        [Fact]
        public void ValidateHeatingOperation_WhenAcEnabled_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                Temperature = 22.0,
                AcEnabled = true,
                AwayMode = false
            };

            // Act
            var result = validator.ValidateHeatingOperation(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Выключите кондиционер перед включением отопления", result.errorMessage);
        }

        [Fact]
        public void ValidateHeatingOperation_WhenAwayModeActive_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                Temperature = 22.0,
                AcEnabled = false,
                AwayMode = true
            };

            // Act
            var result = validator.ValidateHeatingOperation(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Режим 'Нет дома' активен", result.errorMessage);
        }

        [Fact]
        public void ValidateHeatingOperation_WhenAllConditionsMet_ReturnsTrue()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                Temperature = 20.0,
                AcEnabled = false,
                AwayMode = false
            };

            // Act
            var result = validator.ValidateHeatingOperation(state);

            // Assert
            Assert.True(result.isValid);
            Assert.Equal("Условия выполнены", result.errorMessage);
        }

        [Fact]
        public void ValidateAlarmArming_WhenDoorsOpen_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                FrontDoorLocked = false,
                BackDoorLocked = true,
                MotionDetected = false
            };

            // Act
            var result = validator.ValidateAlarmArming(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Все двери должны быть закрыты", result.errorMessage);
        }

        [Fact]
        public void ValidateAlarmArming_WhenMotionDetected_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                FrontDoorLocked = true,
                BackDoorLocked = true,
                MotionDetected = true
            };

            // Act
            var result = validator.ValidateAlarmArming(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Обнаружено движение", result.errorMessage);
        }

        [Fact]
        public void ValidateAwayMode_WhenLightsOn_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                LivingRoomLight = true,
                KitchenLight = false,
                BedroomLight = false,
                AlarmArmed = true
            };

            // Act
            var result = validator.ValidateAwayMode(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Выключите свет перед активацией режима", result.errorMessage);
        }

        [Fact]
        public void ValidateAwayMode_WhenAlarmNotArmed_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                LivingRoomLight = false,
                KitchenLight = false,
                BedroomLight = false,
                AlarmArmed = false
            };

            // Act
            var result = validator.ValidateAwayMode(state);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Система должна быть на охране", result.errorMessage);
        }

        [Fact]
        public void ValidateAwayMode_WhenAllConditionsMet_ReturnsTrue()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                LivingRoomLight = false,
                KitchenLight = false,
                BedroomLight = false,
                AlarmArmed = true
            };

            // Act
            var result = validator.ValidateAwayMode(state);

            // Assert
            Assert.True(result.isValid);
            Assert.Equal("Режим 'Нет дома' активирован", result.errorMessage);
        }

        [Fact]
        public void VerifyHeatingPostConditions_WhenHeatingEnabledAndTemperatureWithinRange_ReturnsTrue()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                HeatingEnabled = true,
                Temperature = 22.0,
                TargetTemperature = 20.0
            };

            // Act
            var result = validator.VerifyHeatingPostConditions(state);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHeatingPostConditions_WhenHeatingEnabledAndTemperatureTooHigh_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                HeatingEnabled = true,
                Temperature = 25.0,
                TargetTemperature = 20.0
            };

            // Act
            var result = validator.VerifyHeatingPostConditions(state);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyHeatingPostConditions_WhenHeatingDisabled_ReturnsFalse()
        {
            // Arrange
            var validator = new ContractValidator();
            var state = new SmartHomeState
            {
                HeatingEnabled = false,
                Temperature = 20.0,
                TargetTemperature = 20.0
            };

            // Act
            var result = validator.VerifyHeatingPostConditions(state);

            // Assert
            Assert.False(result);
        }
    }

    // Тесты для модуля weakest precondition
    public class WPCalculatorTests
    {
        [Fact]
        public void CalculateComfortScenarioWP_WhenTemperatureInRangeAndDoorLocked_ReturnsSuccess()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                Temperature = 22.0,
                FrontDoorLocked = true
            };

            // Act
            var result = calculator.CalculateComfortScenarioWP(state);

            // Assert
            Assert.Equal("Условия выполнены", result);
        }

        [Fact]
        public void CalculateComfortScenarioWP_WhenTemperatureTooLow_ReturnsWarning()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                Temperature = 15.0,
                FrontDoorLocked = true
            };

            // Act
            var result = calculator.CalculateComfortScenarioWP(state);

            // Assert
            Assert.Contains("Температура вне оптимального диапазона", result);
        }

        [Fact]
        public void CalculateComfortScenarioWP_WhenTemperatureTooHigh_ReturnsWarning()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                Temperature = 28.0,
                FrontDoorLocked = true
            };

            // Act
            var result = calculator.CalculateComfortScenarioWP(state);

            // Assert
            Assert.Contains("Температура вне оптимального диапазона", result);
        }

        [Fact]
        public void CalculateComfortScenarioWP_WhenDoorUnlocked_ReturnsWarning()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                Temperature = 22.0,
                FrontDoorLocked = false
            };

            // Act
            var result = calculator.CalculateComfortScenarioWP(state);

            // Assert
            Assert.Contains("Передняя дверь не закрыта", result);
        }

        [Fact]
        public void CalculateComfortScenarioWP_WhenMultipleConditionsViolated_ReturnsAllWarnings()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                Temperature = 30.0,
                FrontDoorLocked = false
            };

            // Act
            var result = calculator.CalculateComfortScenarioWP(state);

            // Assert
            Assert.Contains("Температура вне оптимального диапазона", result);
            Assert.Contains("Передняя дверь не закрыта", result);
        }

        [Fact]
        public void CalculateSecurityScenarioWP_WhenMotionDetected_ReturnsWarning()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                MotionDetected = true,
                FrontDoorLocked = true,
                BackDoorLocked = true
            };

            // Act
            var result = calculator.CalculateSecurityScenarioWP(state);

            // Assert
            Assert.Contains("Обнаружено движение", result);
        }

        [Fact]
        public void CalculateSecurityScenarioWP_WhenDoorsOpen_ReturnsWarning()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                MotionDetected = false,
                FrontDoorLocked = false,
                BackDoorLocked = false
            };

            // Act
            var result = calculator.CalculateSecurityScenarioWP(state);

            // Assert
            Assert.Contains("Не все двери закрыты", result);
        }

        [Fact]
        public void CalculateSecurityScenarioWP_WhenAllConditionsMet_ReturnsSuccess()
        {
            // Arrange
            var calculator = new WPCalculator();
            var state = new SmartHomeState
            {
                MotionDetected = false,
                FrontDoorLocked = true,
                BackDoorLocked = true
            };

            // Act
            var result = calculator.CalculateSecurityScenarioWP(state);

            // Assert
            Assert.Equal("Безопасные условия", result);
        }





        // Тесты для модуля инвариантов
        public class InvariantCheckerTests
        {
            [Fact]
            public void CheckTemperatureInvariant_ValidatesTemperatureRange_ForMinimumValue()
            {
                // Arrange
                var checker = new InvariantChecker();
                var state = new SmartHomeState { Temperature = 10.0 };

                // Act
                var result = checker.CheckTemperatureInvariant(state);

                // Assert
                Assert.True(result.holds);
                Assert.Equal("Температура в норме", result.message);
            }

            [Fact]
            public void CheckTemperatureInvariant_ValidatesTemperatureRange_ForBelowMinimum()
            {
                // Arrange
                var checker = new InvariantChecker();
                var state = new SmartHomeState { Temperature = 9.9 };

                // Act
                var result = checker.CheckTemperatureInvariant(state);

                // Assert
                Assert.False(result.holds);
                Assert.Equal("Температура вне допустимого диапазона!", result.message);
            }

            [Fact]
            public void CheckTemperatureInvariant_ValidatesTemperatureRange_ForNormalValue()
            {
                // Arrange
                var checker = new InvariantChecker();
                var state = new SmartHomeState { Temperature = 22.5 };

                // Act
                var result = checker.CheckTemperatureInvariant(state);
            }
        }
    }
}

           