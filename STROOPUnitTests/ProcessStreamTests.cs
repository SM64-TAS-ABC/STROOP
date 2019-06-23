using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using STROOPUnitTests.Mocks;

namespace STROOPUnitTests
{
    [TestClass]
    public class ProcessStreamTests
    {
        [TestInitialize]
        public void TestInit()
        {
            Config.RamSize = 0x800000;
        }

        public void BasicTest<T>(IEnumerable<(uint, T, byte[])> valuesToWrite, ProcessStream processStream, Func<uint, int, bool, T> valueReader)
        {
            MockEmuIO io = new MockEmuIO(EndiannessType.Little);
            processStream.SwitchIO(io);
            processStream.Suspend();

            // Loop over each endianess type
            foreach (EndiannessType endianness in new List<EndiannessType>() { EndiannessType.Big, EndiannessType.Little })
            {
                io.SetEndianness(endianness);

                // Loop over each absolute/relative type
                foreach (bool absolute in new List<bool>() { true, false }) {
                    // Clear the IO
                    io.Clear();

                    string message(uint relativeAddress) => $"Failed to match at address {relativeAddress:X4}, Endianess: {endianness}, Absolute Addressing: {absolute}";

                    uint addressFromRelativeAddress(uint relativeAddress, int dataSize)
                    {
                        uint address = absolute ? relativeAddress + MockEmuIO.Offset : relativeAddress;
                        if (absolute)
                            address = EndiannessUtilities.SwapAddressEndianness(address, dataSize);
                        return address;
                    }

                    uint emuAddressFromRelativeAddress(uint relativeAddress, int dataSize)
                    {
                        uint address = relativeAddress;
                        if (endianness == EndiannessType.Little)
                            address = EndiannessUtilities.SwapAddressEndianness(address, dataSize);
                        return address;
                    }

                    // Write all values to the IO
                    foreach ((uint relativeAddress, T value, byte[] byteValue) in valuesToWrite)
                    {
                        uint address = addressFromRelativeAddress(relativeAddress, byteValue.Length);
                        uint emuAddress = emuAddressFromRelativeAddress(relativeAddress, byteValue.Length);

                        EndiannessType dataEndianess = (typeof(T) == typeof(byte[])) ? EndiannessType.Big : EndiannessType.Little; 
                        byte[] expectedBytes = byteValue;
                        if (endianness != dataEndianess)
                            expectedBytes = EndiannessUtilities.SwapByteEndianness(byteValue);

                        if (typeof(T) == typeof(byte[]))
                            processStream.WriteRam(byteValue, new UIntPtr(address), EndiannessType.Big, absolute);
                        else
                            processStream.SetValue(typeof(T), value, address, absolute);
                        byte[] actualBytes = io.GetBytes(emuAddress, byteValue.Length);
                        CollectionAssert.AreEqual(expectedBytes, actualBytes, message(relativeAddress));
                    }

                    // Refresh IO so the buffer can be used
                    processStream.RefreshRam();
                    
                    // Validate values read are values written
                    foreach ((uint relativeAddress, T value, byte[] bytes) in valuesToWrite)
                    {
                        uint address = addressFromRelativeAddress(relativeAddress, bytes.Length);

                        T actualValue = valueReader(address, bytes.Length, absolute);
                        if (typeof(T) == typeof(byte[]))
                            CollectionAssert.AreEqual(value as byte[], actualValue as byte[], message(relativeAddress));
                        else
                            Assert.AreEqual(value, actualValue, message(relativeAddress));
                    }
                }
            }
        }

        [TestMethod]
        public void Test_ProcessStream_ReadWrite_U8()
        {
            ProcessStream processStream = new ProcessStream();
            var values = new List<(uint, byte)>()
            {
                (0, 1),
                (1, 2),
                (2, 3),
                (3, 4),
            };

            var valuesWithBytes = values.Select(s => (s.Item1, s.Item2, new byte[] { s.Item2 })).ToList();

            BasicTest<byte>(valuesWithBytes, processStream, (address, _, absolute) => processStream.GetByte(address, absolute));
        }

        [TestMethod]
        public void Test_ProcessStream_ReadWrite_U16()
        {
            ProcessStream processStream = new ProcessStream();
            var values = new List<(uint, UInt16)>()
            {
                (0, 0x0102),
                (2, 0x0304),
                (5, 0x0506),
            };

            var valuesWithBytes = values.Select(s => (s.Item1, s.Item2, BitConverter.GetBytes(s.Item2))).ToList();

            BasicTest<UInt16>(valuesWithBytes, processStream, (address, _, absolute) => processStream.GetUInt16(address, absolute));
        }

        [TestMethod]
        public void Test_ProcessStream_ReadWrite_U32()
        {
            ProcessStream processStream = new ProcessStream();
            var values = new List<(uint, UInt32)>()
            {
                (0, 0x01020304),
                (4, 0x05060708),
            };

            var valuesWithBytes = values.Select(s => (s.Item1, s.Item2, BitConverter.GetBytes(s.Item2))).ToList();

           BasicTest<UInt32>(valuesWithBytes, processStream, (address, _, absolute) => processStream.GetUInt32(address, absolute));
        }

        [TestMethod]
        public void Test_ProcessStream_ReadWrite_data()
        {
            ProcessStream processStream = new ProcessStream();
            var values = new List<(uint, byte[])>()
            {
                (0, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 }),
                (8, new byte[] { 0x10, 0x11, 0x12, 0x13 }),
                (12, new byte[] { 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B }),
            };

            var valuesWithBytes = values.Select(s => (s.Item1, s.Item2, s.Item2)).ToList();

            BasicTest<byte[]>(valuesWithBytes, processStream, (address, length, absolute) => processStream.ReadRam(new UIntPtr(address), length, EndiannessType.Big, absolute));
        }
    }
}
