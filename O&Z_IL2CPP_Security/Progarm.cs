﻿//Unity IL2CPP Version 24.5
using O_Z_IL2CPP_Security;
using System.Text;
//stringLiterals
List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();

//Test();
Console.WriteLine("OrangeIL2CPP");
Console.WriteLine("Loading Meatadata:" + args[0]);
byte[]? metadata_origin = null;

switch(args[1])
{
    case "Crypt":_Crypt();break;
    case "Decrypt":return;
    case "Read":_Read();break;
    case "Test":_Test();break;
    default:_default();break;
}
return;
void _Crypt()
{
    if (!File.Exists(args[0]))
    {
        Console.WriteLine("File is not EXISTS!");
        return;
    }
    metadata_origin = File.ReadAllBytes(args[0]);
    if (!CheckMetadataFile()) return;
    Metadata metadata = new Metadata(new MemoryStream(metadata_origin));
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes);
    byte[] allstring = metadata.GetAllStringFromMeta();

    Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, CryptB(allstring));
    byte[] tmp = Tools.StreamToBytes(stream);
    File.WriteAllBytes(args[2], Crypt.CryptMetadataBody(tmp)); //对整体进行加密
    return;
}
void _default()
{
    Console.WriteLine("parameter ERROR!");
    return;
}
void _Read()
{
    if (!File.Exists(args[0]))
    {
        Console.WriteLine("File is not EXISTS!");
        return;
    }
    metadata_origin = File.ReadAllBytes(args[0]);
    Metadata metadata = new Metadata(new MemoryStream(metadata_origin));
    MetadataHeader header = metadata.GetHeader();
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Console.WriteLine("[StringLitera]Count:"+ StringLiteraBytes.Count+"Baseoffset:"+header.stringLiteralDataOffset);
    for (int i = 0;i<StringLiteraBytes.Count; i++)
    {
        Console.WriteLine("[" +  i + "]" + Encoding.Default.GetString(StringLiteraBytes[i]));
    }
    Console.WriteLine(Encoding.UTF8.GetString(allstring));

    return;
}
bool CheckMetadataFile()
{
    if (BitConverter.ToUInt32(metadata_origin, 0) != 4205910959)
    {
        return false;
    }
    else
        return true;
}
void _Test()
{
    byte[] dat = File.ReadAllBytes(args[0]);
    byte[] dat2 = AssetBundleCrypt.CryptAssetBundle(dat);
    File.WriteAllBytes(args[2],dat2);
}
byte[] decrypt(byte[] b)
{
    byte[] result = new byte[b.Length];
    for (int i = 0; i < b.Length; i++)
    {
        if (b[i] != 0x52)
            result[i] = (byte)(b[i] ^ 114514);
        else
            result[i] = b[i];
    }
    return result;
}
byte[] CryptB(byte[] b)
{
    byte[] result = new byte[b.Length];
    for (int i = 0; i < b.Length; i++)
    {
        if (b[i] != 0 && b[i] != 0x52)
            result[i] = (byte)(b[i] ^ 114514);
        else
            result[i] = b[i];
    }
    return result;
}
