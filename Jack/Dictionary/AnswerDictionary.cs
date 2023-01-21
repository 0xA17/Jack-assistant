using System;

namespace Jack
{
    public class AnswerDictionary
    {
        public static String[] HelloAnswer = { "привет", "доброго времени суток", "здравствуй", "приветик", "ку" };

        public static String[] BayAnswer = { "пока", "всего хорошего", "всего доброго", "всех благ", "давай" };

        public static String[] LaunchAnswer = { "запущен", "открыт" };

        public static String[] IsNotLaunchAnswer = { "не удалось запустить", "не удалось открыть" };

        public static String[] IsLaunchedAnswer = { "уже запущен", "уже и так открыт", "и так открыт", "и так запущен", "в настаящий момент открыт", "в настаящий момент запущен" };

        public static String[] IsClosedAnswer = { "уже закрыт", "уже и так закрыт", "и так закрыт", "в настаящий момент закрыт"};

        public static String[] CloseAnswer = { "закрыт", "убран" };

        public static String[] OkeyAnswer = { "окей", "хорошо", "акей", "ок", "конечно" };

        public static String[] SilentAnswer = { "ну ладно", "ну хорошо", "молчу", "так и быть", "будет исполнено", "как хочешь", "как можелаешь" };

        public static String[] OffPCAnswer = { "выключу через 7 секудн", "будет выключен через 7 секунд", "готовся, выключится через 7 секунд", "выключится через 7 секунд"};

        public static String[] VolumeAnswer = {"готово", "громкость изменена", "сделано", "выполнено", "установил громкость"};

        public static String[] VolumeIsBadAnswer = {"выбери от одного до ста", "это некорректное значение для громкости", "такое значение не подходит", "плохое значение для громкости"};

        public static String[] CommandErrorAnswer = { "При выполнении команды произошла ошибка", "Хмм, что-то пошло не так", "Почему-то не смог выполнить команду", "Упс, какая-то ошибка", "Не могу выполнить из-за ошибка"};

        public static String[] CommandNotFound = { "Комманда не распознона", "Хэм, такой команды нету", "Ты уверен в своей команде?", "Упс, команда не найдена", "Повтори ка, такой команды нету"};
    }
}
