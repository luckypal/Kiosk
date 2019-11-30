﻿// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipStorer
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.Collections.Generic;
using System.Text;

namespace System.IO.Compression
{
  public class ZipStorer : IDisposable
  {
    public bool EncodeUTF8 = false;
    public bool ForceDeflating = false;
    private List<ZipStorer.ZipFileEntry> Files = new List<ZipStorer.ZipFileEntry>();
    private string Comment = "";
    private byte[] CentralDirImage = (byte[]) null;
    private ushort ExistingFiles = 0;
    private static uint[] CrcTable = (uint[]) null;
    private static Encoding DefaultEncoding = Encoding.GetEncoding(437);
    private string FileName;
    private Stream ZipFileStream;
    private FileAccess Access;

    static ZipStorer()
    {
      ZipStorer.CrcTable = new uint[256];
      for (int index1 = 0; index1 < ZipStorer.CrcTable.Length; ++index1)
      {
        uint num = (uint) index1;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((int) num & 1) != 0)
            num = 3988292384U ^ num >> 1;
          else
            num >>= 1;
        }
        ZipStorer.CrcTable[index1] = num;
      }
    }

    public static ZipStorer Create(string _filename, string _comment)
    {
      ZipStorer zipStorer = ZipStorer.Create((Stream) new FileStream(_filename, FileMode.Create, FileAccess.ReadWrite), _comment);
      zipStorer.Comment = _comment;
      zipStorer.FileName = _filename;
      return zipStorer;
    }

    public static ZipStorer Create(Stream _stream, string _comment)
    {
      return new ZipStorer()
      {
        Comment = _comment,
        ZipFileStream = _stream,
        Access = FileAccess.Write
      };
    }

    public static ZipStorer Open(string _filename, FileAccess _access)
    {
      ZipStorer zipStorer = ZipStorer.Open((Stream) new FileStream(_filename, FileMode.Open, _access == FileAccess.Read ? FileAccess.Read : FileAccess.ReadWrite), _access);
      zipStorer.FileName = _filename;
      return zipStorer;
    }

    public static ZipStorer Open(Stream _stream, FileAccess _access)
    {
      if (!_stream.CanSeek && _access != FileAccess.Read)
        throw new InvalidOperationException("Stream cannot seek");
      ZipStorer zipStorer = new ZipStorer();
      zipStorer.ZipFileStream = _stream;
      zipStorer.Access = _access;
      if (zipStorer.ReadFileInfo())
        return zipStorer;
      throw new InvalidDataException();
    }

    public void AddFile(
      ZipStorer.Compression _method,
      string _pathname,
      string _filenameInZip,
      string _comment)
    {
      if (this.Access == FileAccess.Read)
        throw new InvalidOperationException("Writing is not alowed");
      FileStream fileStream = new FileStream(_pathname, FileMode.Open, FileAccess.Read);
      this.AddStream(_method, _filenameInZip, (Stream) fileStream, File.GetLastWriteTime(_pathname), _comment);
      fileStream.Close();
    }

    public void AddStream(
      ZipStorer.Compression _method,
      string _filenameInZip,
      Stream _source,
      DateTime _modTime,
      string _comment)
    {
      if (this.Access == FileAccess.Read)
        throw new InvalidOperationException("Writing is not alowed");
      long num;
      if (this.Files.Count == 0)
      {
        num = 0L;
      }
      else
      {
        ZipStorer.ZipFileEntry file = this.Files[this.Files.Count - 1];
        num = (long) (file.HeaderOffset + file.HeaderSize);
      }
      ZipStorer.ZipFileEntry _zfe = new ZipStorer.ZipFileEntry();
      _zfe.Method = _method;
      _zfe.EncodeUTF8 = this.EncodeUTF8;
      _zfe.FilenameInZip = this.NormalizedFilename(_filenameInZip);
      _zfe.Comment = _comment == null ? "" : _comment;
      _zfe.Crc32 = 0U;
      _zfe.HeaderOffset = (uint) this.ZipFileStream.Position;
      _zfe.ModifyTime = _modTime;
      this.WriteLocalHeader(ref _zfe);
      _zfe.FileOffset = (uint) this.ZipFileStream.Position;
      this.Store(ref _zfe, _source);
      _source.Close();
      this.UpdateCrcAndSizes(ref _zfe);
      this.Files.Add(_zfe);
    }

    public void Close()
    {
      if (this.Access != FileAccess.Read)
      {
        uint position1 = (uint) this.ZipFileStream.Position;
        uint _size = 0;
        if (this.CentralDirImage != null)
          this.ZipFileStream.Write(this.CentralDirImage, 0, this.CentralDirImage.Length);
        for (int index = 0; index < this.Files.Count; ++index)
        {
          long position2 = this.ZipFileStream.Position;
          this.WriteCentralDirRecord(this.Files[index]);
          _size += (uint) (this.ZipFileStream.Position - position2);
        }
        if (this.CentralDirImage != null)
          this.WriteEndRecord(_size + (uint) this.CentralDirImage.Length, position1);
        else
          this.WriteEndRecord(_size, position1);
      }
      if (this.ZipFileStream == null)
        return;
      this.ZipFileStream.Flush();
      this.ZipFileStream.Dispose();
      this.ZipFileStream = (Stream) null;
    }

    public List<ZipStorer.ZipFileEntry> ReadCentralDir()
    {
      if (this.CentralDirImage == null)
        throw new InvalidOperationException("Central directory currently does not exist");
      List<ZipStorer.ZipFileEntry> zipFileEntryList = new List<ZipStorer.ZipFileEntry>();
      ushort uint16_1;
      ushort uint16_2;
      ushort uint16_3;
      for (int startIndex = 0; startIndex < this.CentralDirImage.Length && BitConverter.ToUInt32(this.CentralDirImage, startIndex) == 33639248U; startIndex += 46 + (int) uint16_1 + (int) uint16_2 + (int) uint16_3)
      {
        bool flag = ((int) BitConverter.ToUInt16(this.CentralDirImage, startIndex + 8) & 2048) != 0;
        ushort uint16_4 = BitConverter.ToUInt16(this.CentralDirImage, startIndex + 10);
        uint uint32_1 = BitConverter.ToUInt32(this.CentralDirImage, startIndex + 12);
        uint uint32_2 = BitConverter.ToUInt32(this.CentralDirImage, startIndex + 16);
        uint uint32_3 = BitConverter.ToUInt32(this.CentralDirImage, startIndex + 20);
        uint uint32_4 = BitConverter.ToUInt32(this.CentralDirImage, startIndex + 24);
        uint16_1 = BitConverter.ToUInt16(this.CentralDirImage, startIndex + 28);
        uint16_2 = BitConverter.ToUInt16(this.CentralDirImage, startIndex + 30);
        uint16_3 = BitConverter.ToUInt16(this.CentralDirImage, startIndex + 32);
        uint uint32_5 = BitConverter.ToUInt32(this.CentralDirImage, startIndex + 42);
        uint num = 46U + (uint) uint16_1 + (uint) uint16_2 + (uint) uint16_3;
        Encoding encoding = flag ? Encoding.UTF8 : ZipStorer.DefaultEncoding;
        ZipStorer.ZipFileEntry zipFileEntry = new ZipStorer.ZipFileEntry();
        zipFileEntry.Method = (ZipStorer.Compression) uint16_4;
        zipFileEntry.FilenameInZip = encoding.GetString(this.CentralDirImage, startIndex + 46, (int) uint16_1);
        zipFileEntry.FileOffset = this.GetFileOffset(uint32_5);
        zipFileEntry.FileSize = uint32_4;
        zipFileEntry.CompressedSize = uint32_3;
        zipFileEntry.HeaderOffset = uint32_5;
        zipFileEntry.HeaderSize = num;
        zipFileEntry.Crc32 = uint32_2;
        zipFileEntry.ModifyTime = this.DosTimeToDateTime(uint32_1);
        if (uint16_3 > (ushort) 0)
          zipFileEntry.Comment = encoding.GetString(this.CentralDirImage, startIndex + 46 + (int) uint16_1 + (int) uint16_2, (int) uint16_3);
        zipFileEntryList.Add(zipFileEntry);
      }
      return zipFileEntryList;
    }

    public bool ExtractFile(ZipStorer.ZipFileEntry _zfe, string _filename)
    {
      string directoryName = Path.GetDirectoryName(_filename);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      if (Directory.Exists(_filename))
        return true;
      Stream _stream = (Stream) new FileStream(_filename, FileMode.Create, FileAccess.Write);
      bool file = this.ExtractFile(_zfe, _stream);
      if (file)
        _stream.Close();
      File.SetCreationTime(_filename, _zfe.ModifyTime);
      File.SetLastWriteTime(_filename, _zfe.ModifyTime);
      return file;
    }

    public bool ExtractFile(ZipStorer.ZipFileEntry _zfe, Stream _stream)
    {
      if (!_stream.CanWrite)
        throw new InvalidOperationException("Stream cannot be written");
      byte[] buffer1 = new byte[4];
      this.ZipFileStream.Seek((long) _zfe.HeaderOffset, SeekOrigin.Begin);
      this.ZipFileStream.Read(buffer1, 0, 4);
      if (BitConverter.ToUInt32(buffer1, 0) != 67324752U)
        return false;
      Stream stream;
      if (_zfe.Method == ZipStorer.Compression.Store)
      {
        stream = this.ZipFileStream;
      }
      else
      {
        if (_zfe.Method != ZipStorer.Compression.Deflate)
          return false;
        stream = (Stream) new DeflateStream(this.ZipFileStream, CompressionMode.Decompress, true);
      }
      byte[] buffer2 = new byte[16384];
      this.ZipFileStream.Seek((long) _zfe.FileOffset, SeekOrigin.Begin);
      int count;
      for (uint fileSize = _zfe.FileSize; fileSize > 0U; fileSize -= (uint) count)
      {
        count = stream.Read(buffer2, 0, (int) Math.Min((long) fileSize, (long) buffer2.Length));
        _stream.Write(buffer2, 0, count);
      }
      _stream.Flush();
      if (_zfe.Method == ZipStorer.Compression.Deflate)
        stream.Dispose();
      return true;
    }

    public static bool RemoveEntries(ref ZipStorer _zip, List<ZipStorer.ZipFileEntry> _zfes)
    {
      if (!(_zip.ZipFileStream is FileStream))
        throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");
      List<ZipStorer.ZipFileEntry> zipFileEntryList = _zip.ReadCentralDir();
      string tempFileName1 = Path.GetTempFileName();
      string tempFileName2 = Path.GetTempFileName();
      try
      {
        ZipStorer zipStorer = ZipStorer.Create(tempFileName1, string.Empty);
        foreach (ZipStorer.ZipFileEntry _zfe in zipFileEntryList)
        {
          if (!_zfes.Contains(_zfe) && _zip.ExtractFile(_zfe, tempFileName2))
            zipStorer.AddFile(_zfe.Method, tempFileName2, _zfe.FilenameInZip, _zfe.Comment);
        }
        _zip.Close();
        zipStorer.Close();
        File.Delete(_zip.FileName);
        File.Move(tempFileName1, _zip.FileName);
        _zip = ZipStorer.Open(_zip.FileName, _zip.Access);
      }
      catch
      {
        return false;
      }
      finally
      {
        if (File.Exists(tempFileName1))
          File.Delete(tempFileName1);
        if (File.Exists(tempFileName2))
          File.Delete(tempFileName2);
      }
      return true;
    }

    private uint GetFileOffset(uint _headerOffset)
    {
      byte[] buffer = new byte[2];
      this.ZipFileStream.Seek((long) (_headerOffset + 26U), SeekOrigin.Begin);
      this.ZipFileStream.Read(buffer, 0, 2);
      ushort uint16_1 = BitConverter.ToUInt16(buffer, 0);
      this.ZipFileStream.Read(buffer, 0, 2);
      ushort uint16_2 = BitConverter.ToUInt16(buffer, 0);
      return (uint) ((ulong) (30 + (int) uint16_1 + (int) uint16_2) + (ulong) _headerOffset);
    }

    private void WriteLocalHeader(ref ZipStorer.ZipFileEntry _zfe)
    {
      long position = this.ZipFileStream.Position;
      byte[] bytes = (_zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding).GetBytes(_zfe.FilenameInZip);
      this.ZipFileStream.Write(new byte[6]
      {
        (byte) 80,
        (byte) 75,
        (byte) 3,
        (byte) 4,
        (byte) 20,
        (byte) 0
      }, 0, 6);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? (ushort) 2048 : (ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) _zfe.Method), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
      this.ZipFileStream.Write(new byte[12], 0, 12);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) bytes.Length), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 0), 0, 2);
      this.ZipFileStream.Write(bytes, 0, bytes.Length);
      _zfe.HeaderSize = (uint) (this.ZipFileStream.Position - position);
    }

    private void WriteCentralDirRecord(ZipStorer.ZipFileEntry _zfe)
    {
      Encoding encoding = _zfe.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding;
      byte[] bytes1 = encoding.GetBytes(_zfe.FilenameInZip);
      byte[] bytes2 = encoding.GetBytes(_zfe.Comment);
      this.ZipFileStream.Write(new byte[8]
      {
        (byte) 80,
        (byte) 75,
        (byte) 1,
        (byte) 2,
        (byte) 23,
        (byte) 11,
        (byte) 20,
        (byte) 0
      }, 0, 8);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.EncodeUTF8 ? (ushort) 2048 : (ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) _zfe.Method), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes(this.DateTimeToDosTime(_zfe.ModifyTime)), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) bytes1.Length), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) bytes2.Length), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 0), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) 33024), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.HeaderOffset), 0, 4);
      this.ZipFileStream.Write(bytes1, 0, bytes1.Length);
      this.ZipFileStream.Write(bytes2, 0, bytes2.Length);
    }

    private void WriteEndRecord(uint _size, uint _offset)
    {
      byte[] bytes = (this.EncodeUTF8 ? Encoding.UTF8 : ZipStorer.DefaultEncoding).GetBytes(this.Comment);
      this.ZipFileStream.Write(new byte[8]
      {
        (byte) 80,
        (byte) 75,
        (byte) 5,
        (byte) 6,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      }, 0, 8);
      this.ZipFileStream.Write(BitConverter.GetBytes((int) (ushort) this.Files.Count + (int) this.ExistingFiles), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes((int) (ushort) this.Files.Count + (int) this.ExistingFiles), 0, 2);
      this.ZipFileStream.Write(BitConverter.GetBytes(_size), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_offset), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) bytes.Length), 0, 2);
      this.ZipFileStream.Write(bytes, 0, bytes.Length);
    }

    private void Store(ref ZipStorer.ZipFileEntry _zfe, Stream _source)
    {
      byte[] buffer = new byte[16384];
      uint num = 0;
      long position1 = this.ZipFileStream.Position;
      long position2 = _source.Position;
      Stream stream = _zfe.Method != ZipStorer.Compression.Store ? (Stream) new DeflateStream(this.ZipFileStream, CompressionMode.Compress, true) : this.ZipFileStream;
      _zfe.Crc32 = uint.MaxValue;
      int count;
      do
      {
        count = _source.Read(buffer, 0, buffer.Length);
        num += (uint) count;
        if (count > 0)
        {
          stream.Write(buffer, 0, count);
          for (uint index = 0; (long) index < (long) count; ++index)
            _zfe.Crc32 = ZipStorer.CrcTable[(IntPtr) (uint) (((int) _zfe.Crc32 ^ (int) buffer[(IntPtr) index]) & (int) byte.MaxValue)] ^ _zfe.Crc32 >> 8;
        }
      }
      while (count == buffer.Length);
      stream.Flush();
      if (_zfe.Method == ZipStorer.Compression.Deflate)
        stream.Dispose();
      _zfe.Crc32 ^= uint.MaxValue;
      _zfe.FileSize = num;
      _zfe.CompressedSize = (uint) (this.ZipFileStream.Position - position1);
      if (_zfe.Method != ZipStorer.Compression.Deflate || this.ForceDeflating || !_source.CanSeek || _zfe.CompressedSize <= _zfe.FileSize)
        return;
      _zfe.Method = ZipStorer.Compression.Store;
      this.ZipFileStream.Position = position1;
      this.ZipFileStream.SetLength(position1);
      _source.Position = position2;
      this.Store(ref _zfe, _source);
    }

    private uint DateTimeToDosTime(DateTime _dt)
    {
      return (uint) (_dt.Second / 2 | _dt.Minute << 5 | _dt.Hour << 11 | _dt.Day << 16 | _dt.Month << 21 | _dt.Year - 1980 << 25);
    }

    private DateTime DosTimeToDateTime(uint _dt)
    {
      return new DateTime((int) (_dt >> 25) + 1980, (int) (_dt >> 21) & 15, (int) (_dt >> 16) & 31, (int) (_dt >> 11) & 31, (int) (_dt >> 5) & 63, ((int) _dt & 31) * 2);
    }

    private void UpdateCrcAndSizes(ref ZipStorer.ZipFileEntry _zfe)
    {
      long position = this.ZipFileStream.Position;
      this.ZipFileStream.Position = (long) (_zfe.HeaderOffset + 8U);
      this.ZipFileStream.Write(BitConverter.GetBytes((ushort) _zfe.Method), 0, 2);
      this.ZipFileStream.Position = (long) (_zfe.HeaderOffset + 14U);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.Crc32), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.CompressedSize), 0, 4);
      this.ZipFileStream.Write(BitConverter.GetBytes(_zfe.FileSize), 0, 4);
      this.ZipFileStream.Position = position;
    }

    private string NormalizedFilename(string _filename)
    {
      string str = _filename.Replace('\\', '/');
      int num = str.IndexOf(':');
      if (num >= 0)
        str = str.Remove(0, num + 1);
      return str.Trim('/');
    }

    private bool ReadFileInfo()
    {
      if (this.ZipFileStream.Length < 22L)
        return false;
      try
      {
        this.ZipFileStream.Seek(-17L, SeekOrigin.End);
        BinaryReader binaryReader = new BinaryReader(this.ZipFileStream);
        do
        {
          this.ZipFileStream.Seek(-5L, SeekOrigin.Current);
          if (binaryReader.ReadUInt32() == 101010256U)
          {
            this.ZipFileStream.Seek(6L, SeekOrigin.Current);
            ushort num1 = binaryReader.ReadUInt16();
            int count = binaryReader.ReadInt32();
            uint num2 = binaryReader.ReadUInt32();
            if (this.ZipFileStream.Position + (long) binaryReader.ReadUInt16() != this.ZipFileStream.Length)
              return false;
            this.ExistingFiles = num1;
            this.CentralDirImage = new byte[count];
            this.ZipFileStream.Seek((long) num2, SeekOrigin.Begin);
            this.ZipFileStream.Read(this.CentralDirImage, 0, count);
            this.ZipFileStream.Seek((long) num2, SeekOrigin.Begin);
            return true;
          }
        }
        while (this.ZipFileStream.Position > 0L);
      }
      catch
      {
      }
      return false;
    }

    public void Dispose()
    {
      this.Close();
    }

    public enum Compression : ushort
    {
      Store = 0,
      Deflate = 8,
    }

    public struct ZipFileEntry
    {
      public ZipStorer.Compression Method;
      public string FilenameInZip;
      public uint FileSize;
      public uint CompressedSize;
      public uint HeaderOffset;
      public uint FileOffset;
      public uint HeaderSize;
      public uint Crc32;
      public DateTime ModifyTime;
      public string Comment;
      public bool EncodeUTF8;

      public override string ToString()
      {
        return this.FilenameInZip;
      }
    }
  }
}
