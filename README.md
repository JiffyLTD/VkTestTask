# VkTestTask
Test task for VK 

Не совсем понял задание : 
Добавление нового пользователя должно занимать 5 сек.
За это время при попытке добавления пользователя с таким же login должна возвращаться ошибка.

Логин уникальный или же данное задание направлено как защита от спама регистрации аккаунтов с одинаковыми логинами.
Я решил реализовать второй вариант, поэтому в течении 5 секунд после регистрации пользователя, пользователь с таким же логином не сможет производить регистрацию,
но после истечении этих 5 секунд, использовать такой же логин можно.