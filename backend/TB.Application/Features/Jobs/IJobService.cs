namespace TB.Application.Features.Jobs;

public interface IJobService
{
    Task<List<JobDto>> GetJobsAsync(string? skillFilter, CancellationToken cancellationToken = default);
    
    Task<JobDto?> CreateJobAsync(
        Guid employerId, 
        CreateJobRequest request, 
        CancellationToken cancellationToken = default);
}