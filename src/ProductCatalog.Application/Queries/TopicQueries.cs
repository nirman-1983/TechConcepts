using SharedKernel.Application;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries;

/// <summary>
/// Query to get all topics
/// </summary>
public class GetAllTopicsQuery : IQuery<GetTopicsResponse>
{

    public GetAllTopicsQuery()
    {
    }
}

/// <summary>
/// Query to get a topic by ID
/// </summary>
public class GetTopicByIdQuery : IQuery<GetTopicResponse?>
{
    public int Id { get; }

    public GetTopicByIdQuery(int id)
    {
        Id = id;
    }
}

/// <summary>
/// Query to get topics by subject ID
/// </summary>
public class GetTopicsBySubjectIdQuery : IQuery<GetTopicsResponse>
{
    public int SubjectId { get; }

    public GetTopicsBySubjectIdQuery(int subjectId)
    {
        SubjectId = subjectId;
    }
}

/// <summary>
/// Query to get a topic by name and subject
/// </summary>
public class GetTopicByNameAndSubjectQuery : IQuery<GetTopicResponse?>
{
    public string TopicName { get; }
    public int SubjectId { get; }

    public GetTopicByNameAndSubjectQuery(string topicName, int subjectId)
    {
        TopicName = topicName;
        SubjectId = subjectId;
    }
}
