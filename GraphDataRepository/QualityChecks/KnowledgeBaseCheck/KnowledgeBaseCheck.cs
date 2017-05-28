﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using VDS.RDF;

namespace GraphDataRepository.QualityChecks.KnowledgeBaseCheck
{
    /// <summary> 
    /// Checks whether triple subjects exist in external knowledge base and if custom SPARQL query for them has results.
    /// Example: Checking if subject is in DBpedia knowledge base and if it's concept is known in YAGO project.
    /// </summary>
    public class KnowledgeBaseCheck : QualityCheck
    {
        public override QualityCheckReport CheckGraphs(IEnumerable<IGraph> graphs, IEnumerable<string> parameters = null)
        {
            var parallelOptions = new ParallelOptions {CancellationToken = CancellationTokenSource.Token};
            Parallel.ForEach(graphs, parallelOptions, graph =>
            {
                var tripleSubjects = graph.Triples.Select(triple => triple.Subject.ToString()).Distinct().ToList();
                //TODO
            });

            throw new System.NotImplementedException();
        }

        public override QualityCheckReport CheckData(IEnumerable<string> triples, IEnumerable<IGraph> graphs = null, IEnumerable<string> parameters = null)
        {
            throw new System.NotImplementedException();
        }

        public override void FixErrors(QualityCheckReport qualityCheckReport, string dataset, IEnumerable<int> errorsToFix)
        {
            throw new System.NotImplementedException();
        }

        public override bool ImportParameters(IEnumerable<object> parameters)
        {
            throw new System.NotImplementedException();
        }

        public KnowledgeBaseCheck(ILog log) : base(log)
        {
        }
    }
}
