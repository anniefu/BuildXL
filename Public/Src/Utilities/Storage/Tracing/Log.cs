// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if FEATURE_MICROSOFT_DIAGNOSTICS_TRACING
using Microsoft.Diagnostics.Tracing;
#endif
using System.Collections.Generic;
using BuildXL.Tracing;
using BuildXL.Utilities.Instrumentation.Common;
using BuildXL.Utilities.Tracing;
#if !FEATURE_MICROSOFT_DIAGNOSTICS_TRACING
using System.Diagnostics.Tracing;
#endif

#pragma warning disable 1591

namespace BuildXL.Storage.Tracing
{
    /// <summary>
    /// Logging for bxl.exe.
    /// </summary>
    [EventKeywordsType(typeof(Events.Keywords))]
    [EventTasksType(typeof(Events.Tasks))]
    public abstract partial class Logger
    {
        /// <summary>
        /// Returns the logger instance
        /// </summary>
        public static Logger Log { get { return m_log; } }

        #region Change detection / Journal-based incremental builds

        // ||| TODO: We should have well structured events like normal. As a stopgap, we've promoted some Console.WriteLine debugging to some single-string events.
        [GeneratedEvent(
            (int)EventId.ChangeDetectionDueToPerpetualDirtyNode,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "{0}")]
        public abstract void ChangeDetectionDueToPerpetualDirtyNode(LoggingContext context, string message);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionProbeSnapshotInconsistent,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "While attempting to probe and track the existence of {1}: Result of probing path {0} changed (previously non-existent; now probing returned status {2}). Retrying operation to get a consistent snapshot.")]
        public abstract void ChangeDetectionProbeSnapshotInconsistent(LoggingContext context, string changedPath, string fullPath, int nativeError);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionComputedDirectoryMembershipTrackingFingerprint,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Tracking membership of path '{0}' [{1} members; fingerprint {2}]")]
        public abstract void ChangeDetectionComputedDirectoryMembershipTrackingFingerprint(LoggingContext context, string path, int numberOfEntries, string fingerprint);

        [GeneratedEvent(
            (int)EventId.StartSavingChangeTracker,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Start,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Saving change tracking set to '{path}'")]
        public abstract void StartSavingChangeTracker(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.EndSavingChangeTracker,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Stop,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Done saving change tracking set")]
        public abstract void EndSavingChangeTracker(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.SavingChangeTracker,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Saving change tracking set: Path: {path} | Token: {token} | State: {state} | Tracked volume count: {trackedVolumeCount} | Elapsed time: {durationMs}ms")]
        public abstract void SavingChangeTracker(LoggingContext context, string path, string token, string state, int trackedVolumeCount, long durationMs);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionFailCreateTrackingSetDueToJournalQueryError,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Failed to create file change tracking for volume '({0:X16} @ {1})' set because journal query error '{2}': {3}")]
        public abstract void ChangeDetectionFailCreateTrackingSetDueToJournalQueryError(LoggingContext context, long volumeSerial, string volumePath, string errorStatus, string message);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionCreateResult,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Created file change tracking for volume '({0:X16} @ {1})': {2} (next USN: {3}, checkpoint: {4})")]
        public abstract void ChangeDetectionCreateResult(LoggingContext context, long volumeSerial, string volumePath, string status, string nextUsn, string checkpoint);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionSingleVolumeScanJournalResult,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Scan journal result for volume '({0:X16} @ {1})': Status: {2} | Checkpoint: {3} {4}")]
        public abstract void ChangeDetectionSingleVolumeScanJournalResult(LoggingContext context, long volumeSerial, string volumePath, string status, string checkpoint, string statMessage);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionSingleVolumeScanJournalResultTelemetry,
            EventGenerators = EventGenerators.TelemetryOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Scan journal result for volume '({0:X16} @ {1})': Status: {2} | Checkpoint: {3} {4}")]
        public abstract void ChangeDetectionSingleVolumeScanJournalResultTelemetry(LoggingContext context, long volumeSerial, string volumePath, string status, string checkpoint, string statMessage, IDictionary<string, long> stats);

        [GeneratedEvent(
            (int)EventId.ChangedPathsDetectedByJournalScanning,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Some changed paths detected by scanning journal in volume '({0:X16} @ {1})': \r\n{2}")]
        public abstract void ChangedPathsDetectedByJournalScanning(LoggingContext context, long volumeSerial, string volumePath, string changedPaths);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionParentPathIsUntrackedOnTrackingAbsentRelativePath,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeDetection,
            Message = "Parent path '{parentPath}' is untracked on tracking absent relative path '{relativePath}'")]
        public abstract void ChangeDetectionParentPathIsUntrackedOnTrackingAbsentRelativePath(LoggingContext context, string parentPath, string relativePath);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionScanJournalFailedSinceJournalGotOverwritten,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed scanning journal for volume '({0:X16} @ {1})' because it got overwritten due to its small size, '{journalSize}' bytes")]
        public abstract void ChangeDetectionScanJournalFailedSinceJournalGotOverwritten(LoggingContext context, long volumeSerial, string volumePath, string journalSize);

        [GeneratedEvent(
            (int)EventId.ChangeDetectionScanJournalFailedSinceTimeout,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed scanning journal for volume '({0:X16} @ {1})' due to timeout")]
        public abstract void ChangeDetectionScanJournalFailedSinceTimeout(LoggingContext context, long volumeSerial, string volumePath);

        [GeneratedEvent(
            (int)EventId.AntiDependencyValidationPotentiallyAddedButVerifiedAbsent,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Path '{0}' was potentially added, but verified actually absent (re-tracked)")]
        public abstract void AntiDependencyValidationPotentiallyAddedButVerifiedAbsent(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.IgnoredRecordsDueToUnchangedJunctionRootCount,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Ignored an usn record that is due to deleting and recreating a junction root '({path})' and the target is the same.")]
        public abstract void IgnoredRecordsDueToUnchangedJunctionRootCount(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.TrackChangesToFileDiagnostic,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.Diagnostics,
            Message = "Track changes to file/directory '{path}' ({identity}), with '{mode}' mode.")]
        public abstract void TrackChangesToFileDiagnostic(LoggingContext context, string path, string identity, string mode);

        [GeneratedEvent(
            (int)EventId.AntiDependencyValidationFailedRetrackPathAsNonExistent,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to re-track the path as non-existent '{0}' (must assume it existent now): {1}")]
        public abstract void AntiDependencyValidationFailedRetrackPathAsNonExistent(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.AntiDependencyValidationFailedProbePathToVerifyNonExistent,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to probe the path '{0}' to verify if it is still non-existent (must assume it existent now): {1}")]
        public abstract void AntiDependencyValidationFailedProbePathToVerifyNonExistent(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.AntiDependencyValidationStats,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Anti dependency validation: Verified to be absent: {0} | Failed to retrack as non-existent: {1} | Failed to probe: {2} | Elapsed time: {3}ms")]
        public abstract void AntiDependencyValidationStats(LoggingContext context, int numOfVerifiedToBeAbsent, int numOfFailedToRetrackAsNonExistent, int numOfFailedToProbe, long elapsedMs);

        [GeneratedEvent(
            (int)EventId.EnumerationDependencyValidationPotentiallyAddedOrRemovedDirectChildrenButVerifiedUnchanged,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Path '{0}' potentially had direct children added or removed, but was verified to be unchanged (re-tracked)")]
        public abstract void EnumerationDependencyValidationPotentiallyAddedOrRemovedDirectChildrenButVerifiedUnchanged(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.EnumerationDependencyValidationFailedRetrackUnchangedDirectoryForMembershipChanges,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to re-track an unchanged directory for membership changes '{0}' (must assume it changed now): {1}")]
        public abstract void EnumerationDependencyValidationFailedRetrackUnchangedDirectoryForMembershipChanges(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.EnumerationDependencyValidationFailedToOpenOrEnumerateDirectoryForMembershipChanges,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to open and enumerate the path '{0}' to verify that its membership has not changed (must assume it changed now): {1}")]
        public abstract void EnumerationDependencyValidationFailedToOpenOrEnumerateDirectoryForMembershipChanges(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.EnumerationDependencyValidationStats,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Enumeration dependency validation: Unchanged: {0} | Failed to retrack: {1} | Failed to open and enumerate: {2} | Elapsed time: {3}ms")]
        public abstract void EnumerationDependencyValidationStats(LoggingContext context, int numOfUnchanges, int numOfFailedToRetrack, int numOfFailedToOpenAndEnumerate, long elapsedMs);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationPotentiallyChangedButVerifiedUnchanged,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Hard link '{0}' was potentially changed, but verified to not changed (re-tracked)")]
        public abstract void HardLinkValidationPotentiallyChangedButVerifiedUnchanged(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationHardLinkChangedBecauseFileIdChanged,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Hard link '{0}' has changed because its file id has changed, perhaps due to deletion or its target is modified")]
        public abstract void HardLinkValidationHardLinkChangedBecauseFileIdChanged(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationFailedRetrackUnchangedHardLink,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to re-track potentially unchanged hard link '{0}' (must assume it changed now): {1}")]
        public abstract void HardLinkValidationFailedRetrackUnchangedHardLink(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationFailedToOpenHardLinkDueToNonExistent,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to open the hard link '{0}' because hard link is non-existent (must assume it changed)")]
        public abstract void HardLinkValidationFailedToOpenHardLinkDueToNonExistent(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationFailedToOpenOrTrackHardLink,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to open and track the hard link '{0}' (must assume it changed): {1}")]
        public abstract void HardLinkValidationFailedToOpenOrTrackHardLink(LoggingContext context, string path, string failureMessage);

        [GeneratedEvent(
            (int)EventId.HardLinkValidationStats,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Hard link validation: Unchanged: {0} | File id changes: {1} | Failed to retrack: {2} | Non-existent: {3} | Failed to open: {4} | Elapsed time: {5}ms")]
        public abstract void HardLinkValidationStats(LoggingContext context, int numOfUnchanges, int numOfFileIdChanges, int numOfFailedToRetrack, int numOfNonExistent, int numOfFailedToOpenAndTrack, long elapsedMs);

        #endregion

        #region Change Journal Service

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceRequestStart,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            EventOpcode = (byte)EventOpcode.Start,
            Message = "Start: A client has connected to the change journal service.")]
        public abstract void ChangeJournalServiceRequestStart(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceRequestStop,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            EventOpcode = (byte)EventOpcode.Stop,
            Message = "Stop: A client has disconnected from the change journal service.")]
        public abstract void ChangeJournalServiceRequestStop(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.ChangeJournalPipeServerInstanceThreadCrash,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Critical,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "A pipe worker for the change journal service has failed due to an unhandled exception (HRESULT {0}): {1}")]
        public abstract void ChangeJournalPipeServerInstanceThreadCrash(LoggingContext context, int hresult, string exceptionMessage);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceRequestIOError,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "Change journal client connection failed (HRESULT {0:X}): {1}")]
        public abstract void ChangeJournalServiceRequestIOError(LoggingContext context, int hresult, string exceptionMessage);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceProtocolError,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "A change journal client sent a malformed request. It will be disconnected forcefully.")]
        public abstract void ChangeJournalServiceProtocolError(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceReadJournalRequest,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "Read Journal request: VolumePath={0}, StartUSN={1:X16}")]
        public abstract void ChangeJournalServiceReadJournalRequest(LoggingContext context, string volumePath, ulong startUsn);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceQueryJournalRequest,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "Query Journal request: VolumePath={0}")]
        public abstract void ChangeJournalServiceQueryJournalRequest(LoggingContext context, string volumePath);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceQueryServiceVersionRequest,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "Query service version request: <no parameters>")]
        public abstract void ChangeJournalServiceQueryServiceVersionRequest(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.ChangeJournalServiceUnsupportedProtocolVersion,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.ChangeJournalService,
            Message = "A change journal client has been rejected due to requesting an unsupported protocol version. Client version: {0} ; Server version: {1}")]
        public abstract void ChangeJournalServiceUnsupportedProtocolVersion(LoggingContext context, int clientVersion, int serverVersion);

        #endregion

        [GeneratedEvent(
            (int)EventId.StorageFileContentTableIgnoringFileSinceVersionedFileIdentityIsNotSupported,
                    EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message =
                "Versioned file identity for '{path}' cannot be established. This file will be re-hashed on subsequent builds, which negatively impacts performance. "
                + "Note that additional paths may also be affected. For Windows, verify that all source and output volumes have enabled change journals. "
                + "{message}")]
        public abstract void StorageFileContentTableIgnoringFileSinceVersionedFileIdentityIsNotSupported(LoggingContext context, string path, string message);

        [GeneratedEvent(
            (int)EventId.StorageLoadFileContentTable,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message =
                "Load file content table at '{path}': Status: {status} | Reason: {reason} | Elapsed: {elapsed}ms{stackTrace}")]
        public abstract void StorageLoadFileContentTable(LoggingContext context, string path, string status, string reason, long elapsed, string stackTrace);

        /*
        [GeneratedEvent(
            (int)EventId.StorageCacheFlushingBegin,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            EventOpcode = (byte)EventOpcode.Start,
            Message = "The cache has begun to wait for outstanding asynchronous operations to complete.")]
        public abstract void StorageCacheFlushingBegin(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.StorageCacheFlushingComplete,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            EventOpcode = (byte)EventOpcode.Stop,
            Message = "The cache's asynchronous operations have completed.")]
        public abstract void StorageCacheFlushingComplete(LoggingContext context);
        */
        [GeneratedEvent(
            (int)EventId.StorageCacheCopyLocalError,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message = "While bringing {0} local, the cache reported error: {1}")]
        public abstract void StorageCacheCopyLocalError(LoggingContext context, string contentHash, string errorMessage);

        /*
        [GeneratedEvent(
            (int)EventId.StorageCacheGetContentUsingFallback,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Placing content {0}: Trying ingress of fallback path '{1}' since content not in cache.")]
        public abstract void StorageCacheGetContentUsingFallback(LoggingContext context, string contentHash, string fallbackPath);

        [GeneratedEvent(
            (int)EventId.StorageCacheStartupError,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Error,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Starting up the cache resulted in an error: {0}")]
        public abstract void StorageCacheStartupError(LoggingContext context, string errorMessage);
        */

        [GeneratedEvent(
            (int)EventId.StorageCacheContentPinned,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (ushort)Events.Tasks.PipExecutor,
            Message = "Pinned a CAS entry {casEntry} to cache {cacheId}")]
        public abstract void StorageCacheContentPinned(LoggingContext loggingContext, string casEntry, string cacheId);

        [GeneratedEvent(
            (int)EventId.StorageKnownUsnHit,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Hit (matched known USN): Path {0} (id {1:X16}-{2:X16} on {3:X16}) @ {4:X16} => {5}")]
        public abstract void StorageKnownUsnHit(LoggingContext context, string path, ulong idHigh, ulong idLow, ulong volumeSerial, ulong usn, string contentHash);

        [GeneratedEvent(
            (int)EventId.StorageUnknownUsnMiss,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Miss (unknown USN): Path {0} (id {1:X16}-{2:X16} on {3:X16}) @ {4:X16} (known {5:X16} => {6})")]
        public abstract void StorageUnknownUsnMiss(LoggingContext context, string path, ulong idHigh, ulong idLow, ulong volumeSerial, ulong readUsn, ulong knownUsn, string knownContentHash);

        [GeneratedEvent(
            (int)EventId.StorageUnknownFileMiss,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Miss (unknown file ID): Path {0} (id {1:X16}-{2:X16} on {3:X16}) (no USN => content mapping known)")]
        public abstract void StorageUnknownFileMiss(LoggingContext context, string path, ulong idHigh, ulong idLow, ulong volumeSerial, ulong readUsn);

        [GeneratedEvent(
            (int)EventId.StorageVersionedFileIdentityNotSupportedMiss,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Miss (version file identity not supported): Path {0}")]
        public abstract void StorageVersionedFileIdentityNotSupportedMiss(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.StorageRecordNewKnownUsn,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Record known USN: Path {0} (id {1:X16}-{2:X16} on {3:X16}) ->@ {4:X16} => {5}")]
        public abstract void StorageRecordNewKnownUsn(LoggingContext context, string path, ulong idHigh, ulong idLow, ulong volumeSerial, ulong newUsn, string contentHash);

        [GeneratedEvent(
            (int)EventId.StorageUsnMismatchButContentMatch,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.Diagnostics,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Usn number is changed but content is unchanged: Path => {0} OldUSN => {1:X16} NewUSN => {2:X16} ContentHash => {3}")]
        public abstract void StorageUsnMismatchButContentMatch(LoggingContext context, string path, ulong oldUsn, ulong newUsn, string contentHash);

        [GeneratedEvent(
            (int)EventId.StorageVolumeCollision,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message = "The local volumes {1} and {2} have the same serial {0:X16}. This causes slower builds. You should take the necessary steps to have two unique serial IDs.")]
        public abstract void StorageVolumeCollision(LoggingContext context, ulong serial, string guidPathA, string guidPathB);

        [GeneratedEvent(
            (ushort)EventId.FailedCheckingDirectJournalAccess,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (ushort)Events.Tasks.Storage,
            Message = "Encountered an error while checking whether process can access the journal directly. The build may still proceed but without use of change journal scanning. {errorMessage}")]
        internal abstract void FailedCheckingDirectJournalAccess(LoggingContext context, string errorMessage);

        [GeneratedEvent(
            (int)EventId.StartLoadingChangeTracker,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Start,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Loading previous change tracking set from '{path}'")]
        public abstract void StartLoadingChangeTracker(LoggingContext context, string path);

        [GeneratedEvent(
            (int)EventId.EndLoadingChangeTracker,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Stop,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Done loading previous change tracking set")]
        public abstract void EndLoadingChangeTracker(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.LoadingChangeTracker,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Loading previous change tracking set: Path: {path} |  Token: {token} | Status: {status} | Reason: {reason} | Tracked volume count: {trackedVolumeCount} | Tracked journal size: {trackedJournalSizeByte} bytes| Elapsed time: {durationMs}ms")]
        public abstract void LoadingChangeTracker(LoggingContext context, string path, string token, string status, string reason, int trackedVolumeCount, long trackedJournalSizeByte, long durationMs);

        [GeneratedEvent(
            (int)EventId.DisableChangeTracker,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)Events.Keywords.UserMessage,
            Message = "Failed to track changes to '{path}', and so tracking become incomplete and a subsequent build will not be able to incrementally scan for changes: {reason}")]
        public abstract void DisableChangeTracker(LoggingContext context, string path, string reason);

        [GeneratedEvent(
            (int)EventId.StartScanningJournal,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Start,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Scanning USN journal")]
        public abstract void StartScanningJournal(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.ScanningJournal,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance),
            Message = "Scanning journal: Token: {token} | Status: {status} | Elapsed time: {durationMs}ms")]
        public abstract void ScanningJournal(LoggingContext context, string token, string status, long durationMs);

        [GeneratedEvent(
            (int)EventId.EndScanningJournal,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            EventTask = (ushort)Events.Tasks.ChangeDetection,
            EventOpcode = (byte)EventOpcode.Stop,
            Keywords = (int)(Events.Keywords.UserMessage | Events.Keywords.Performance | Events.Keywords.Progress),
            Message = Events.PhasePrefix + "Done scanning USN journal")]
        public abstract void EndScanningJournal(LoggingContext context);

        [GeneratedEvent(
            (int)EventId.FileCombinerVersionIncremented,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Parser,
            Message =
                "FileCombiner for usage '{0}' ignored because its format has changed. It will be recreated.")]
        public abstract void FileCombinerVersionIncremented(LoggingContext context, string usage);

        [GeneratedEvent(
            (int)EventId.FileCombinerFailedToInitialize,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Parser,
            Message =
                "FileCombiner for usage '{0}' failed to load. It will be recreated. Exception: {1}")]
        public abstract void FileCombinerFailedToInitialize(LoggingContext context, string usage, string exception);

        [GeneratedEvent(
            (int)EventId.FileCombinerFailedToCreate,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Warning,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Parser,
            Message =
                "FileCombiner for usage '{0}' failed creation. It will be disabled for this execution. Exception: {1}")]
        public abstract void FileCombinerFailedToCreate(LoggingContext context, string usage, string exception);

#pragma warning disable CA1823 // Unused field
        private const string FileCombinerStatsMessage = "BeginCount:{stats.BeginCount} InitializationTimeMs:{stats.InitializationTimeMs} Hits:{stats.Hits} Misses:{stats.Misses} UnreferencedPercent:{stats.UnreferencedPercent} EndCount:{stats.EndCount} FinalSizeInMB:{stats.CompactingTimeMs} EndCount:{stats.CompactingTimeMs}";

#pragma warning restore CA1823 // Unused field
        [GeneratedEvent(
            (int)EventId.SpecCache,
            EventGenerators = EventGenerators.LocalAndTelemetryAndStatistic,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Parser,
            Message =
                "SpecCache stats: " + FileCombinerStatsMessage)]
        public abstract void SpecCache(LoggingContext context, FileCombinerStats stats);

        [GeneratedEvent(
            (int)EventId.IncrementalFrontendCache,
            EventGenerators = EventGenerators.LocalAndTelemetryAndStatistic,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Parser,
            Message =
                "Incremental {ShortScriptName} cache stats: " + FileCombinerStatsMessage)]
        public abstract void IncrementalFrontendCache(LoggingContext context, FileCombinerStats stats);

        [GeneratedEvent(
            (int)EventId.ValidateJunctionRoot,
            EventGenerators = EventGenerators.LocalOnly,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (int)Events.Tasks.Storage,
            Message = "Junction root FileId check: Path: {path} | Result: {result}")]
        public abstract void ValidateJunctionRoot(LoggingContext context, string path, string result);

        [GeneratedEvent(
            (ushort)EventId.ConflictDirectoryMembershipFingerprint,
            EventGenerators = EventGenerators.LocalAndTelemetry,
            EventLevel = Level.Verbose,
            Keywords = (int)Events.Keywords.UserMessage,
            EventTask = (ushort)Events.Tasks.Storage,
            Message = "Directory '{path}' is enumerated multiple times with different resulting directory fingerprints; this indicates that the membership of directory changed during the build")]
        public abstract void ConflictDirectoryMembershipFingerprint(LoggingContext loggingContext, string path);

        public class FileCombinerStats
        {
            public long BeginCount;
            public long InitializationTimeMs;
            public long Hits;
            public long Misses;
            public long UnreferencedPercent;
            public long EndCount;
            public long FinalSizeInMB;
            public long CompactingTimeMs;
        }
    }
}
