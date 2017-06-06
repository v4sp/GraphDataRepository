﻿using System.Collections.Generic;

namespace GraphDataRepository.QualityChecks
{
    /// <summary>
    /// Indicates if quality check passed and lists errors
    /// </summary>
    public class QualityCheckReport
    {
        public bool QualityCheckPassed { get; set; }

        //Key: ID, value: graph URI, triple, error message and flag indicating if it's possible to fix the error automatically
        public readonly Dictionary<int, (string graphUri, string triple, string errorMessage, bool canFixAutomatically)> ErrorsById = new Dictionary<int, (string, string, string, bool )>();
    }
}