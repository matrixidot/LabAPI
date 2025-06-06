using System;

namespace LabApi.Events.Arguments.ServerEvents;

/// <summary>
/// Represents the arguments for the <see cref="Handlers.ServerEvents.BanIssued"/> event.
/// </summary>
public class BanIssuedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BanIssuedEventArgs"/> class.
    /// </summary>
    /// <param name="banType">The type of the ban.</param>
    /// <param name="banDetails">The details of the ban.</param>
    public BanIssuedEventArgs(BanHandler.BanType banType, BanDetails banDetails)
    {
        BanType = banType;
        BanDetails = banDetails;
    }

    /// <summary>
    /// The type of ban that was issued.
    /// </summary>
    public BanHandler.BanType BanType { get; }

    /// <summary>
    /// The details of the ban that was issued.
    /// </summary>
    public BanDetails BanDetails { get; }
}