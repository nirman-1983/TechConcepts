using SharedKernel.Domain;

namespace ProductCatalog.Domain.Events;

public class TopicCreatedEvent : BaseDomainEvent
{
    public int TopicId { get; }
    public string TopicName { get; }
    public int SubjectId { get; }

    public TopicCreatedEvent(int topicId, string topicName, int subjectId)
    {
        TopicId = topicId;
        TopicName = topicName;
        SubjectId = subjectId;
    }
}

public class TopicUpdatedEvent : BaseDomainEvent
{
    public int TopicId { get; }
    public string OldName { get; }
    public string NewName { get; }

    public TopicUpdatedEvent(int topicId, string oldName, string newName)
    {
        TopicId = topicId;
        OldName = oldName;
        NewName = newName;
    }
}
