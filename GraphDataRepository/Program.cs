﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BrightstarDB;
using BrightstarDB.Client;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using Vocab;

namespace GraphDataRepository
{
    class Program
    {
        static void Main(string[] args)
        {
            Playground();
        }

        private static void Playground()
        {
            //TestGraph();

            /********************************/
            /***********BRIGHTSTAR**********/
            /********************************/

            var client = BrightstarService.GetClient("Type=rest;endpoint=http://192.168.1.112:8090/brightstar;");
            string storeName = "Store_" + Guid.NewGuid();
            client.CreateStore(storeName);


            var addTriples = new StringBuilder();
            addTriples.AppendLine(
                "<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/name> \"BrightstarDB\" <http://example.org/graphs/alice> .");
            addTriples.AppendLine(
                "<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/nosql> <http://example.org/graphs/alice> .");
            addTriples.AppendLine(
                "<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/.net> <http://example.org/graphs/alice> .");
            addTriples.AppendLine(
                "<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/rdf> <http://example.org/graphs/alice> .");
            var transactionData = new UpdateTransactionData
            {
                InsertData = addTriples.ToString(),
                //DefaultGraphUri = "http://example.org/graphs/alice"
            };

            var jobInfo = client.ExecuteTransaction(storeName, transactionData);
            var namedGraphs = client.ListNamedGraphs(storeName);

            var query = "SELECT ?category WHERE { " +
                        "<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> ?category ." +
                        "}";

            var result = XDocument.Load(client.ExecuteQuery(storeName, query, "http://example.org/graphs/alice"));
            var result2 = client.ExecuteQuery(storeName, query, resultsFormat: SparqlResultsFormat.Xml);

            /********************************/
            /************DOTNETRDF***********/
            /********************************/

            SparqlConnector connect = new SparqlConnector(new Uri($"http://192.168.1.112:8090/brightstar/{storeName}//SPARQL"));
            PersistentTripleStore store = new PersistentTripleStore(connect);
            Object results = store.ExecuteQuery(query);//TODO: search in !default graph
            if (results is SparqlResultSet)
            {
                //Print out the results
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (VDS.RDF.Query.SparqlResult resultxxxx in rset)
                {
                    Console.WriteLine(resultxxxx.ToString());
                }

            }

            var y = store.UnderlyingStore.ListGraphs();

            foreach (var graphUri in store.UnderlyingStore.ListGraphs())
            {
                var z = store.Graphs.FirstOrDefault(g => g.BaseUri == graphUri);
            }

            //IGraph g = new Graph();
            //var x = new RdfXmlParser();
            //x.Load(g, result);

            //var gNamespaceMap = g.NamespaceMap;
            //foreach (var triple in g.Triples)
            //{
            //    Console.WriteLine(triple.Predicate);
            //}
            Console.WriteLine("KUNIEC");
            Console.ReadLine();
        }

        private static void TestGraph()
        {
            var dataGraph = new Graph();
            FileLoader.Load(dataGraph, @"..\..\TestData\RDF\foaf_example.rdf");

            var schemaGraph = new Graph();
            FileLoader.Load(schemaGraph, @"..\..\TestData\Schemas\foaf_20140114.rdf");

            var subjectList = schemaGraph.Triples.Select(triple => triple.Subject.ToString()).Distinct().ToList();

            var wrongPredicates = new List<string>();
            foreach (var triple in dataGraph.Triples)
            {
                var predicate = triple.Predicate.ToString();
                if (!subjectList.Contains(predicate) && !wrongPredicates.Contains(predicate))
                {
                    wrongPredicates.Add(predicate);
                }
            }

            foreach (var predicate in wrongPredicates)
            {
                Console.WriteLine(predicate);
            }
        }
    }
}