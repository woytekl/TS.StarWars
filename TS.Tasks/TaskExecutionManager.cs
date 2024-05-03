namespace TS.Tasks;

public class TaskExecutionManager
{
    private readonly TaskFactory _taskFactory;

    public TaskExecutionManager(int maxDegreeOfParallelism)
    {
        _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None, 
            new LimitedConcurrencyLevelTaskScheduler(maxDegreeOfParallelism));
    }

    public Task Run(Func<Task> func)
    {
        return _taskFactory.StartNew(func).Unwrap();
    }

    public Task<T> Run<T>(Func<Task<T>> func)
    {
        return _taskFactory.StartNew(func).Unwrap();
    }

    public Task Run(Action func)
    {
        return _taskFactory.StartNew(func);
    }

    public Task<T> Run<T>(Func<T> func)
    {
        return _taskFactory.StartNew(func);
    }
}
