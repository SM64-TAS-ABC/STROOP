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

        public void BasicTest<T>(IEnumerable<(uint, T, byte[])> valuesToWrite, ProcessStream processStream, Func<uint, bool, T> valueReader)
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

                    uint addressFromRelativeAddress(uint relativeAddress)
                    {
                        uint address = absolute ? relativeAddress + MockEmuIO.Offset : relativeAddress;
                        int dataSize = TypeUtilities.TypeSize[typeof(T)];
                        if (absolute)
                            address = EndiannessUtilities.SwapAddressEndianness(address, dataSize);
                        return address;
                    }

                    uint emuAddressFromRelativeAddress(uint relativeAddress)
                    {
                        uint address = relativeAddress;
                        int dataSize = TypeUtilities.TypeSize[typeof(T)];
                        if (endianness == EndiannessType.Little)
                            address = EndiannessUtilities.SwapAddressEndianness(address, dataSize);
                        return address;
                    }

                    // Write all values to the IO
                    foreach ((uint relativeAddress, T value, byte[] byteValue) in valuesToWrite)
                    {
                        uint address = addressFromRelativeAddress(relativeAddress);
                        uint emuAddress = emuAddressFromRelativeAddress(relativeAddress);
                        byte[] expectedBytes = endianness == EndiannessType.Little ? byteValue : byteValue.Reverse().ToArray();

                        processStream.SetValue(typeof(T), value, address, absolute);

                        int dataSize = TypeUtilities.TypeSize[typeof(T)];
                        byte[] actualBytes = io.GetBytes(emuAddress, dataSize);
                        CollectionAssert.AreEqual(expectedBytes, actualBytes, message(relativeAddress));
                    }

                    // Refresh IO so the buffer can be used
                    processStream.RefreshRam();
                    
                    // Validate values read are values written
                    foreach ((uint relativeAddress, T value, _) in valuesToWrite)
                    {
                        uint address = addressFromRelativeAddress(relativeAddress);

                        T actualValue = valueReader(address, absolute);
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

            BasicTest<byte>(valuesWithBytes, processStream, (address, absolute) => processStream.GetByte(address, absolute));
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

            BasicTest<UInt16>(valuesWithBytes, processStream, (address, absolute) => processStream.GetUInt16(address, absolute));
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

            BasicTest<UInt32>(valuesWithBytes, processStream, (address, absolute) => processStream.GetUInt32(address, absolute));
        }
    }
}
