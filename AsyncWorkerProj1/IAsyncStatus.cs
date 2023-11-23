namespace AsyncWorkerProj1
{
    /// <summary>
    /// Interface for a worker to deliver updates on status for a task. TODO rephrase
    /// </summary>
    public interface IAsyncStatus
    {
        bool IsDone();
        int ProgressNumber();
        // TODO perhaps add later: IEnumerable<string> PartialTextResults() and/or IEnumerable<object> PartialObjectResults()
    }
}
