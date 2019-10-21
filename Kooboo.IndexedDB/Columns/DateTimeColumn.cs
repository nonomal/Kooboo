//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Serializer.Simple;
using System;

namespace Kooboo.IndexedDB.Columns
{
    public class DateTimeColumn<TValue> : IColumn<TValue>
    {
        internal Func<TValue, DateTime> Get;
        internal Action<TValue, DateTime> Set;

        private IByteConverter<Int64> byteConverter;

        /// <summary>
        ///  The enum class type.
        /// </summary>
        public Type DataType
        {
            get; set;
        }

        public DateTimeColumn(string fieldName)
        {
            this.FieldName = fieldName;
            this.Get = Helper.ObjectHelper.GetGetValue<TValue, DateTime>(fieldName);
            this.Set = Helper.ObjectHelper.GetSetValue<TValue, DateTime>(fieldName);

            byteConverter = ObjectContainer.GetConverter<Int64>();

            this.DataType = typeof(DateTime);

            this.FieldNameHash = Helper.ObjectHelper.GetHashCode(this.FieldName);
            var fieldnamehashbytes = BitConverter.GetBytes(FieldNameHash);

            this.IsString = false;

            this.FieldNameLengthBytes = new byte[8];
            var lenbytes = BitConverter.GetBytes(this.Length);
            System.Buffer.BlockCopy(fieldnamehashbytes, 0, this.FieldNameLengthBytes, 0, 4);
            System.Buffer.BlockCopy(lenbytes, 0, this.FieldNameLengthBytes, 4, 4);
        }

        public byte[] GetBytes(TValue input)
        {
            DateTime fieldvalue = this.Get(input);
            return ValueConverter.DateTimeToBytes(fieldvalue);
        }

        public void SetBytes(TValue input, byte[] bytes)
        {
            if (this.Set != null)
            {
                var date = (DateTime)ValueConverter.FromDateTimeBytes(bytes);
                this.Set(input, date);
            }
        }

        public string FieldName
        {
            get;
            set;
        }

        public int Length
        {
            get
            {
                return 8;
            }
            set
            {
            }
        }

        public int relativePosition
        {
            get;
            set;
        }

        public bool IsString { get; set; }

        public byte[] FieldNameLengthBytes { get; set; }
        public int FieldNameHash { get; set; }
    }
}