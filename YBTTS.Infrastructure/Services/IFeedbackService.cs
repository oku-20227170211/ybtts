using YBTTS.Core.Entities;

namespace YBTTS.Infrastructure.Services;

public interface IFeedbackService
{
    Task<Feedback> CreateAsync(int requestId, int studentId, int rating, string comment);
    Task<List<Feedback>> GetByRequestAsync(int requestId);
    Task<double> GetAverageRatingForStaffAsync(int staffId);
}
