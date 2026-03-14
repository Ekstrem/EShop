namespace Category.InternalContracts;

public interface ICategoryQueryRepository
{
    Task<CategoryReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryReadModel>> GetChildrenAsync(Guid? parentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryReadModel>> GetTreeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetSiblingNamesAsync(Guid? parentId, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasActiveChildrenAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Guid>> GetAncestorIdsAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
