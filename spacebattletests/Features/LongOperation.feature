﻿#language: ru

Функция: LongOperation

Сценарий: Позитивный тест долгой операции

Дано Инициализирован IoC контейнер с необходимыми зависимостями.
Когда Выполняется команда Game.Operation.Movement.
То Макро Команда успешно завершает выполнение.

Сценарий: Позитивный тест долгой операции с обычной командой

Дано Инициализирован IoC контейнер с обычной командой.
Когда Выполняется команда Game.Operation.Movement.
То Команда успешно завершает выполнение.

Сценарий: Негативный тест долгой операции

Дано Инициализирован IoC контейнер без активации команды.
Когда Выполняется команда Game.Operation.Movement.
То Команда не вызывается и не выполняется.