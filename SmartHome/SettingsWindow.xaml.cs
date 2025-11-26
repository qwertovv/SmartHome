using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartHome
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
            FontSizeSlider.ValueChanged += FontSizeSlider_ValueChanged;
        }

        private void LoadSettings()
        {
            // Загрузка сохраненных настроек (здесь можно добавить загрузку из файла/базы данных)
            // Пока просто устанавливаем значения по умолчанию
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FontSizeValue.Text = $"{FontSizeSlider.Value:0} pt";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение настроек
            try
            {
                // Получаем значения из элементов управления
                string theme = (ThemeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                double fontSize = FontSizeSlider.Value;
                bool animationsEnabled = AnimationsCheckBox.IsChecked ?? false;

                bool emailNotifications = EmailNotificationsCheckBox.IsChecked ?? false;
                bool pushNotifications = PushNotificationsCheckBox.IsChecked ?? false;
                bool soundNotifications = SoundNotificationsCheckBox.IsChecked ?? false;

                bool securityNotifications = SecurityNotificationsCheckBox.IsChecked ?? false;
                bool climateNotifications = ClimateNotificationsCheckBox.IsChecked ?? false;
                bool energyNotifications = EnergyNotificationsCheckBox.IsChecked ?? false;

                int dayTemperature = int.Parse(DayTemperatureTextBox.Text);
                int nightTemperature = int.Parse(NightTemperatureTextBox.Text);

                string lightsOnTime = LightsOnTimeTextBox.Text;
                string lightsOffTime = LightsOffTimeTextBox.Text;

                bool autoAwayMode = AutoAwayModeCheckBox.IsChecked ?? false;

                string updateInterval = (UpdateIntervalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                string language = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                bool autoUpdate = AutoUpdateCheckBox.IsChecked ?? false;
                bool dataCollection = DataCollectionCheckBox.IsChecked ?? false;

                // Здесь можно сохранить настройки в файл, базу данных или Properties.Settings
                // Например: Properties.Settings.Default.Theme = theme;

                MessageBox.Show("Настройки успешно сохранены!", "Сохранение",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите сбросить все настройки к значениям по умолчанию?",
                                       "Сброс настроек",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Сброс к значениям по умолчанию
                ThemeComboBox.SelectedIndex = 0;
                FontSizeSlider.Value = 12;
                AnimationsCheckBox.IsChecked = true;

                EmailNotificationsCheckBox.IsChecked = true;
                PushNotificationsCheckBox.IsChecked = true;
                SoundNotificationsCheckBox.IsChecked = true;

                SecurityNotificationsCheckBox.IsChecked = true;
                ClimateNotificationsCheckBox.IsChecked = true;
                EnergyNotificationsCheckBox.IsChecked = false;

                DayTemperatureTextBox.Text = "22";
                NightTemperatureTextBox.Text = "18";

                LightsOnTimeTextBox.Text = "18:00";
                LightsOffTimeTextBox.Text = "23:00";

                AutoAwayModeCheckBox.IsChecked = false;

                UpdateIntervalComboBox.SelectedIndex = 1;
                LanguageComboBox.SelectedIndex = 0;

                AutoUpdateCheckBox.IsChecked = true;
                DataCollectionCheckBox.IsChecked = false;

                MessageBox.Show("Настройки сброшены к значениям по умолчанию", "Сброс",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Валидация ввода температуры
        private void TemperatureTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Разрешаем только цифры
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        // Валидация ввода времени
        private void TimeTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;

            // Проверяем формат времени (HH:MM)
            if (newText.Length > 5)
            {
                e.Handled = true;
                return;
            }

            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c) && c != ':')
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}