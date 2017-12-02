using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prometheus.Client.Contracts;
using Prometheus.Client.Internal;

namespace Prometheus.Client
{
    public static class ScrapeHandler
    {
        private const string _protoContentType = "application/vnd.google.protobuf; proto=io.prometheus.client.MetricFamily; encoding=delimited";
        private const string _textContentType = "text/plain; version=0.0.4";
        private const string _protoAcceptType = "application/vnd.google.protobuf";

        public static void ProcessScrapeRequest(
            IEnumerable<MetricFamily> collected,
            string contentType,
            Stream outputStream)
        {
            if (contentType == _protoContentType)
            {
                ProtoFormatter.Format(outputStream, collected);
            }
            else
            {
                TextFormatter.Format(outputStream, collected);
            }
        }

        public static string GetContentType(IEnumerable<string> acceptHeaders)
        {
            return ProtobufAccepted(acceptHeaders) ? _protoContentType : _textContentType;
        }

        private static bool ProtobufAccepted(IEnumerable<string> acceptTypesHeader)
        {
            if (acceptTypesHeader == null)
                return false;

            var splitParams = acceptTypesHeader.Select(_ => _.Split(';'));
            var acceptTypes = splitParams.Select(_ => _.First()).ToList();

            return acceptTypes.Any(_ => _.Equals(_protoAcceptType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
