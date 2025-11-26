using SmartHome.Models;

namespace SmartHome.Services
{
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
}