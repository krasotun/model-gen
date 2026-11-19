# ModelGen

Консольная программа, которая читает `swagger.json` и генерирует C#-код моделей: по одному `record` на каждую схему из раздела `definitions`.

> Статус: проект в разработке. Готово преобразование имён полей (`first_name` → `FirstName`). `ModelGenerator.Generate` читает `definitions` через `JsonDocument` и строит `record` для одной схемы. Типы полей: `string` → `string`, `integer` → `int`, `number` → `double` (`number` с `format: integer` → `int`), массив скаляров одного уровня → `T[]` (`format` элемента учитывается). Поле без `type` (`$ref`) и массив в массиве → `NotSupportedException`. `required` пока не читается. Ограничение: при нескольких схемах в `definitions` вывод некорректен — поддержка в следующем шаге. Консольная программа к генератору ещё не подключена, `make run` печатает заглушку.

## Что делает

На входе — схема из `swagger.json`:

```json
"UserResponse": {
  "required": ["id", "first_name", "login"],
  "properties": {
    "id":         { "type": "integer" },
    "first_name": { "type": "string" },
    "login":      { "type": "string" }
  }
}
```

На выходе — объявление модели:

```csharp
public record UserResponse(int Id, string FirstName, string Login);
```

Так для каждой схемы из `definitions`. Задуманный запуск: `model-gen path/to/swagger.json`, результат — файл `Models.cs`.

## Условие

- **Вход и выход.** На входе — JSON-файл в формате Swagger 2.0 с диска, путь передаётся параметром командной строки. На выходе — один файл `Models.cs` со всеми моделями.
- **Типы.** Генератор переводит типы строго по спецификации и не угадывает по `description`:

  | Swagger                          | C#         |
  | -------------------------------- | ---------- |
  | `string`                         | `string`   |
  | `integer`                        | `int`      |
  | `number` + `"format": "integer"` | `int`      |
  | `number` без `format`            | `double`   |
  | `array` из `integer`             | `int[]`    |
  | `array` из `string`              | `string[]` |

  Открыто: `object`, `$ref`, `string` с `format: date-time`, массивы `$ref`.

  Известная кривизна входа:
  - `id` в `SignUpResponse` и `CreateChatResponse` описаны как `number` без `format`, поэтому станут `double`.
  - `user` в `last_message` у `ChatsResponse` - имеет ошибочный format date-time. У `$ref` есть соседи. Генератор будет игнорировать соседей. `user` будет обработан, когда появится поведение с `$ref`.

- **Обязательность.** Открыто: как отличить в `record` поле из `required` от необязательного.
- **Кривой вход.**
  - Имя поля, которое не переводится в PascalCase (например, `first__Name`): вся модель пропускается, в консоль выводится ворнинг, остальные модели генерируются. Открыто: если на пропущенную модель ссылается `$ref`, `Models.cs` не соберётся.
  - Нет раздела `definitions`: ошибка с сообщением, ненулевой код выхода, `Models.cs` не создаётся.

## Как устроен

1. Прочитать JSON и достать из него схемы.
2. Превратить тип поля в тип C#: по таблице из раздела «Условие».
3. Превратить имя поля: `first_name` → `FirstName` (готово, `FieldNameMapper`).
4. Собрать текст `record` и вывести.

## Запуск

Нужен .NET SDK 10.0.401 или более новый патч той же линейки (версия закреплена в `global.json`) и `make`.

```sh
git clone https://github.com/krasotun/model-gen.git
cd model-gen

# восстановить пакеты и локальные инструменты (CSharpier)
make install

# запустить программу
make run
```

## Команды

| Команда             | Что делает                                                                  |
| ------------------- | --------------------------------------------------------------------------- |
| `make install`      | Восстанавливает NuGet-пакеты и локальные инструменты из `dotnet-tools.json` |
| `make build`        | Собирает решение; предупреждения анализаторов считаются ошибками            |
| `make test`         | Запускает тесты xUnit                                                       |
| `make test-watch`   | Перезапускает тесты при сохранении файла                                    |
| `make format`       | Форматирует код через CSharpier                                             |
| `make format-check` | Проверяет форматирование, ничего не меняя                                   |
| `make check`        | Полная проверка перед коммитом: `format-check`, `build`, `test`             |
| `make run`          | Запускает программу                                                         |

Один тест по имени: `dotnet test --filter <часть имени>`.

## Структура

```
src/ModelGen/              код генератора
tests/ModelGen.Tests/      тесты xUnit
Directory.Build.props      общие настройки сборки и анализаторов
Makefile                   команды разработки
```
