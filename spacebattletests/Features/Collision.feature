﻿#language: ru

Функция: Collision

Сценарий: Произошла коллизия
	Дано команда просчитать коллизию
	Когда выполняется команда проверки коллизий
	Тогда команда-обработчик коллизий должна быть вызвана

Сценарий: Прочитать дерево решений невозможно
	Дано коллизии нет в дереве решений
	Когда выполняется команда проверки коллизий с ошибкой
	Тогда команда-обработчик коллизий не вызывается