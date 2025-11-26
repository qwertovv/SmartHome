using System;
using System.Windows;
using System.Windows.Media;

namespace SmartHome
{
    public partial class MainWindow : Window
    {
        private bool isAlarmOn = false;
        private bool isAirConditionerOn = false;
        private bool isTVOn = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Освещение
        private void LivingRoomLight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💡 Свет в гостиной переключен", "Освещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void KitchenLight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💡 Свет на кухне переключен", "Освещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BedroomLight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💡 Свет в спальне переключен", "Освещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AllLightsOff_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🔌 Весь свет выключен", "Освещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Климат-контроль
        private void TemperatureUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🌡️ Температура повышена на 1°C", "Климат-контроль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TemperatureDown_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🌡️ Температура понижена на 1°C", "Климат-контроль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ToggleAirConditioner_Click(object sender, RoutedEventArgs e)
        {
            isAirConditionerOn = !isAirConditionerOn;
            string status = isAirConditionerOn ? "включен" : "выключен";
            MessageBox.Show($"❄️ Кондиционер {status}", "Климат-контроль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Безопасность
        private void ToggleAlarm_Click(object sender, RoutedEventArgs e)
        {
            isAlarmOn = !isAlarmOn;
            string status = isAlarmOn ? "включена" : "выключена";
            MessageBox.Show($"🚨 Сигнализация {status}", "Безопасность", MessageBoxButton.OK,
                          isAlarmOn ? MessageBoxImage.Warning : MessageBoxImage.Information);
        }

        private void ShowCameras_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📹 Открыт просмотр камер наблюдения", "Безопасность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckDoorsStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🚪 Все двери закрыты", "Безопасность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Мультимедиа
        private void PlayMusic_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🎵 Музыка включена", "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ToggleTV_Click(object sender, RoutedEventArgs e)
        {
            isTVOn = !isTVOn;
            string status = isTVOn ? "включен" : "выключен";
            MessageBox.Show($"📺 Телевизор {status}", "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CinemaMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🎬 Кино-режим активирован:\n• Приглушен свет\n• Включен телевизор\n• Закрыты шторы",
                          "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Сценарии
        private void MorningScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🌅 Утренний сценарий:\n• Постепенное включение света\n• Включение кофеварки\n• Погода на день",
                          "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EveningScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🌆 Вечерний сценарий:\n• Приглушенный свет\n• Расслабляющая музыка\n• Подготовка ко сну",
                          "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NightScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🌙 Ночной сценарий:\n• Выключен весь свет\n• Включена сигнализация\n• Подготовка к утру",
                          "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GuestsScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("👥 Сценарий 'Гости':\n• Яркое освещение\n• Фоновая музыка\n• Комфортная температура",
                          "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Нижняя панель
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("⚙️ Открыты настройки системы", "Настройки", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📊 Статистика энергопотребления:\n• Электричество: 45 кВт/ч\n• Вода: 12 м³\n• Отопление: норма",
                          "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("❓ Помощь по системе Умный Дом\n\nДля настройки обратитесь к администратору системы.",
                          "Помощь", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}