// Подключаем необходимые библиотеки
using System; // Для работы с датами и временем
using System.Collections.Generic; // Для работы со списками
using System.Diagnostics;
using System.IO; // Для работы с файлами
using System.Linq; // Для работы с LINQ 
using System.Net; // Для работы с IP-адресами
using System.Net.Sockets; // Для работы с Modbus TCP
using System.Windows.Forms; // Для работы с формами Windows

namespace ПР103 // Имя пространства имен проекта
{
    public partial class PR103 : Form // Основной класс формы, наследуется от Form
    {
        // Константы для работы с Modbus
        private const ushort MAX_READ_REGISTERS = 200; // Максимальное количество регистров за один запрос
        private const int CONNECT_TIMEOUT = 2000; // Таймаут подключения в миллисекундах
        private const int IO_TIMEOUT = 2000; // Таймаут ввода-вывода в миллисекундах
        private const int MAX_CONNECT_ATTEMPTS = 5; // Максимальное количество попыток подключения

        // Переменные для хранения текущих параметров подключения
        private string deviceIP; // IP-адрес устройства
        private int port; // Порт устройства
        private byte slaveID; // Идентификатор устройства (Slave ID)
        private int interval; // Интервал опроса в секундах
        private List<ushort> registerList; // Список регистров для опроса
        private List<RegisterGroup> registerGroups; // Группы регистров для эффективного чтения
        private bool isPolling = false; // Флаг, указывающий, идет ли опрос
        private byte currentFunctionCode = 0x03; // По умолчанию Holding Registers
        private int maxPollCount = -1; // Максимальное количество опросов (-1 - бесконечно)
        private int currentPollCount = 0; // Текущее количество выполненных опросов
        private int failedConnectAttempts = 0; // Счетчик неудачных попыток подключения

        // Переменные для работы с лог-файлом
        private string logFilePath = ""; // Путь к файлу лога
        private StreamWriter logFileWriter; // Объект для записи в файл
        private bool logToFileEnabled = false; // Флаг, разрешена ли запись


        public PR103()
        {
            InitializeComponent(); // Инициализация компонентов формы (автоматически генерируется)
            InitializeDefaults(); // Установка значений по умолчанию
            cmbFunctionCode.SelectedIndex = 0; // Устанавливаем Holding Registers по умолчанию
            cmbDataFormat.SelectedIndex = 0; // Устанавливаем "DEC | HEX | BIN" по умолчанию
            // Устанавливаем имя файла лога по умолчанию: log_дата_IP.log
            txtLogFileName.Text = $"TCP_{txtDeviceIP.Text}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.log";

            // Обработчик для отложенной инициализации
            this.HandleCreated += (s, e) =>
            {
                txtLogFileName.Text = $"TCP_{txtDeviceIP.Text}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.log";
            };
        }

        // Метод для установки значений по умолчанию в полях ввода
        private void InitializeDefaults()
        {
            txtDeviceIP.Text = "10.2.11.122"; // IP по умолчанию
            txtPort.Text = "502"; // Порт Modbus по умолчанию
            txtSlaveID.Text = "1"; // Slave ID по умолчанию
            txtInterval.Text = "2"; // Интервал опроса по умолчанию (2 сек)
            txtRegisters.Text = "26-27(ip), 61563, 61553-61554(UTC)"; // Регистры по умолчанию
            txtPollCount.Text = ""; // По умолчанию - бесконечные опросы
        }

        // Обработчик нажатия кнопки "Старт"
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Парсим настройки, если не получилось - выходим
            if (!ParseSettings()) // Если парсинг настроек не удался
                return; // Выходим из метода

            // Сбрасываем счетчики при новом запуске
            currentPollCount = 0;
            failedConnectAttempts = 0;

            // Меняем состояние кнопок
            isPolling = true; // Устанавливаем флаг опроса
            btnStart.Enabled = false; // Делаем кнопку "Старт" неактивной
            btnStop.Enabled = true; // Активируем кнопку "Стоп"
            timer.Interval = interval * 1000; // Устанавливаем интервал таймера (переводим секунды в миллисекунды)
            timer.Start(); // Запускаем таймер

