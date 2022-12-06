namespace Aurora.PowershellConnector.Services;

using Aurora.PowershellConnector.Interfaces;
using Aurora.PowershellConnector.Models;
using Microsoft.PowerShell;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

public sealed class TeamsPowershellConnector : IPowershellConnector
{
    private readonly Runspace _runspace;

    public TeamsPowershellConnector()
    {
        InitialSessionState initial = InitialSessionState.CreateDefault();
        initial.ExecutionPolicy = ExecutionPolicy.Unrestricted;
        initial.ImportPSModule(new[] { @"C:\Program Files\WindowsPowerShell\Modules\MicrosoftTeams\4.9.0\MicrosoftTeams.psm1" });
        _runspace = RunspaceFactory.CreateRunspace(initial);
        _runspace.Open();
    }

    public void Connect(string username, SecureString password)
    {
        PowerShell ps = PowerShell.Create();

        PSCredential credential = new(username, password);

        ps.Streams.Error.DataAdded += ErrorEventHandler;

        ps.Runspace = _runspace;
        ps.AddStatement()
            .AddCommand("Connect-MicrosoftTeams")
            .AddParameter("Credential", credential);

        ps.Invoke();
    }

    public List<Team> GetTeams(string userEmail)
    {
        PowerShell ps = PowerShell.Create();

        ps.Streams.Error.DataAdded += ErrorEventHandler;

        ps.Runspace = _runspace;
        ps.AddStatement()
            .AddCommand("Get-Team")
            .AddParameter("User", userEmail)
            .AddParameter("Archived", false);

        Collection<PSObject> results = ps.Invoke();

        return ConvertObjects<Team>(results);
    }

    public List<TeamMember> GetTeamMembers(string groupId)
    {
        PowerShell ps = PowerShell.Create();

        ps.Streams.Error.DataAdded += ErrorEventHandler;

        ps.Runspace = _runspace;
        ps.AddStatement()
            .AddCommand("Get-TeamUser")
            .AddParameter("GroupId", groupId);

        Collection<PSObject> results = ps.Invoke();

        return ConvertObjects<TeamMember>(results);
    }



    public void Dispose()
    {
        _runspace.Close();
        _runspace.Dispose();
    }

    private List<T> ConvertObjects<T>(ICollection<PSObject> results)
    {
        List<T> returnData = new();

        foreach (PSObject obj in results)
        {
            string objectString = JsonConvert.SerializeObject(obj.Properties.ToDictionary(k => k.Name, k => k.Value));

            if (objectString is not null)
            {
                T convertedObject = JsonConvert.DeserializeObject<T>(objectString);
                returnData.Add(convertedObject);
            }
        }

        return returnData;
    }

    private static void ErrorEventHandler(object? sender, DataAddedEventArgs e)
    {
        if (sender is not null && e is not null)
            Console.WriteLine(((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString());
    }
}
