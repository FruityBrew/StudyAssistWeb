using System;

namespace StudyAssist.Core.Interfaces
{
    public interface IRepeatCalculator
    {
        int FirstRepeatPeriod { get; } 

        int SecondRepeatPeriod { get; }

        int ThirdRepeatPeriod { get; } 

        int FourthRepeatPeriod { get; } 

        int FifthRepeatPeriod { get; }

        int DefaultRepeatPeriod { get; } 

        /// <summary>
        /// Получает дату повтора, соответствующую уровню.
        /// </summary>
        /// <param name="studyLevel">Уровень обучения.</param>
        /// <param name="oldRepeateDate">Дата повтора, установленная ранее.
        /// </param>
        /// <returns>Дата повтора.</returns>
        DateTime GetRepeatDate(Int32 studyLevel, DateTime? oldRepeateDate);
    }
}
