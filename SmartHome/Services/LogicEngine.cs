using SmartHome.Models;

namespace SmartHome.Services
{
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
}