            LogMessage($"=== ОПРОС НАЧАТ ===");
            LogMessage($"Устройство: {deviceIP}:{port}, Slave ID: {slaveID}");
            LogMessage($"Функция: 0x{currentFunctionCode:X2}");
            LogMessage($"Интервал: {interval} сек");
            LogMessage($"Регистры: {string.Join(", ", registerGroups.Select(g => g.ToString()))}");
            if (maxPollCount > 0)
                LogMessage($"Количество опросов: {maxPollCount}");
            else
                LogMessage($"Количество опросов: бесконечно");
            LogMessage(new string('=', 50)); // Разделительная линия. 50 - Количество символов в линии
        }

        // Обработчик нажатия кнопки "Стоп"
        private void btnStop_Click(object sender, EventArgs e) // Обработчик события нажатия кнопки "Стоп"
        {
            StopPolling(); // Вызываем метод остановки опроса
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // Проверяем флаг опроса и создание дескриптора формы
            if (!isPolling || !this.IsHandleCreated || this.IsDisposed)
                return;

            // Проверяем ограничение на количество опросов
            if (maxPollCount > 0 && currentPollCount >= maxPollCount)
            {
                SafeLogMessage($"=== ДОСТИГНУТО МАКСИМАЛЬНОЕ КОЛИЧЕСТВО ОПРОСОВ ({maxPollCount}) ===");
                StopPolling();
                return;
            }

            try
            {
                // Записываем в лог время начала опроса
                SafeLogMessage($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ОПРОС {currentPollCount + 1} (Function 0x{currentFunctionCode:X2}):");

                // Получаем выбранный формат данных
                string selectedFormat = cmbDataFormat.SelectedItem?.ToString() ?? "DEC | HEX | BIN";

                // Проходим по всем группам регистров
                foreach (var group in registerGroups)
                {
                    try
                    {
                        ushort[] values = ReadRegisters(group.Start, group.Count);

                        // Обрабатываем данные в зависимости от выбранного формата
                        if (selectedFormat == "Float" || selectedFormat == "32-bit integer" ||
                            selectedFormat == "IP-адрес" || selectedFormat == "UTC время")
                        {
                            // Для этих форматов обрабатываем по 2 регистра за раз
                            for (int i = 0; i < values.Length; i += 2)
                            {
                                if (i + 1 >= values.Length) break;

                                ushort address = (ushort)(group.Start + i);
                                string formattedValue = FormatValue(values, i, selectedFormat);

                                SafeUpdateDataGrid(address, values[i], formattedValue);
                                SafeLogMessage($"Регистры {address}-{address + 1} (0x{address:X4}-0x{address + 1:X4}): {formattedValue}");
                            }
                        }
                        else
                        {
                            // Стандартная обработка для одиночных регистров
                            for (int i = 0; i < values.Length; i++)
                            {
                                ushort address = (ushort)(group.Start + i);
                                string formattedValue = FormatValue(values, i, selectedFormat);

                                SafeUpdateDataGrid(address, values[i], formattedValue);
                                SafeLogMessage($"Регистр {address} (0x{address:X4}): {formattedValue}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SafeLogMessage($"ОШИБКА при чтении группы {group}: {GetExceptionMessage(ex)}");
                    }
                }

                // Увеличиваем счетчик успешных опросов
                currentPollCount++;
                // Сбрасываем счетчик неудачных подключений после успешного опроса
                failedConnectAttempts = 0;
            }
            catch (Exception ex)
            {
                SafeLogMessage($"ОШИБКА: {GetExceptionMessage(ex)}");

                // Если это ошибка подключения, увеличиваем счетчик неудачных попыток
                if (ex is SocketException || ex is IOException || ex is TimeoutException)
                {
                    failedConnectAttempts++;
                    SafeLogMessage($"Неудачная попытка подключения #{failedConnectAttempts}");

                    // Если достигли максимума неудачных попыток, останавливаем опрос
                    if (failedConnectAttempts >= MAX_CONNECT_ATTEMPTS)
                    {
                        SafeLogMessage($"=== ПРЕВЫШЕНО МАКСИМАЛЬНОЕ КОЛИЧЕСТВО НЕУДАЧНЫХ ПОПЫТОК ({MAX_CONNECT_ATTEMPTS}) ===");
                        StopPolling();
                    }
                }
            }
        }

        // Вспомогательные безопасные методы для работы с UI
        private void SafeLogMessage(string message)
        {
            if (rtbLog.IsDisposed || !rtbLog.IsHandleCreated)
            {
                Debug.WriteLine("Лог недоступен: " + message);
                return;
            }

            if (rtbLog.InvokeRequired)
            {
                try
                {
                    rtbLog.BeginInvoke(new Action<string>(SafeLogMessage), message);
                }
                catch (ObjectDisposedException)
                {
                    Debug.WriteLine("Лог был удален: " + message);
                }
                return;
            }

            try
            {
                rtbLog.AppendText(message + "\n");
                rtbLog.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка записи в лог: " + ex.Message);
            }
        }

        private void SafeUpdateDataGrid(ushort address, ushort value, string formattedValue)
        {
            if (dgvResults.IsDisposed || !dgvResults.IsHandleCreated)
                return;

            if (dgvResults.InvokeRequired)
            {
                try
                {
                    dgvResults.BeginInvoke(new Action<ushort, ushort, string>(SafeUpdateDataGrid),
                        address, value, formattedValue);
                }
                catch (ObjectDisposedException)
                {
                    Debug.WriteLine("Таблица была удалена");
                }
                return;
            }

            try
            {
                // Ищем строку с таким адресом
                DataGridViewRow row = dgvResults.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells[0].Value != null &&
                                       r.Cells[0].Value.ToString() == address.ToString());

                if (row == null)
                {
                    // Если строка не найдена, добавляем новую
                    row = new DataGridViewRow();
                    row.CreateCells(dgvResults);
                    row.Cells[0].Value = address.ToString(); // Адрес
                    dgvResults.Rows.Add(row);
                }

                // Обновляем значения
                row.Cells[1].Value = value.ToString(); // DEC
                row.Cells[2].Value = $"0x{value:X4}"; // HEX
                row.Cells[3].Value = Convert.ToString(value, 2).PadLeft(16, '0'); // BIN
                row.Cells[4].Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"); // Время UTC
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка обновления таблицы: " + ex.Message);
            }
        }

        // Метод для форматирования значения в соответствии с выбранным форматом
        private string FormatValue(ushort[] values, int index, string format)
        {
            try
            {
                switch (format)
                {
                    case "DEC | HEX | BIN":
                        return $"{values[index]} | 0x{values[index]:X4}, | {Convert.ToString(values[index], 2).PadLeft(16, '0')}";

                    case "16-bit integer":
                        return values[index].ToString();

                    case "32-bit integer":
                        if (index + 1 >= values.Length) return "Недостаточно данных";
                        uint uintValue = (uint)((values[index] << 16) | values[index + 1]);
                        return uintValue.ToString();

                    case "Float":
                        if (index + 1 >= values.Length) return "Недостаточно данных";
                        byte[] floatBytes = new byte[4];
                        floatBytes[3] = (byte)(values[index + 1] >> 8);   // C  
                        floatBytes[2] = (byte)(values[index + 1] & 0xFF); // D   
                        floatBytes[1] = (byte)(values[index] >> 8);       // A
                        floatBytes[0] = (byte)(values[index] & 0xFF);     // B
                        float floatValue = BitConverter.ToSingle(floatBytes, 0);
                        return floatValue.ToString("F6");

                    case "IP-адрес":
                        if (index + 1 >= values.Length) return "Недостаточно данных";
                        byte[] ipBytes = new byte[4];
                        ipBytes[2] = (byte)(values[index] >> 8);       // A
                        ipBytes[3] = (byte)(values[index] & 0xFF);     // B
                        ipBytes[0] = (byte)(values[index + 1] >> 8);   // C
                        ipBytes[1] = (byte)(values[index + 1] & 0xFF); // D
                        return new IPAddress(ipBytes).ToString();

                    case "UTC время":
                        if (index + 1 >= values.Length) return "Недостаточно данных";
                        uint secondsSince2000 = (uint)((values[index + 1] << 16) | values[index]);
                        DateTime utcTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            .AddSeconds(secondsSince2000);
                        return utcTime.ToString("dd-MM-yyyy HH:mm:ss");

                    default:
                        return values[index].ToString();
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка формата: {ex.Message}";
            }
        }

        private void btnApplyFunction_Click(object sender, EventArgs e)
        {
            switch (cmbFunctionCode.SelectedIndex)
            {
                case 0: currentFunctionCode = 0x03; break; // Holding Registers
                case 1: currentFunctionCode = 0x04; break; // Input Registers
                case 2: currentFunctionCode = 0x01; break; // Coils
                case 3: currentFunctionCode = 0x02; break; // Discrete Inputs
                default: currentFunctionCode = 0x03; break;
            }

            LogMessage($"Функциональный код изменен на: 0x{currentFunctionCode:X2}");
        }

        // Метод для чтения регистров по Modbus TCP
        private ushort[] ReadRegisters(ushort startAddress, ushort count)
        {
            // Проверяем, не превышено ли максимальное количество регистров
            if (count > MAX_READ_REGISTERS) // Если количество регистров больше максимального
                throw new Exception($"Превышено максимальное количество регистров для чтения: {MAX_READ_REGISTERS}"); // Прерываем выполнение

            // Создаем TCP-клиент
            using (TcpClient client = new TcpClient()) // Используем блок using для автоматического закрытия клиента
            {
                // Устанавливаем таймауты
                client.ReceiveTimeout = IO_TIMEOUT; // Таймаут получения данных
                client.SendTimeout = IO_TIMEOUT; // Таймаут отправки данных

                // Подключаемся к устройству с таймаутом
                var connectTask = client.ConnectAsync(IPAddress.Parse(deviceIP), port); // Подключаемся асинхронно
                if (!connectTask.Wait(CONNECT_TIMEOUT)) // Ждем подключения с таймаутом
                    throw new TimeoutException($"Таймаут подключения ({CONNECT_TIMEOUT} мс)"); // Если таймаут истек

                if (!client.Connected) // Если клиент не подключен
                    throw new IOException("Соединение не установлено"); // Прерываем выполнение

                // Работаем с сетевым потоком
                using (NetworkStream stream = client.GetStream())
                {
                    // Формируем Modbus-запрос
                    byte[] request = new byte[12]; // Буфер для запроса (12 байт)
                    request[0] = 0x00; // Transaction ID (high byte) - Идентификатор транзакции (Значение старшего разряда регистра)
                    request[1] = 0x01; // Transaction ID (low byte) - Идентификатор транзакции (Значение младшего разряда регистра)
                    request[2] = 0x00; // Protocol ID (high byte) - Идентификатор протокола (Значение старшего разряда регистра)
                    request[3] = 0x00; // Protocol ID (low byte) - Идентификатор протокола (Значение младшего разряда регистра)
                    request[4] = 0x00; // Message Length (high byte) - Длина (Значение старшего разряда регистра)
                    request[5] = 0x06; // Message Length (low byte) - Длина (Значение младшего разряда регистра)
                    request[6] = slaveID; // Unit ID - Адрес устройства
                    request[7] = currentFunctionCode;  // Function Code - команда (функция) чтения
                    request[8] = (byte)(startAddress >> 8); // (high byte) - Адрес первого регистра
                    request[9] = (byte)(startAddress & 0xFF); // (low byte) - Адрес первого регистра
                    request[10] = (byte)(count >> 8); // (high byte) - Количество требуемых регистров
                    request[11] = (byte)(count & 0xFF); // (low byte) - Количество требуемых регистров

                    // Отправляем запрос на устройство через сетевой поток
                    stream.Write(request, 0, request.Length);

                    // Объявляем переменную для хранения ожидаемого размера ответа от устройства
                    int expectedResponseSize;

                    // Проверяем тип функционального кода (0x01 - Coils, 0x02 - Discrete Inputs)
                    if (currentFunctionCode == 0x01 || currentFunctionCode == 0x02)
                    {
                        // Для битовых операций (Coils/Discrete Inputs) рассчитываем размер ответа:
                        expectedResponseSize = 9 + (count + 7) / 8;
                    }
                    else
                    {
                        // Для 16-битных регистров (Holding/Input) рассчитываем размер ответа:
                        expectedResponseSize = 9 + count * 2;
                    }

                    // Создаем буфер для приема ответа от устройства с рассчитанным размером
                    byte[] response = new byte[expectedResponseSize];

                    // Читаем данные из потока
                    int bytesRead = stream.Read(response, 0, expectedResponseSize);

                    // Проверяем, что получено как минимум 9 байт (минимальный размер ответа Modbus)
                    if (bytesRead < 9)
                        throw new Exception($"Неполный ответ ({bytesRead} байт)");

                    // Проверяем бит ошибки в ответе (7-й бит 7-го байта установлен в 1)
                    if ((response[7] & 0x80) == 0x80)
                    {
                        // Получаем код ошибки из 8-го байта ответа
                        byte errorCode = response[8];

                        // Создаем переменную для сообщения об ошибке
                        string errorMsg;

                        // Определяем текст сообщения в зависимости от кода ошибки
                        switch (errorCode)
                        {
                            case 0x01: errorMsg = "Недопустимая функция"; break;
                            case 0x02: errorMsg = "Недопустимый адрес"; break;
                            case 0x03: errorMsg = "Недопустимое значение"; break;
                            case 0x04: errorMsg = "Ошибка устройства"; break;
                            default: errorMsg = $"Неизвестная ошибка (код 0x{errorCode:X2})"; break;
                        }

                        // Генерируем исключение с описанием ошибки
                        throw new Exception($"Ошибка устройства: {errorMsg}");
                    }

                    // Обработка ответа для битовых операций (Coils/Discrete Inputs)
                    if (currentFunctionCode == 0x01 || currentFunctionCode == 0x02)
                    {
                        // Получаем количество байт данных из ответа (8-й байт)
                        byte receivedByteCount = response[8];

                        // Создаем массив для хранения значений (каждое значение - 0 или 1)
                        ushort[] values = new ushort[count];

                        // Обрабатываем каждый запрошенный бит
                        for (int i = 0; i < count; i++)
                        {
                            int bytePos = 9 + i / 8;
                            int bitPos = i % 8;
                            values[i] = (ushort)((response[bytePos] & (1 << bitPos)) != 0 ? 1 : 0);
                        }

                        return values;
                    }
                    else
                    {
                        // Обработка ответа для 16-битных регистров (Holding/Input)
                        byte receivedByteCount = response[8];

                        // Проверяем, что количество полученных байт соответствует ожидаемому
                        if (receivedByteCount != count * 2)
                            throw new Exception($"Несоответствие количества данных. Ожидалось {count * 2}, получено {receivedByteCount}");

                        // Создаем массив для хранения 16-битных значений
                        ushort[] values = new ushort[count];

                        // Обрабатываем каждый 16-битный регистр
                        for (int i = 0; i < count; i++)
                        {
                            int offset = 9 + i * 2;
                            values[i] = (ushort)((response[offset] << 8) | response[offset + 1]);
                        }

                        return values;
                    }
                }
            }
        }

        // Метод для парсинга настроек из полей ввода
        private bool ParseSettings()
        {
            // Парсим IP-адрес 
            if (!IPAddress.TryParse(txtDeviceIP.Text, out IPAddress ip)) // Проверяем, что IP-адрес корректный
            {
                MessageBox.Show("Неверный IP адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;  // Если IP-адрес некорректный, показываем сообщение об ошибке
            }
            deviceIP = txtDeviceIP.Text; // Сохраняем IP-адрес в переменную

            // Парсим порт (должен быть числом от 1 до 65535)
            if (!int.TryParse(txtPort.Text, out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Порт должен быть числом от 1 до 65535", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Парсим Slave ID (должен быть числом от 1 до 255)
            if (!byte.TryParse(txtSlaveID.Text, out slaveID) || slaveID == 0)
            {
                MessageBox.Show("Slave ID должен быть числом от 1 до 255", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Парсим интервал опроса (должен быть положительным числом)
            if (!int.TryParse(txtInterval.Text, out interval) || interval <= 0)
            {
                MessageBox.Show("Интервал должен быть положительным числом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Парсим количество опросов (пустое поле - бесконечно)
            if (!string.IsNullOrWhiteSpace(txtPollCount.Text))
            {
                if (!int.TryParse(txtPollCount.Text, out maxPollCount) || maxPollCount <= 0)
                {
                    MessageBox.Show("Количество опросов должно быть положительным числом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                maxPollCount = -1; // Бесконечные опросы
            }

            // Парсим список регистров
            var (parsedInterval, parsedRegisters) = ParseInput(txtRegisters.Text);
            if (parsedRegisters.Count == 0)
            {
                MessageBox.Show("Не указаны регистры для чтения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Если список регистров пуст, показываем сообщение об ошибке
            }

            // Проверяем, что все адреса в допустимом диапазоне (1-65535)
            if (parsedRegisters.Any(r => r < 1 || r > 65535))
            {
                MessageBox.Show("Адреса регистров должны быть в диапазоне от 1 до 65535", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Сохраняем список регистров и группируем их
            registerList = parsedRegisters;

            // Если выбран формат, требующий 2 регистра, убедимся что количество регистров четное
            if (cmbDataFormat.SelectedItem != null &&
                (cmbDataFormat.SelectedItem.ToString() == "Float" ||
                 cmbDataFormat.SelectedItem.ToString() == "32-bit integer" ||
                 cmbDataFormat.SelectedItem.ToString() == "IP-адрес" ||
                 cmbDataFormat.SelectedItem.ToString() == "UTC время"))
            {
                if (registerList.Count % 2 != 0)
                {
                    MessageBox.Show("Для выбранного формата данных необходимо указать четное количество регистров",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            registerGroups = GroupRegisters(registerList);
            return true;
        }


        // Метод остановки опроса
        private void StopPolling()
        {
            isPolling = false; // Сбрасываем флаг опроса
            timer.Stop(); // Останавливаем таймер

            // Меняем состояние кнопок
            btnStart.Enabled = true; // Делаем кнопку "Старт" активной
            btnStop.Enabled = false; // Делаем кнопку "Стоп" неактивной

            // Записываем в лог информацию об остановке
            LogMessage("=== ОПРОС ОСТАНОВЛЕН ===");
            LogMessage($"Всего выполнено опросов: {currentPollCount}");

            // Закрываем файл лога, если он открыт
            if (logFileWriter != null)
            {
                logFileWriter.Close();
                logFileWriter = null;
            }
            logToFileEnabled = false;
        }

        // Обработчик нажатия кнопки "Очистить"
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear(); // Очищаем лог на форме
            dgvResults.Rows.Clear(); // Очищаем таблицу результатов
        }

        // Метод для записи сообщений в лог
        // Метод для записи сообщений в лог
        private void LogMessage(string message)
        {
            // Проверяем, создан ли дескриптор окна
            if (!rtbLog.IsHandleCreated)
            {
                // Если дескриптор еще не создан, просто записываем в консоль (для отладки)
                Console.WriteLine("Лог не готов: " + message);
                return;
            }

            // Если метод вызывается не из основного потока, перенаправляем вызов в основной поток
            if (rtbLog.InvokeRequired)
            {
                try
                {
                    rtbLog.BeginInvoke(new Action<string>(LogMessage), message);
                }
                catch (InvalidOperationException)
                {
                    // Если Invoke невозможно, записываем в консоль
                    Console.WriteLine("Ошибка Invoke: " + message);
                }
                return;
            }

            // Основной поток - безопасные операции с контролом
            try
            {
                string logLine = $"{message}";
                rtbLog.AppendText(logLine + "\n");
                rtbLog.ScrollToCaret();

                if (logToFileEnabled && logFileWriter != null)
                {
                    try
                    {
                        logFileWriter.WriteLine(logLine);
                    }
                    catch (Exception ex)
                    {
                        logToFileEnabled = false;
                        rtbLog.AppendText($"Ошибка записи в лог-файл: {ex.Message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи в лог: " + ex.Message);
            }
        }

        // Обработчик нажатия кнопки "Указать путь лога"
        private void btnLogSettings_Click(object sender, EventArgs e)
        {

            // Устанавливаем имя файла по умолчанию: log_IP_дата_время.log
            saveFileDialog.FileName = $"TCP_{txtDeviceIP.Text}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.log";

            // Показываем диалог выбора файла
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                logFilePath = saveFileDialog.FileName; // Сохраняем выбранный путь
                txtLogFileName.Text = logFilePath; // Показываем путь в текстовом поле
                lblLogPath.Text = "Лог активен"; // Меняем статус лога

                try
                {
                    logFileWriter?.Close();  // Закрываем предыдущий файл, если он был открыт
                    logFileWriter = new StreamWriter(logFilePath, true) // Создаем новый файл или открываем для дозаписи
                    {
                        AutoFlush = true // Автоматическая запись без буферизации
                    };
                    logToFileEnabled = true; // Разрешаем запись в файл
                    // Записываем в лог информацию о начале записи в файл
                    LogMessage($"=== Логирование в файл начато ===");
                    LogMessage($"Файл: {logFilePath}");
                }
                catch (Exception ex)
                {
                    logToFileEnabled = false; // Запрещаем запись в файл при ошибке
                    MessageBox.Show($"Ошибка при открытии файла лога: {ex.Message}",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Метод для парсинга строки с регистрами при вводе
        private (int interval, List<ushort> registers) ParseInput(string input)
        {
            // Разбиваем строку по запятым, удаляем пробелы и пустые элементы
            var parts = input.Split(',') // Разделяем строку по запятым
                .Select(x => x.Trim()) // Удаляем пробелы в начале и конце каждого элемента
                .Where(x => !string.IsNullOrEmpty(x)) // Оставляем только непустые элементы
                .ToList(); // Преобразуем в список строк

            // Если нет регистров - возвращаем пустой список
            if (parts.Count == 0)
                return (0, new List<ushort>()); // Возвращаем пустой список регистров

            var registers = new List<ushort>(); // Список для хранения уникальных регистров

            // Обрабатываем каждый элемент
            foreach (var part in parts)
            {
                // Если элемент содержит диапазон (через дефис)
                if (part.Contains('-')) // Проверяем, содержит ли элемент дефис
                {
                    var rangeParts = part.Split('-'); // Разделяем диапазон по дефису
                    if (rangeParts.Length == 2 && // Проверяем, что диапазон состоит из двух частей
                        ushort.TryParse(rangeParts[0], out ushort start) && // Преобразуем первую часть в ushort
                        ushort.TryParse(rangeParts[1], out ushort end) && // Преобразуем вторую часть в ushort
                        start <= end) // Проверяем, что начало меньше или равно концу
                    {
                        // Добавляем все регистры из диапазона
                        for (ushort i = start; i <= end; i++) // Проходим по всем значениям в диапазоне
                            registers.Add(i); // Добавляем регистр в список
                    }
                }
                // Если элемент - одиночный регистр
                else if (ushort.TryParse(part, out ushort reg))
                {
                    registers.Add(reg);
                }
            }

            // Возвращаем интервал и список уникальных отсортированных регистров
            return (interval, registers.Distinct().OrderBy(x => x).ToList());
        }

        // Метод для группировки регистров (оптимизация запросов)
        private List<RegisterGroup> GroupRegisters(List<ushort> registers)
        {
            var groups = new List<RegisterGroup>();
            if (registers.Count == 0) return groups;

            registers.Sort();
            ushort start = registers[0];
            ushort count = 1;

            for (int i = 1; i < registers.Count; i++)
            {
                // Для 32-битных значений убедимся, что следующее значение - это следующий регистр
                if (registers[i] == registers[i - 1] + 1)
                {
                    count++;
                }
                else
                {
                    // Если текущий формат требует 2 регистра, убедимся что count четный
                    if (count % 2 != 0 && (cmbDataFormat.SelectedItem.ToString() == "Float" ||
                                           cmbDataFormat.SelectedItem.ToString() == "32-bit integer" ||
                                           cmbDataFormat.SelectedItem.ToString() == "IP-адрес" ||
                                           cmbDataFormat.SelectedItem.ToString() == "UTC время"))
                    {
                        count--; // Исключаем последний регистр из группы
                        i--; // Вернемся к этому регистру на следующей итерации
                    }

                    groups.Add(new RegisterGroup(start, count));
                    start = registers[i];
                    count = 1;
                }
            }

            // Добавляем последнюю группу
            groups.Add(new RegisterGroup(start, count));
            return groups;
        }

        // Метод для получения сообщения об ошибке
        private string GetExceptionMessage(Exception ex)
        {
            // Если это ошибка сокета - возвращаем специальное сообщение
            if (ex is SocketException socketEx)
                return $"Сетевая ошибка ({socketEx.SocketErrorCode}): {socketEx.Message}";

            // Если это агрегированное исключение - берем внутреннее исключение
            var aggEx = ex as AggregateException;
            if (aggEx != null && aggEx.InnerException != null)
                return GetExceptionMessage(aggEx.InnerException);

            // Для всех остальных - стандартное сообщение
            return $"{ex.GetType().Name}: {ex.Message}";
        }

        // Метод вызывается при закрытии формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Закрываем файл лога, если он открыт
            if (logFileWriter != null)
            {
                logFileWriter.Close();
                logFileWriter = null;
            }
        }

        // Структура для хранения группы регистров
        private readonly struct RegisterGroup
        {
            public ushort Start { get; } // Начальный адрес группы
            public ushort Count { get; } // Количество регистров в группе
            public ushort End => (ushort)(Start + Count - 1); // Конечный адрес группы

            // Конструктор
            public RegisterGroup(ushort start, ushort count)
            {
                Start = start;
                Count = count;
            }

            // Преобразование в строку
            public override string ToString() =>
                Count == 1 ? $"0x{Start:X4}" : $"0x{Start:X4}-0x{End:X4}";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}