using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{

    /// <summary>
    /// Code Port of n64js by Paul Holden
    /// Project GitHub: https://github.com/hulkholden/n64js/
    /// </summary>
    public static class N64Disassembler
    {
        public static string DisassembleInstruction(uint address, uint opcode)
        {
            var i = new Instruction(address, opcode);
            var disassembly = simpleTable[_op(opcode)](i);

            return disassembly;
        }

        public static uint _fd(uint i) { return (i >> 6) & 0x1f; }
        public static uint _fs(uint i) { return (i >> 11) & 0x1f; }
        public static uint _ft(uint i) { return (i >> 16) & 0x1f; }
        public static uint _copop(uint i) { return (i >> 21) & 0x1f; }

        public static uint _offset(uint i) { return (i) & 0xffff; }
        public static uint _sa(uint i) { return (i >> 6) & 0x1f; }
        public static uint _rd(uint i) { return (i >> 11) & 0x1f; }
        public static uint _rt(uint i) { return (i >> 16) & 0x1f; }
        public static uint _rs(uint i) { return (i >> 21) & 0x1f; }
        public static uint _op(uint i) { return (i >> 26) & 0x3f; }

        public static uint _tlbop(uint i) { return i & 0x3f; }
        public static uint _cop1_func(uint i) { return i & 0x3f; }
        public static uint _cop1_bc(uint i) { return (i >> 16) & 0x3; }

        public static uint _target(uint i) { return (i) & 0x3ffffff; }
        public static uint _imm(uint i) { return (i) & 0xffff; }
        public static short _imms(uint i) { return (short)_imm(i); }   // treat immediate value as signed
        public static uint _base(uint i) { return (i >> 21) & 0x1f; }


        private static uint _branchAddress(uint a, uint i) { return (uint)((a + 4) + _imms(i) * 4); }
        private static uint _jumpAddress(uint a, uint i) { return (a & 0xf0000000) | (_target(i) * 4); }

        static string[] gprRegisterNames = {
            "r0", "at", "v0", "v1", "a0", "a1", "a2", "a3",
            "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7",
            "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7",
            "t8", "t9", "k0", "k1", "gp", "sp", "s8", "ra"
        };

        static string[] cop0ControlRegisterNames = {
            "Index", "Rand", "EntryLo0", "EntryLo1", "Context", "PageMask", "Wired", "?7",
            "BadVAddr", "Count", "EntryHi", "Compare", "SR", "Cause", "EPC", "PrID",
            "?16", "?17", "WatchLo", "WatchHi", "?20", "?21", "?22", "?23",
            "?24", "?25", "ECC", "CacheErr", "TagLo", "TagHi", "ErrorEPC", "?31"
        };

        static string[] cop1RegisterNames = {
            "f00", "f01", "f02", "f03", "f04", "f05", "f06", "f07",
            "f08", "f09", "f10", "f11", "f12", "f13", "f14", "f15",
            "f16", "f17", "f18", "f19", "f20", "f21", "f22", "f23",
            "f24", "f25", "f26", "f27", "f28", "f29", "f30", "f31"
        };

        static string[] cop2RegisterNames = {
            "GR00", "GR01", "GR02", "GR03", "GR04", "GR05", "GR06", "GR07",
            "GR08", "GR09", "GR10", "GR11", "GR12", "GR13", "GR14", "GR15",
            "GR16", "GR17", "GR18", "GR19", "GR20", "GR21", "GR22", "GR23",
            "GR24", "GR25", "GR26", "GR27", "GR28", "GR29", "GR30", "GR31"
        };

        static string makeLabelText(uint address)
        {
            var text = toHex(address, 32);
            return "<span class='dis-address-jump'>" + text + "</span>";
        }

        static string makeRegSpan(string t)
        {
            return "<span class='dis-reg-" + t + "'>" + t + "</span>";
        }
        static string makeFPRegSpan(string t)
        {
            // We only use the "-" as a valic css identifier, but want to use "." in the visible text
            var text = t.Replace("-", ".");
            return "<span class='dis-reg-" + t + "'>" + text + "</span>";
        }


        class Instruction
        {
            public uint address;
            public uint opcode;
            public uint[] srcRegs = new uint[64];
            public uint[] dstRegs = new uint[64];
            public uint target;
            public string mode = "";
            public short offset;
            public uint register;

            public Instruction(uint add, uint op)
            {
                address = add;
                opcode = op;
            }

            // cop0 regs
            public string rt_d() { var reg = gprRegisterNames[_rt(this.opcode)]; return makeRegSpan(reg); }
            public string rd() { var reg = gprRegisterNames[_rd(this.opcode)]; return makeRegSpan(reg); }
            public string rt() { var reg = gprRegisterNames[_rt(this.opcode)]; return makeRegSpan(reg); }
            public string rs() { var reg = gprRegisterNames[_rs(this.opcode)]; return makeRegSpan(reg); }

            // dummy operand - just marks ra as being a dest reg
            public string writesRA() {  return ""; }

            // cop1 regs
            public string ft_d(string fmt) { var reg = getCop1RegisterName(_ft(this.opcode), fmt); return makeFPRegSpan(reg); }
            public string fs_d(string fmt) { var reg = getCop1RegisterName(_fs(this.opcode), fmt); return makeFPRegSpan(reg); }
            public string fd(string fmt) { var reg = getCop1RegisterName(_fd(this.opcode), fmt); return makeFPRegSpan(reg); }
            public string ft(string fmt) { var reg = getCop1RegisterName(_ft(this.opcode), fmt); return makeFPRegSpan(reg); }
            public string fs(string fmt) { var reg = getCop1RegisterName(_fs(this.opcode), fmt); return makeFPRegSpan(reg); }

            // cop2 regs
            public string gt_d() { var reg = cop2RegisterNames[_rt(this.opcode)]; return makeRegSpan(reg); }
            public string gd() { var reg = cop2RegisterNames[_rd(this.opcode)]; return makeRegSpan(reg); }
            public string gt() { var reg = cop2RegisterNames[_rt(this.opcode)]; return makeRegSpan(reg); }
            public string gs() { var reg = cop2RegisterNames[_rs(this.opcode)]; return makeRegSpan(reg); }
                   
            public string imm() { return toHex(_imm(this.opcode), 16); }
            public string immwd() { return toHex(_imm(this.opcode), 16) + " (" + _imms(this.opcode) + ")"; }

            public string branchAddress() { this.target = _branchAddress(this.address, this.opcode); return makeLabelText(this.target); }
            public string jumpAddress() { this.target = _jumpAddress(this.address, this.opcode); return makeLabelText(this.target); }

            public string memaccess(string _mode)
            {
                var r = this.rs();
                var off = this.imm();
                register = _rs(this.opcode);
                offset = _imms(this.opcode);
                this.mode = _mode;
                return "[" + r + "+" + off + "]";
            }

            public string memload()
            {
                return this.memaccess("load");
            }

            public string memstore()
            {
                return this.memaccess("store");
            }

        }

        static string toHex(uint r, uint bits)
        {

            var t = r.ToString("X");

            if (bits != 0)
            {
                var len = bits >> 2; // 4 bits per hex char
                while (t.Length < len)
                {
                    t = '0' + t;
                }
            }

            return "0x" + t;
        }

        private static string getCop1RegisterName(uint r, string fmt)
        {
            var suffix = (fmt != "") ? "-" + fmt : "";
            return cop1RegisterNames[r] + suffix;
        }

        static Func<Instruction, string>[] specialTable =
        {
            (i) => {
                if (i.opcode == 0)
                    return "NOP";
                return "SLL       " + i.rd() + " = " + i.rt() + " << "  + _sa(i.opcode);
            },
            (i) => { return "Unk"; },
            (i) =>
            { return "SRL       " + i.rd() + " = " + i.rt() + " >>> " + _sa(i.opcode); },
            (i) =>
            { return "SRA       " + i.rd() + " = " + i.rt() + " >> " + _sa(i.opcode); },
            (i) =>
            { return "SLLV      " + i.rd() + " = " + i.rt() + " << " + i.rs(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "SRLV      " + i.rd() + " = " + i.rt() + " >>> " + i.rs(); },
            (i) =>
            { return "SRAV      " + i.rd() + " = " + i.rt() + " >> " + i.rs(); },
            (i) =>
            { return "JR        " + i.rs(); },
            (i) =>
            { return "JALR      " + i.rd() + ", " + i.rs(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "SYSCALL   " + toHex((i.opcode >> 6) & 0xfffff, 20); },
            (i) =>
            { return "BREAK     " + toHex((i.opcode >> 6) & 0xfffff, 20); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "SYNC"; },
            (i) =>
            { return "MFHI      " + i.rd() + " = MultHi"; },
            (i) =>
            { return "MTHI      MultHi = " + i.rs(); },
            (i) =>
            { return "MFLO      " + i.rd() + " = MultLo"; },
            (i) =>
            { return "MTLO      MultLo = " + i.rs(); },
            (i) =>
            { return "DSLLV     " + i.rd() + " = " + i.rt() + " << " + i.rs(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "DSRLV     " + i.rd() + " = " + i.rt() + " >>> " + i.rs(); },
            (i) =>
            { return "DSRAV     " + i.rd() + " = " + i.rt() + " >> " + i.rs(); },
            (i) =>
            { return "MULT      " + i.rs() + " * " + i.rt(); },
            (i) =>
            { return "MULTU     " + i.rs() + " * " + i.rt(); },
            (i) =>
            { return "DIV       " + i.rs() + " / " + i.rt(); },
            (i) =>
            { return "DIVU      " + i.rs() + " / " + i.rt(); },
            (i) =>
            { return "DMULT     " + i.rs() + " * " + i.rt(); },
            (i) =>
            { return "DMULTU    " + i.rs() + " * " + i.rt(); },
            (i) =>
            { return "DDIV      " + i.rs() + " / " + i.rt(); },
            (i) =>
            { return "DDIVU     " + i.rs() + " / " + i.rt(); },
            (i) =>
            { return "ADD       " + i.rd() + " = " + i.rs() + " + " + i.rt(); },
            (i) =>
            { return "ADDU      " + i.rd() + " = " + i.rs() + " + " + i.rt(); },
            (i) =>
            { return "SUB       " + i.rd() + " = " + i.rs() + " - " + i.rt(); },
            (i) =>
            { return "SUBU      " + i.rd() + " = " + i.rs() + " - " + i.rt(); },
            (i) =>
            { return "AND       " + i.rd() + " = " + i.rs() + " & " + i.rt(); },
            (i) =>
            {
                if (_rt(i.opcode) == 0)
                {
                    if (_rs(i.opcode) == 0)
                    {
                        return "CLEAR     " + i.rd() + " = 0";
                    }
                    else
                    {
                        return "MOV       " + i.rd() + " = " + i.rs();
                    }
                }
                return "OR        " + i.rd() + " = " + i.rs() + " | " + i.rt();
            },
            (i) =>
            { return "XOR       " + i.rd() + " = " + i.rs() + " ^ " + i.rt(); },
            (i) =>
            { return "NOR       " + i.rd() + " = ~( " + i.rs() + " | " + i.rt() + " )"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "SLT       " + i.rd() + " = " + i.rs() + " < " + i.rt(); },
            (i) =>
            { return "SLTU      " + i.rd() + " = " + i.rs() + " < " + i.rt(); },
            (i) =>
            { return "DADD      " + i.rd() + " = " + i.rs() + " + " + i.rt(); },
            (i) =>
            { return "DADDU     " + i.rd() + " = " + i.rs() + " + " + i.rt(); },
            (i) =>
            { return "DSUB      " + i.rd() + " = " + i.rs() + " - " + i.rt(); },
            (i) =>
            { return "DSUBU     " + i.rd() + " = " + i.rs() + " - " + i.rt(); },
            (i) =>
            { return "TGE       trap( " + i.rs() + " >= " + i.rt() + " )"; },
            (i) =>
            { return "TGEU      trap( " + i.rs() + " >= " + i.rt() + " )"; },
            (i) =>
            { return "TLT       trap( " + i.rs() + " < " + i.rt() + " )"; },
            (i) =>
            { return "TLTU      trap( " + i.rs() + " < " + i.rt() + " )"; },
            (i) =>
            { return "TEQ       trap( " + i.rs() + " == " + i.rt() + " )"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "TNE       trap( " + i.rs() + " != " + i.rt() + " )"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "DSLL      " + i.rd() + " = " + i.rt() + " << " + _sa(i.opcode); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "DSRL      " + i.rd() + " = " + i.rt() + " >>> " + _sa(i.opcode); },
            (i) =>
            { return "DSRA      " + i.rd() + " = " + i.rt() + " >> " + _sa(i.opcode); },
            (i) =>
            { return "DSLL32    " + i.rd() + " = " + i.rt() + " << (32+" + _sa(i.opcode) + ")"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "DSRL32    " + i.rd() + " = " + i.rt() + " >>> (32+" + _sa(i.opcode) + ")"; },
            (i) =>
            { return "DSRA32    " + i.rd() + " = " + i.rt() + " >> (32+" + _sa(i.opcode) + ")"; }
        };

        private static string disassembleSpecial(Instruction i)
        {
            var fn = i.opcode & 0x3f;
            return specialTable[fn](i);
        }


        static Func<Instruction, string>[] cop0Table = {
            (i) => { return "MFC0      " + i.rt() + " <- " + cop0ControlRegisterNames[_fs(i.opcode)]; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "MTC0      " + i.rt() + " -> " + cop0ControlRegisterNames[_fs(i.opcode)]; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            disassembleTLB,
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
            (i) => { return "Unk"; },
        };

        private static string disassembleCop0(Instruction i)
        {
            var fmt = (i.opcode >> 21) & 0x1f;
            return cop0Table[fmt](i);
        }

        static string disassembleBCInstr(Instruction i)
        {
            if (((i.opcode >> 18) & 0x7) != 0)
                return "???";

            switch (_cop1_bc(i.opcode))
            {
                case 0: return "BC1F      !c ? --> " + i.branchAddress();
                case 1: return "BC1T      c ? --> " + i.branchAddress();
                case 2: return "BC1FL     !c ? --> " + i.branchAddress();
                case 3: return "BC1TL     c ? --> " + i.branchAddress();
            }

            return "???";
        }

        static string disassembleCop1Instr(Instruction i, string fmt)
        {
            var fmt_u = fmt;

            switch (_cop1_func(i.opcode))
            {
                case 0x00: return "ADD." + fmt_u + "     " + i.fd(fmt) + " = " + i.fs(fmt) + " + " + i.ft(fmt);
                case 0x01: return "SUB." + fmt_u + "     " + i.fd(fmt) + " = " + i.fs(fmt) + " - " + i.ft(fmt);
                case 0x02: return "MUL." + fmt_u + "     " + i.fd(fmt) + " = " + i.fs(fmt) + " * " + i.ft(fmt);
                case 0x03: return "DIV." + fmt_u + "     " + i.fd(fmt) + " = " + i.fs(fmt) + " / " + i.ft(fmt);
                case 0x04: return "SQRT." + fmt_u + "    " + i.fd(fmt) + " = sqrt(" + i.fs(fmt) + ")";
                case 0x05: return "ABS." + fmt_u + "     " + i.fd(fmt) + " = abs(" + i.fs(fmt) + ")";
                case 0x06: return "MOV." + fmt_u + "     " + i.fd(fmt) + " = " + i.fs(fmt);
                case 0x07: return "NEG." + fmt_u + "     " + i.fd(fmt) + " = -" + i.fs(fmt);
                case 0x08: return "ROUND.L." + fmt_u + " " + i.fd("l") + " = round.l(" + i.fs(fmt) + ")";
                case 0x09: return "TRUNC.L." + fmt_u + " " + i.fd("l") + " = trunc.l(" + i.fs(fmt) + ")";
                case 0x0a: return "CEIL.L." + fmt_u + "  " + i.fd("l") + " = ceil.l(" + i.fs(fmt) + ")";
                case 0x0b: return "FLOOR.L." + fmt_u + " " + i.fd("l") + " = floor.l(" + i.fs(fmt) + ")";
                case 0x0c: return "ROUND.W." + fmt_u + " " + i.fd("w") + " = round.w(" + i.fs(fmt) + ")";
                case 0x0d: return "TRUNC.W." + fmt_u + " " + i.fd("w") + " = trunc.w(" + i.fs(fmt) + ")";
                case 0x0e: return "CEIL.W." + fmt_u + "  " + i.fd("w") + " = ceil.w(" + i.fs(fmt) + ")";
                case 0x0f: return "FLOOR.W." + fmt_u + " " + i.fd("w") + " = floor.w(" + i.fs(fmt) + ")";

                case 0x20: return "CVT.S." + fmt_u + "   " + i.fd("s") + " = (s)" + i.fs(fmt);
                case 0x21: return "CVT.D." + fmt_u + "   " + i.fd("d") + " = (d)" + i.fs(fmt);
                case 0x24: return "CVT.W." + fmt_u + "   " + i.fd("w") + " = (w)" + i.fs(fmt);
                case 0x25: return "CVT.L." + fmt_u + "   " + i.fd("l") + " = (l)" + i.fs(fmt);

                case 0x30: return "C.F." + fmt_u + "     c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x31: return "C.UN." + fmt_u + "    c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x32: return "C.EQ." + fmt_u + "    c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x33: return "C.UEQ." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x34: return "C.OLT." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x35: return "C.ULT." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x36: return "C.OLE." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x37: return "C.ULE." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x38: return "C.SF." + fmt_u + "    c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x39: return "C.NGLE." + fmt_u + "  c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3a: return "C.SEQ." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3b: return "C.NGL." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3c: return "C.LT." + fmt_u + "    c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3d: return "C.NGE." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3e: return "C.LE." + fmt_u + "    c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
                case 0x3f: return "C.NGT." + fmt_u + "   c = " + i.fs(fmt) + " cmp " + i.ft(fmt);
            }

            return "Cop1." + fmt + toHex(_cop1_func(i.opcode), 8) + "?";
        }
        static string disassembleCop1SInstr(Instruction i)
        {
            return disassembleCop1Instr(i, "s");
        }
        static string disassembleCop1DInstr(Instruction i)
        {
            return disassembleCop1Instr(i, "d");
        }
        static string disassembleCop1WInstr(Instruction i)
        {
            return disassembleCop1Instr(i, "w");
        }
        static string disassembleCop1LInstr(Instruction i)
        {
            return disassembleCop1Instr(i, "l");
        }


        static Func<Instruction, string>[] cop1Table = {
          (i) => { return "MFC1      " + i.rt_d() + " = " + i.fs("");
            },
            (i) =>
            { return "DMFC1     " + i.rt_d() + " = " + i.fs(""); },
            (i) =>
            { return "CFC1      " + i.rt_d() + " = CCR" + _rd(i.opcode); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "MTC1      " + i.fs_d("") + " = " + i.rt(); },
            (i) =>
            { return "DMTC1     " + i.fs_d("") + " = " + i.rt(); },
            (i) =>
            { return "CTC1      CCR" + _rd(i.opcode) + " = " + i.rt(); },
            (i) =>
            { return "Unk"; },
            disassembleBCInstr,
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },

            disassembleCop1SInstr,
            disassembleCop1DInstr,
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            disassembleCop1WInstr,
            disassembleCop1LInstr,
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; }
        };

        static string disassembleCop1(Instruction i)
        {
            var fmt = (i.opcode >> 21) & 0x1f;
            return cop1Table[fmt](i);
        }


        static string disassembleTLB(Instruction i)
        {
            switch (_tlbop(i.opcode))
            {
                case 0x01: return "TLBR";
                case 0x02: return "TLBWI";
                case 0x06: return "TLBWR";
                case 0x08: return "TLBP";
                case 0x18: return "ERET";
            }

            return "Unk";
        }

        static Func<Instruction, string>[] regImmTable = {
            (i) => { return "BLTZ      " + i.rs() + " < 0 --> " + i.branchAddress();
            },
            (i) =>
            { return "BGEZ      " + i.rs() + " >= 0 --> " + i.branchAddress(); },
            (i) =>
            { return "BLTZL     " + i.rs() + " < 0 --> " + i.branchAddress(); },
            (i) =>
            { return "BGEZL     " + i.rs() + " >= 0 --> " + i.branchAddress(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },

            (i) =>
            { return "TGEI      " + i.rs() + " >= " + i.rt() + " --> trap "; },
            (i) =>
            { return "TGEIU     " + i.rs() + " >= " + i.rt() + " --> trap "; },
            (i) =>
            { return "TLTI      " + i.rs() + " < " + i.rt() + " --> trap "; },
            (i) =>
            { return "TLTIU     " + i.rs() + " < " + i.rt() + " --> trap "; },
            (i) =>
            { return "TEQI      " + i.rs() + " == " + i.rt() + " --> trap "; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "TNEI      " + i.rs() + " != " + i.rt() + " --> trap "; },
            (i) =>
            { return "Unk"; },

            (i) =>
            { return "BLTZAL    " + i.rs() + " < 0 --> " + i.branchAddress() + i.writesRA(); },
            (i) =>
            { return "BGEZAL    " + i.rs() + " >= 0 --> " + i.branchAddress() + i.writesRA(); },
            (i) =>
            { return "BLTZALL   " + i.rs() + " < 0 --> " + i.branchAddress() + i.writesRA(); },
            (i) =>
            { return "BGEZALL   " + i.rs() + " >= 0 --> " + i.branchAddress() + i.writesRA(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; }
        };


        static string disassembleRegImm(Instruction i)
        {
            var rt = (i.opcode >> 16) & 0x1f;
            return regImmTable[rt](i);
        }

        static Func<Instruction, string>[] simpleTable = {

          disassembleSpecial,
          disassembleRegImm,
          (i) => { return "J         --> " + i.jumpAddress();
            },
            (i) =>
            { return "JAL       --> " + i.jumpAddress() + i.writesRA(); },
            (i) =>
            {
                if (_rs(i.opcode) == _rt(i.opcode))
                {
                    return "B         --> " + i.branchAddress();
                }
                return "BEQ       " + i.rs() + " == " + i.rt() + " --> " + i.branchAddress();
            },
            (i) =>
            { return "BNE       " + i.rs() + " != " + i.rt() + " --> " + i.branchAddress(); },
            (i) =>
            { return "BLEZ      " + i.rs() + " <= 0 --> " + i.branchAddress(); },
            (i) =>
            { return "BGTZ      " + i.rs() + " > 0 --> " + i.branchAddress(); },
            (i) =>
            { return "ADDI      " + i.rt_d() + " = " + i.rs() + " + " + i.immwd(); },
            (i) =>
            { return "ADDIU     " + i.rt_d() + " = " + i.rs() + " + " + i.immwd(); },
            (i) =>
            { return "SLTI      " + i.rt_d() + " = (" + i.rs() + " < " + i.imm() + ")"; },
            (i) =>
            { return "SLTIU     " + i.rt_d() + " = (" + i.rs() + " < " + i.imm() + ")"; },
            (i) =>
            { return "ANDI      " + i.rt_d() + " = " + i.rs() + " & " + i.imm(); },
            (i) =>
            { return "ORI       " + i.rt_d() + " = " + i.rs() + " | " + i.imm(); },
            (i) =>
            { return "XORI      " + i.rt_d() + " = " + i.rs() + " ^ " + i.imm(); },
            (i) =>
            { return "LUI       " + i.rt_d() + " = " + i.imm() + " << 16"; },
            disassembleCop0,
            disassembleCop1,
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "BEQL      " + i.rs() + " == " + i.rt() + " --> " + i.branchAddress(); },
            (i) =>
            { return "BNEL      " + i.rs() + " != " + i.rt() + " --> " + i.branchAddress(); },
            (i) =>
            { return "BLEZL     " + i.rs() + " <= 0 --> " + i.branchAddress(); },
            (i) =>
            { return "BGTZL     " + i.rs() + " > 0 --> " + i.branchAddress(); },
            (i) =>
            { return "DADDI     " + i.rt_d() + " = " + i.rs() + " + " + i.imm(); },
            (i) =>
            { return "DADDIU    " + i.rt_d() + " = " + i.rs() + " + " + i.imm(); },
            (i) =>
            { return "LDL       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LDR       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "LB        " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LH        " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LWL       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LW        " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LBU       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LHU       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LWR       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LWU       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "SB        " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SH        " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SWL       " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SW        " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SDL       " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SDR       " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SWR       " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "CACHE     " + toHex(_rt(i.opcode), 8) + ", " + i.memaccess(""); },
            (i) =>
            { return "LL        " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LWC1      " + i.ft_d("") + " <- " + i.memload(); },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "LLD       " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LDC1      " + i.ft_d("") + " <- " + i.memload(); },
            (i) =>
            { return "LDC2      " + i.gt_d() + " <- " + i.memload(); },
            (i) =>
            { return "LD        " + i.rt_d() + " <- " + i.memload(); },
            (i) =>
            { return "SC        " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SWC1      " + i.ft("") + " -> " + i.memstore(); },
            (i) =>
            { return "BREAKPOINT"; },
            (i) =>
            { return "Unk"; },
            (i) =>
            { return "SCD       " + i.rt() + " -> " + i.memstore(); },
            (i) =>
            { return "SDC1      " + i.ft("") + " -> " + i.memstore(); },
            (i) =>
            { return "SDC2      " + i.gt() + " -> " + i.memstore(); },
            (i) =>
            { return "SD        " + i.rt() + " -> " + i.memstore(); }
        };
    }
}
