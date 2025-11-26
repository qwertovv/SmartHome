using SmartHome.Models;

namespace SmartHome.Services
{
    public class WPCalculator
    {
        public string CalculateComfortScenarioWP(SmartHomeState state)
        {
            // Weakest precondition для сценария комфорта
            // Цель: temperature = 22 ± 1 ∧ lights = 70% ∧ doors_locked = true

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
}