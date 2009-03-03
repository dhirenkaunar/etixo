//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Oxite.Mvc
{
    public class AppSettingsHelper
    {
        private NameValueCollection settings;

        public AppSettingsHelper() : this(ConfigurationManager.AppSettings)
        {
        }

        public AppSettingsHelper(NameValueCollection settings)
        {
            this.settings = settings;
        }

        public string this[string name]
        {
            get
            {
                return settings[name];
            }
        }

        public string this[int index]
        {
            get
            {
                return settings[index];
            }
        }

        public string Get(string key)
        {
            return Get(key, null);
        }

        public string Get(string key, string defaultValue)
        {
            string val = settings[key];

            if (val == null)
            {
                if (defaultValue == null)
                {
                    throw new ArgumentException(string.Format("AppSetting '{0}' was not found.", key));
                }
                return defaultValue;
            }

            return val;
        }

        public byte GetByte(string key)
        {
            return GetByte(key, null);
        }

        public byte GetByte(string key, byte defaultValue)
        {
            return GetByte(key, (byte?)defaultValue);
        }

        private byte GetByte(string key, byte? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return byte.Parse(Get(key));
            }

            string value = Get(key, "");

            return value == "" ? defaultValue.Value : byte.Parse(value);
        }

        public short GetInt16(string key)
        {
            return GetInt16(key, null);
        }

        public short GetInt16(string key, short defaultValue)
        {
            return GetInt16(key, (short?)defaultValue);
        }

        private short GetInt16(string key, short? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return short.Parse(Get(key));
            }

            string value = Get(key, "");

            return value == "" ? defaultValue.Value : short.Parse(value);
        }

        public int GetInt32(string key)
        {
            return GetInt32(key, null);
        }

        public int GetInt32(string key, int defaultValue)
        {
            return GetInt32(key, (int?)defaultValue);
        }

        private int GetInt32(string key, int? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return int.Parse(Get(key));
            }

            string value = Get(key, "");

            return value == "" ? defaultValue.Value : int.Parse(value);

        }

        public long GetInt64(string key)
        {
            return GetInt64(key, null);
        }

        public long GetInt64(string key, long defaultValue)
        {
            return GetInt64(key, (long?)defaultValue);
        }

        private long GetInt64(string key, long? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return long.Parse(Get(key));
            }

            string value = Get(key, "");

            return value == "" ? defaultValue.Value : long.Parse(value);
        }

        public bool GetBoolean(string key)
        {
            return GetBoolean(key, null);
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            return GetBoolean(key, (bool?)defaultValue);
        }

        private bool GetBoolean(string key, bool? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return bool.Parse(Get(key));
            }

            string returnValue = Get(key, "");

            return returnValue == "" ? defaultValue.Value : bool.Parse(returnValue);
        }

        public string[] GetArray(string key)
        {
            return GetArray(key, (string[])null);
        }

        public string[] GetArray(string key, params char[] delimeter)
        {
            return GetArray(key, null, delimeter);
        }

        public string[] GetArray(string key, string[] defaultValue)
        {
            return GetArray(key, defaultValue, ',');
        }

        public string[] GetArray(string key, string[] defaultValue, params char[] delimeter)
        {
            if (defaultValue == null)
            {
                //TODO: (erikpo) Add "caching"
                return Get(key).Split(delimeter);
            }
            string returnValue = Get(key, "");

            return returnValue == "" ? defaultValue : returnValue.Split(delimeter);
        }
    }
}