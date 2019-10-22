using Flow.Client.API.Models;

namespace Flow.Client.API.Interfaces
{
    public interface IFlowClient
    {
        CreateGroupResponse CreateGroup(CreateGroupParams createGroupParams);
    }
}