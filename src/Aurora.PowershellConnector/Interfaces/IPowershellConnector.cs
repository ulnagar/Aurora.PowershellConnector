namespace Aurora.PowershellConnector.Interfaces;

using Aurora.PowershellConnector.Models;
using System.Security;

public interface IPowershellConnector : IDisposable
{
    void Connect(string username, SecureString password);
    
    /// <summary>
    /// Gets all Teams that are not marked Archived for the specific user.
    /// </summary>
    /// <param name="userEmail">Email address of the user who logged into the Teams system.</param>
    /// <returns>List of <see cref="Aurora.PowershellConnector.Models.Team"/>Team objects</returns>
    List<Team> GetTeams(string userEmail);
    List<TeamMember> GetTeamMembers(string groupId);
}
