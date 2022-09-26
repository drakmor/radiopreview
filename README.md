Обеспечивает автоматическое включение External Preview на консолях Axia QOR при использовании ПО Synadyn Radio 2

Данный костыль реализует преобразование команд GPO N xxxLxx / GPO N xxHxx в GPI N xxLxx / GPI N xxHxx.
Это необходимо для включения External Preview на 13 ноге у Contol Room Monitor

Для работы необходимо:
- в настройка "Show" Axia QOR настроить свободный канал под GPIO
- включить "Preview" в опциях "CR Monitor", назначить в "External Preview" канал, используемый для подслушки.
- назначить GPIO канал управления на любой из выходов драйвера LiveWire на выпускающем ПК
- В Synadyn Radio 2 в файле **config\lawo.conf** указать:
```
[PlayPrevNotify]
HostAdress=127.0.0.1
Pin=4
```
где Pin - номер выхода драйвера LiveWire, на который назначен GPIO канал LiveWire

В файле **config\ultra.ini**
```
[RemoteControl]
ComControlType=2
```

Запуск:
**radiopreview.exe host_address gpio_port**
- где **host_address** - IP адрес виртуального драйвера, 127.0.0.1 - если запускается на том же ПК, где и Synadyn Radio 2
- **gpio_port** - номер выхода виртуального драйвера, на который назначен GPIO адрес управления CR Monitor
