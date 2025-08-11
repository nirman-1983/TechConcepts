using SharedKernel.Domain;
using SharedKernel.Exceptions;
using ProductCatalog.Domain.Events;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Domain.Entities;

/// <summary>
/// Topic entity representing a learning topic within a subject
/// </summary>
public class Topic : AuditableEntity
{

    public TopicName TopicName { get; private set; } = TopicName.Create("Default");
    public int SubjectId { get; private set; }

    // Navigation property
    public Subject? Subject { get; private set; }

    // Private constructor for EF Core
    private Topic() { }

    public Topic(string topicName, int subjectId)
    {
        TopicName = TopicName.Create(topicName);
        SubjectId = subjectId;
        AddDomainEvent(new TopicCreatedEvent(Id, TopicName.Value, subjectId));
    }

    public void UpdateTopicName(string topicName)
    {
        var oldName = TopicName.Value;
        TopicName = TopicName.Create(topicName);
        AddDomainEvent(new TopicUpdatedEvent(Id, oldName, TopicName.Value));
    }
}
