using SmartHome.Models;

namespace SmartHome.Services
{
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
}