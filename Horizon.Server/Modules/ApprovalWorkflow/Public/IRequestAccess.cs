namespace Horizon.Server.Modules.ApprovalWorkflow.Public
{
    /// <summary>
    /// Public cross-module contract for querying leave request data.
    /// Other modules (e.g. LeaveManagement) use this interface to check request status
    /// without taking a direct dependency on ApprovalWorkflow internals.
    /// </summary>
    public interface IRequestAccess
    {
    }
}
