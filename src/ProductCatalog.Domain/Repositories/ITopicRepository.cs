using SharedKernel.Application;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Repositories;

/// <summary>
/// Repository interface for Topic entity
/// </summary>
public interface ITopicRepository : IRepository<Topic>
{
    Task<IEnumerable<Topic>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default);
    Task<Topic?> GetByNameAndSubjectAsync(string topicName, int subjectId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAndSubjectAsync(string topicName, int subjectId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAndSubjectAsync(string topicName, int subjectId, int excludeId, CancellationToken cancellationToken = default);
}
