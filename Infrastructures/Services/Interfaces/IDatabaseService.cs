namespace Infrastructures.Services;
public interface IDatabaseService
    {
        Task<bool> CheckConnectionAsync();
    }