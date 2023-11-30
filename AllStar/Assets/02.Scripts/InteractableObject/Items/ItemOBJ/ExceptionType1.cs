using System;
using System.Runtime.Serialization;

[Serializable]
internal class ExceptionType1 : Exception
{
    public ExceptionType1()
    {
    }

    public ExceptionType1(string message) : base(message)
    {
    }

    public ExceptionType1(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ExceptionType1(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}