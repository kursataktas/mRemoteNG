﻿using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Linq;
using mRemoteNG.UI.Controls.ConnectionTree;
using System.Runtime.Versioning;

namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class PreviousSessionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public PreviousSessionOpener(IConnectionInitiator connectionInitiator)
        {
            if (connectionInitiator == null)
                throw new ArgumentNullException(nameof(connectionInitiator));
            _connectionInitiator = connectionInitiator;
        }

        public void Execute(IConnectionTree connectionTree)
        {
            System.Collections.Generic.IEnumerable<ConnectionInfo> connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList()
                                                   .Where(node => !(node is ContainerInfo));
            System.Collections.Generic.IEnumerable<ConnectionInfo> previouslyOpenedConnections = connectionInfoList
                .Where(item =>
                           item.PleaseConnect &&
                           //ignore items that have already connected
                           !_connectionInitiator.ActiveConnections.Contains(item.ConstantID));

            foreach (ConnectionInfo connectionInfo in previouslyOpenedConnections)
            {
                _connectionInitiator.OpenConnection(connectionInfo);
            }
        }
    }
}