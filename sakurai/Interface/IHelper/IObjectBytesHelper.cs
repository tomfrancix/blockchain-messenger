using System;

namespace sakurai.Interface.IHelper
{
    public interface IObjectBytesHelper
    {
        byte[] ObjectToByteArray(Object obj);
        Object ByteArrayToObject(byte[] arrBytes);
    }
}
