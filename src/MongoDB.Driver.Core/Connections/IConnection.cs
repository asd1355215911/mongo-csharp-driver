﻿/* Copyright 2013-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.WireProtocol.Messages;

namespace MongoDB.Driver.Core.Connections
{
    /// <summary>
    /// Represents a connection.
    /// </summary>
    public interface IConnection : IDisposable
    {
        // properties
        ConnectionId ConnectionId { get; }
        ConnectionDescription Description { get; }
        DnsEndPoint EndPoint { get; }
        ConnectionSettings Settings { get; }

        // methods
        IConnection Fork();
    }

    /// <summary>
    /// Represents additional IConnection methods that are only used internally.
    /// </summary>
    public interface IConnectionInternal : IConnection
    {
        // methods
        Task<ReplyMessage<TDocument>> ReceiveMessageAsync<TDocument>(int responseTo, IBsonSerializer<TDocument> serializer, TimeSpan timeout, CancellationToken cancellationToken);
        Task SendMessagesAsync(IEnumerable<RequestMessage> messages, TimeSpan timeout, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents a connection that hasn't been wrapped.
    /// </summary>
    public interface IRootConnection : IConnectionInternal
    {
        // properties
        int PendingResponseCount { get; }

        // methods
        Task OpenAsync(TimeSpan timeout, CancellationToken cancellationToken);
        void SetConnectionId(ConnectionId connectionId);
        void SetDescription(ConnectionDescription description);
    }
}