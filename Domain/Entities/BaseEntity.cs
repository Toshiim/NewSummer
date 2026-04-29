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
        public Guid Id { get; protected set; }

        /// <summary>
        /// Создаёт сущность с указанным идентификатором.
        /// Используется ORM при восстановлении объектов из БД.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Создаёт сущность с автоматически сгенерированным идентификатором.
        /// Используется при создании новых объектов доменной модели.
        /// </summary>
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
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
    }
}