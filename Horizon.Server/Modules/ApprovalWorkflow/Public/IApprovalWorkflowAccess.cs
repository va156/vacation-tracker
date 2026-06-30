namespace Horizon.Server.Modules.ApprovalWorkflow.Public
{
    /// <summary>
    /// Public cross-module contract that exposes approval workflow operations to other modules.
    /// Keeps module boundaries explicit and prevents direct coupling to internal repositories
    /// or services. Operations to be defined as features are implemented.
    /// </summary>
    public interface IApprovalWorkflowAccess
    {
    }
}
