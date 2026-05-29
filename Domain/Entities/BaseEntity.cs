namespace Domain.Entities
{
    /// <summary>
    /// Базовый класс для всех сущностей доменной модели.
    /// Содержит общий идентификатор и логику сравнения по этому идентификатору.
    /// </summary>
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Время создания сущности
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        /// <summary>
        /// Создаёт сущность с автоматически сгенерированным идентификатором.
        /// Используется при создании новых объектов доменной модели.
        /// </summary>
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Проверяет равенство двух сущностей по их идентификаторам.
        /// </summary>
        /// <param name="other">Сущность для сравнения.</param>
        public bool Equals(BaseEntity? other)
        {
            return other is not null && Id.Equals(other.Id);
        }

        /// <summary>
        /// Проверяет равенство текущего объекта с любым другим объектом.
        /// </summary>
        /// <param name="obj">Объект для сравнения.</param>
        public override bool Equals(object? obj)
        {
            return obj is BaseEntity other && Equals(other);
        }

        /// <summary>
        /// Генерирует hash code на основе идентификатора сущности.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        
        /// <summary>
        /// Перегрузка оператора равенства.
        /// </summary>
        public static bool operator ==(BaseEntity? first, BaseEntity? second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (first is null || second is null)
            {
                return false;
            }

            return first.Equals(second);
        }

        /// <summary>
        /// Перегрузка оператора неравенства.
        /// </summary>
        public static bool operator !=(BaseEntity? first, BaseEntity? second)
        {
            return !(first == second);
        }
    }
}