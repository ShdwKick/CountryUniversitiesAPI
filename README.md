
# Требования
- .NET 9.0 или выше
- PostgreSQL база данных

### Установка и настройка

1. Склонируйте репозиторий:
   ```bash
   git clone https://github.com/ShdwKick/GoTogether.git
   ```
2. Перейдите в директорию проекта:
   ```bash
   cd GoTogether
   ```

   ```
4. Обновите строку подключения в файле `appsettings.json` или `appsettings.Development`, в зависимости от того в каком режиме будете запускать проект(Debug или Release), в папке Properties, указав вашу базу данных PostgreSQL. Также в этом файле можно указать число потоков, которое будет использовани при обработке данных.
5. Примените миграции, в папке с проектом(где лежит файл csproj) выполнить:
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
6. Запустите приложение.

### Использование

После запуска приложения endpoint будет доступен по адресу:

```
https://localhost:7192/swagger
http://localhost:5143/swagger
```

