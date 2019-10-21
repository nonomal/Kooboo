//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System;
using System.Collections.Generic;

namespace Kooboo.IndexedDB
{
    public interface IObjectStore
    {
        string Name { get; set; }
        string ObjectFolder { get; set; }

        int Count();

        void Close();

        void DelSelf();

        void Flush();

        Database OwnerDatabase { get; set; }

        bool add(object key, object value);

        bool update(object key, object value);

        void delete(object key);

        object get(object key);

        List<object> List(int count = 9999, int skip = 0);

        void RollBack(LogEntry log);

        void RollBack(List<LogEntry> loglist);

        void RollBack(Int64 lastVersionId, bool selfIncluded = true);

        void RollBackTimeTick(Int64 timeTick, bool selfIncluded = true);

        void CheckOut(Int64 versionId, IObjectStore destinationStore, bool selfIncluded = true);

        void CheckOut(List<LogEntry> logs, IObjectStore destinationStore);

        int getLength(long blockposition);
    }
}