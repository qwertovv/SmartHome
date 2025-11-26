using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SmartHome
{
    public partial class MainWindow : Window
    {
        // Состояния устройств
        private bool isLivingRoomLightOn = false;
        private bool isKitchenLightOn = false;
        private bool isBedroomLightOn = false;
        private bool isBathroomLightOn = false;
        private bool isFacadeLightOn = false;
        private bool isGardenLightOn = false;

        private bool isAlarmOn = false;
        private bool isAirConditionerOn = false;
        private bool isHeaterOn = false;
        private bool isHumidifierOn = false;
        private bool isTVOn = false;
        private bool isAudioOn = false;
        private bool isProjectorOn = false;

        // Показания датчиков
        private double currentTemperature = 22.0;
        private int currentHumidity = 45;
        private string currentAirQuality = "Хорошо";

        // Таймер для обновления показаний
        private DispatcherTimer sensorTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSensors();
        }

        private void InitializeSensors()
        {
            // Инициализация таймера для обновления показаний датчиков
            sensorTimer = new DispatcherTimer();
            sensorTimer.Interval = TimeSpan.FromSeconds(2);
            sensorTimer.Tick += SensorTimer_Tick;
            sensorTimer.Start();
        }

        private void SensorTimer_Tick(object sender, EventArgs e)
        {
            // Имитация изменения показаний датчиков
            Random random = new Random();

            // Температура немного колеблется
            if (isAirConditionerOn)
                currentTemperature -= 0.1;
            else if (isHeaterOn)
                currentTemperature += 0.1;
            else
                currentTemperature += (random.NextDouble() - 0.5) * 0.2;

            // Ограничиваем температуру
            currentTemperature = Math.Max(18, Math.Min(28, currentTemperature));

            // Влажность немного колеблется
            if (isHumidifierOn)
                currentHumidity = Math.Min(70, currentHumidity + 1);
            else
                currentHumidity += random.Next(-1, 2);

            currentHumidity = Math.Max(30, Math.Min(70, currentHumidity));

            // Обновляем отображение
            UpdateSensorDisplay();
        }

        private void UpdateSensorDisplay()
        {
            TemperatureValue.Text = $"{currentTemperature:0.0}°C";
            HumidityValue.Text = $"{currentHumidity}%";
            AirQualityValue.Text = currentAirQuality;
        }

        // Освещение
        private void LivingRoomLight_Click(object sender, RoutedEventArgs e)
        {
            isLivingRoomLightOn = !isLivingRoomLightOn;
            LivingRoomLightIndicator.Fill = isLivingRoomLightOn ? Brushes.Green : Brushes.Red;
            LivingRoomLightIndicator.ToolTip = isLivingRoomLightOn ? "Свет включен" : "Свет выключен";
        }

        private void KitchenLight_Click(object sender, RoutedEventArgs e)
        {
            isKitchenLightOn = !isKitchenLightOn;
            KitchenLightIndicator.Fill = isKitchenLightOn ? Brushes.Green : Brushes.Red;
            KitchenLightIndicator.ToolTip = isKitchenLightOn ? "Свет включен" : "Свет выключен";
        }

        private void BedroomLight_Click(object sender, RoutedEventArgs e)
        {
            isBedroomLightOn = !isBedroomLightOn;
            BedroomLightIndicator.Fill = isBedroomLightOn ? Brushes.Green : Brushes.Red;
            BedroomLightIndicator.ToolTip = isBedroomLightOn ? "Свет включен" : "Свет выключен";
        }

        private void BathroomLight_Click(object sender, RoutedEventArgs e)
        {
            isBathroomLightOn = !isBathroomLightOn;
            BathroomLightIndicator.Fill = isBathroomLightOn ? Brushes.Green : Brushes.Red;
            BathroomLightIndicator.ToolTip = isBathroomLightOn ? "Свет включен" : "Свет выключен";
        }

        private void FacadeLight_Click(object sender, RoutedEventArgs e)
        {
            isFacadeLightOn = !isFacadeLightOn;
            FacadeLightIndicator.Fill = isFacadeLightOn ? Brushes.Green : Brushes.Red;
            FacadeLightIndicator.ToolTip = isFacadeLightOn ? "Свет включен" : "Свет выключен";
        }

        private void GardenLight_Click(object sender, RoutedEventArgs e)
        {
            isGardenLightOn = !isGardenLightOn;
            GardenLightIndicator.Fill = isGardenLightOn ? Brushes.Green : Brushes.Red;
            GardenLightIndicator.ToolTip = isGardenLightOn ? "Свет включен" : "Свет выключен";
        }

        private void AllLightsOff_Click(object sender, RoutedEventArgs e)
        {
            // Выключаем все света
            isLivingRoomLightOn = isKitchenLightOn = isBedroomLightOn = isBathroomLightOn = isFacadeLightOn = isGardenLightOn = false;

            // Обновляем все индикаторы
            LivingRoomLightIndicator.Fill = Brushes.Red;
            KitchenLightIndicator.Fill = Brushes.Red;
            BedroomLightIndicator.Fill = Brushes.Red;
            BathroomLightIndicator.Fill = Brushes.Red;
            FacadeLightIndicator.Fill = Brushes.Red;
            GardenLightIndicator.Fill = Brushes.Red;

            LivingRoomLightIndicator.ToolTip = "Свет выключен";
            KitchenLightIndicator.ToolTip = "Свет выключен";
            BedroomLightIndicator.ToolTip = "Свет выключен";
            BathroomLightIndicator.ToolTip = "Свет выключен";
            FacadeLightIndicator.ToolTip = "Свет выключен";
            GardenLightIndicator.ToolTip = "Свет выключен";

            MessageBox.Show("🔌 Весь свет выключен", "Освещение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Климат-контроль
        private void TemperatureUp_Click(object sender, RoutedEventArgs e)
        {
            currentTemperature += 0.5;
            UpdateSensorDisplay();
        }

        private void TemperatureDown_Click(object sender, RoutedEventArgs e)
        {
            currentTemperature -= 0.5;
            UpdateSensorDisplay();
        }

        private void ToggleAirConditioner_Click(object sender, RoutedEventArgs e)
        {
            isAirConditionerOn = !isAirConditionerOn;
            AirConditionerIndicator.Fill = isAirConditionerOn ? Brushes.Green : Brushes.Red;
            AirConditionerIndicator.ToolTip = isAirConditionerOn ? "Кондиционер включен" : "Кондиционер выключен";
            AirConditionerButton.Content = isAirConditionerOn ? "Выключить" : "Включить";

            // Выключаем обогреватель при включении кондиционера
            if (isAirConditionerOn && isHeaterOn)
            {
                isHeaterOn = false;
                HeaterIndicator.Fill = Brushes.Red;
                HeaterIndicator.ToolTip = "Обогреватель выключен";
                HeaterButton.Content = "Включить";
            }
        }

        private void ToggleHeater_Click(object sender, RoutedEventArgs e)
        {
            isHeaterOn = !isHeaterOn;
            HeaterIndicator.Fill = isHeaterOn ? Brushes.Green : Brushes.Red;
            HeaterIndicator.ToolTip = isHeaterOn ? "Обогреватель включен" : "Обогреватель выключен";
            HeaterButton.Content = isHeaterOn ? "Выключить" : "Включить";

            // Выключаем кондиционер при включении обогревателя
            if (isHeaterOn && isAirConditionerOn)
            {
                isAirConditionerOn = false;
                AirConditionerIndicator.Fill = Brushes.Red;
                AirConditionerIndicator.ToolTip = "Кондиционер выключен";
                AirConditionerButton.Content = "Включить";
            }
        }

        private void ToggleHumidifier_Click(object sender, RoutedEventArgs e)
        {
            isHumidifierOn = !isHumidifierOn;
            HumidifierIndicator.Fill = isHumidifierOn ? Brushes.Green : Brushes.Red;
            HumidifierIndicator.ToolTip = isHumidifierOn ? "Увлажнитель включен" : "Увлажнитель выключен";
            HumidifierButton.Content = isHumidifierOn ? "Выключить" : "Включить";
        }

        // Безопасность
        private void ToggleAlarm_Click(object sender, RoutedEventArgs e)
        {
            isAlarmOn = !isAlarmOn;
            AlarmIndicator.Fill = isAlarmOn ? Brushes.Green : Brushes.Red;
            AlarmIndicator.ToolTip = isAlarmOn ? "Сигнализация включена" : "Сигнализация выключена";
            AlarmButton.Content = isAlarmOn ? "Выключить" : "Включить";
            AlarmButton.Background = isAlarmOn ? new SolidColorBrush(Color.FromRgb(76, 175, 80)) :
                                                 new SolidColorBrush(Color.FromRgb(244, 67, 54));

            // Добавляем запись в журнал
            AddSecurityLog(isAlarmOn ? "🟢 Сигнализация активирована" : "🔴 Сигнализация деактивирована");
        }

        private void ShowCameras_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📹 Открыт просмотр камер наблюдения", "Безопасность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckDoorsStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🚪 Все двери закрыты", "Безопасность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckWindowsStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🪟 Все окна закрыты", "Безопасность", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearSecurityLog_Click(object sender, RoutedEventArgs e)
        {
            SecurityLogPanel.Children.Clear();
            AddSecurityLog("📋 Журнал очищен");
        }

        private void AddSecurityLog(string message)
        {
            TextBlock logEntry = new TextBlock
            {
                Text = $"{DateTime.Now:HH:mm:ss} - {message}",
                Margin = new Thickness(0, 2, 0, 0),
                Foreground = message.Contains("🟢") ? Brushes.Green :
                            message.Contains("🔴") ? Brushes.Red : Brushes.Blue
            };
            SecurityLogPanel.Children.Add(logEntry);
        }

        // Мультимедиа
        private void PlayMusic_Click(object sender, RoutedEventArgs e)
        {
            isAudioOn = true;
            AudioIndicator.Fill = Brushes.Green;
            AudioIndicator.ToolTip = "Аудиосистема включена";
            NowPlayingText.Text = "Сейчас играет: Relaxing Music";
        }

        private void PauseMusic_Click(object sender, RoutedEventArgs e)
        {
            NowPlayingText.Text = "Воспроизведение приостановлено";
        }

        private void StopMusic_Click(object sender, RoutedEventArgs e)
        {
            isAudioOn = false;
            AudioIndicator.Fill = Brushes.Red;
            AudioIndicator.ToolTip = "Аудиосистема выключена";
            NowPlayingText.Text = "Не воспроизводится";
        }

        private void ToggleTV_Click(object sender, RoutedEventArgs e)
        {
            isTVOn = !isTVOn;
            TVIndicator.Fill = isTVOn ? Brushes.Green : Brushes.Red;
            TVIndicator.ToolTip = isTVOn ? "Телевизор включен" : "Телевизор выключен";
            TVButton.Content = isTVOn ? "Выключить" : "Включить";
            TVChannelText.Text = isTVOn ? "Канал: Новости" : "Канал: -";
        }

        private void ToggleProjector_Click(object sender, RoutedEventArgs e)
        {
            isProjectorOn = !isProjectorOn;
            ProjectorIndicator.Fill = isProjectorOn ? Brushes.Green : Brushes.Red;
            ProjectorIndicator.ToolTip = isProjectorOn ? "Проектор включен" : "Проектор выключен";
            ProjectorButton.Content = isProjectorOn ? "Выключить" : "Включить";
        }

        private void CinemaMode_Click(object sender, RoutedEventArgs e)
        {
            // Активация кино режима
            isTVOn = true;
            TVIndicator.Fill = Brushes.Green;
            TVButton.Content = "Выключить";
            TVChannelText.Text = "Канал: Кино";

            isAudioOn = true;
            AudioIndicator.Fill = Brushes.Green;
            NowPlayingText.Text = "Сейчас играет: Саундтрек фильма";

            // Приглушаем свет в гостиной
            isLivingRoomLightOn = false;
            LivingRoomLightIndicator.Fill = Brushes.Red;

            MessageBox.Show("🎬 Кино-режим активирован:\n• Включен телевизор\n• Включена аудиосистема\n• Приглушен свет",
                          "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MusicEvening_Click(object sender, RoutedEventArgs e)
        {
            isAudioOn = true;
            AudioIndicator.Fill = Brushes.Green;
            NowPlayingText.Text = "Сейчас играет: Вечерний плейлист";
            MessageBox.Show("🎵 Музыкальный вечер активирован", "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SportsMode_Click(object sender, RoutedEventArgs e)
        {
            isTVOn = true;
            TVIndicator.Fill = Brushes.Green;
            TVButton.Content = "Выключить";
            TVChannelText.Text = "Канал: Спорт";
            MessageBox.Show("📺 Спортивный режим активирован", "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AllMediaOff_Click(object sender, RoutedEventArgs e)
        {
            isTVOn = isAudioOn = isProjectorOn = false;
            TVIndicator.Fill = AudioIndicator.Fill = ProjectorIndicator.Fill = Brushes.Red;
            TVButton.Content = ProjectorButton.Content = "Включить";
            TVChannelText.Text = "Канал: -";
            NowPlayingText.Text = "Не воспроизводится";
            MessageBox.Show("🔇 Все медиаустройства выключены", "Мультимедиа", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void WorkScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💼 Рабочий сценарий активирован", "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnergySaveScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🔋 Сценарий экономии энергии активирован", "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VacationScenario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🏖️ Сценарий 'Отпуск' активирован", "Сценарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateScenario_Click(object sender, RoutedEventArgs e)
        {
            string scenarioName = NewScenarioName.Text;
            if (!string.IsNullOrWhiteSpace(scenarioName))
            {
                MessageBox.Show($"✅ Сценарий '{scenarioName}' создан!", "Создание сценария", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Owner = this; // Устанавливаем главное окно как владельца
            helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            helpWindow.ShowDialog(); // Показываем как модальное окно
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                sensorTimer?.Stop();
                Application.Current.Shutdown();
            }
        }

    }
}