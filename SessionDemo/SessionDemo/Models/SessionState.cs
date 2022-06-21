namespace SessionDemo.Models
{
    using System;

    /// <summary>
    /// Levels of access for clinic users
    /// </summary>
    public enum AccessLevels
    {
        /// <summary>
        /// Client user
        /// </summary>
        Client,
        
        /// <summary>
        /// Technician user
        /// </summary>
        Technician,
        
        /// <summary>
        /// Clinician user
        /// </summary>
        Clinician,
        
        /// <summary>
        /// Administrative user
        /// </summary>
        Admin
    }
    
    /// <summary>
    /// User session state
    /// </summary>
    /// <param name="StartTime">DateTime of session start</param>
    /// <param name="AccessLevel">AccessLevels level of access for session</param>
    public record SessionState(DateTime StartTime, AccessLevels AccessLevel);

}