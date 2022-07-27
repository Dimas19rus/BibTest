Задача:
Автоматизированное рабочее место (АРМ) сотрудника библиотеки.
В данный момент есть действующая библиотека, в ней ведется учёт в
виде справочников книг, а также существует список читателей.
Необходимо реализовать выдачу и возврат книг. Просмотр книги
возможен «на руках» читателя и в библиотеке.
* В плане оформления (реализовать все стандартными средствами).


Реализованно:
- Авторизация;
- Регистрация;
- Ограничение на основе ролей;
- Получение список книг:
  - Поиск по названию книги;
  - Получение книг определенного автора по нажатию ФИО автора;
- Управление запросами на получение книг;
- Управление списком аренды книг;
- Система уведомлений о результате изменений в бд

Не сделанно:
- Добавление и изменение книг(пока только в AppDbContext);
- Нету репозитория(а надо было, а то все обращение к DbContex реализованный в Контроллерах)
