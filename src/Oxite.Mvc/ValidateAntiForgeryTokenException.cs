//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization;

namespace Oxite.Mvc
{
    public class ValidateAntiForgeryTokenException : Exception
    {
        public ValidateAntiForgeryTokenException()
            : base()
        {
        }

        public ValidateAntiForgeryTokenException(string message)
            : base(message)
        {
        }

        public ValidateAntiForgeryTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ValidateAntiForgeryTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}