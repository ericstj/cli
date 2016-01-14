﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DotNet.ProjectModel
{
    public class ProjectContextCollection
    {
        public Project Project { get; set; }

        public List<ProjectContext> ProjectContexts { get; } = new List<ProjectContext>();

        public List<DiagnosticMessage> ProjectDiagnostics { get; } = new List<DiagnosticMessage>();

        public string LockFilePath { get; set; }

        public string ProjectFilePath { get; set; }

        public DateTime LastProjectFileWriteTime { get; set; }

        public DateTime LastLockFileWriteTime { get; set; }

        public bool HasChanged
        {
            get
            {
                if (ProjectFilePath == null || !File.Exists(ProjectFilePath))
                {
                    return true;
                }

                if (LastProjectFileWriteTime.Ticks < File.GetLastWriteTime(ProjectFilePath).Ticks)
                {
                    return true;
                }

                if (LockFilePath == null || !File.Exists(LockFilePath))
                {
                    return true;
                }

                if (LastLockFileWriteTime.Ticks < File.GetLastWriteTime(LockFilePath).Ticks)
                {
                    return true;
                }

                return false;
            }
        }

        public void Reset()
        {
            Project = null;
            ProjectContexts.Clear();
            ProjectFilePath = null;
            LockFilePath = null;
            LastLockFileWriteTime = DateTime.MinValue;
            LastProjectFileWriteTime = DateTime.MinValue;
            ProjectDiagnostics.Clear();
        }
    }
}
