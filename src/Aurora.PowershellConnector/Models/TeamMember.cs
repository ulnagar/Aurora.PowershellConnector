﻿namespace Aurora.PowershellConnector.Models;

public sealed class TeamMember : IEquatable<TeamMember>
{
    public Guid GroupId { get; internal set; }
    public Guid UserId { get; internal set; }
    public string User { get; internal set; } = string.Empty;
    public string Name { get; internal set; } = string.Empty;
    public TeamMemberRole Role { get; internal set; }

    public bool Equals(TeamMember? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() &&
            GroupId == other.GroupId &&
            UserId == other.UserId &&
            Role == other.Role;
    }

    public override bool Equals(object? obj)
    {
        return obj is TeamMember other &&
            Equals(other);
    }

    public override int GetHashCode()
    {
        return $"{GroupId}.{UserId}.{Role}".GetHashCode();
    }

    public override string ToString()
    {
        return $"User {Name} is {Role} of Team {GroupId}";
    }
}