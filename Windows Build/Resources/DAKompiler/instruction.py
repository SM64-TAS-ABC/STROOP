from enum import Enum
import basicTypes

class MainOp(Enum):
    J =     2
    JAL =   3
    BEQ =   4
    BNE =   5
    BLEZ =  6
    BGTZ =  7
    ADDI =  8
    ADDIU = 9
    SLTI =  10
    SLTIU = 11
    ANDI =  12
    ORI =   13
    XORI =  14
    LUI =   15
    BEQL =  20
    BNEL =  21
    BLEZL = 22
    BGTZL = 23
    LB =    32
    LH =    33
    LWL =   34
    LW =    35
    LBU =   36
    LHU =   37
    LWR =   38
    SB =    40
    SH =    41
    SWL =   42
    SW =    43
    SWR =   46
    CACHE = 47
    LL =    48
    LWC1 =  49
    LWC2 =  50
    PREF =  51
    LDC1 =  53
    LDC2 =  54
    SC =    56
    SWC1 =  57
    SWC2 =  58
    SDC1 =  61
    SDC2 =  62
        
class RegOp(Enum):
    SLL = 0
    SRL = 2
    SRA = 3
    SLLV = 4
    SRLV = 6
    SRAV = 7
    JR = 8
    JALR = 9
    MOVZ = 10
    MOVN = 11
    SYSCALL = 12
    BREAK = 13
    SYNC = 15
    MFHI = 16
    MTHI = 17
    MFLO = 18
    MTLO = 19
    MULT = 24
    MULTU = 25
    DIV = 26
    DIVU = 27
    ADD = 32
    ADDU = 33
    SUB = 34
    SUBU = 35
    AND = 36
    OR = 37
    XOR = 38
    NOR = 39
    SLT = 42
    SLTU = 43

class CopOp(Enum):
    MFC = 0
    CFC = 2
    MTC = 4
    CTC = 6
    BCF = 10
    BCT = 11
    BCFL = 12
    BCTL = 13
    
class FloatOp(Enum):
    ADD = 0
    SUB = 1
    MUL = 2
    DIV = 3
    SQRT = 4
    ABS = 5
    MOV = 6
    NEG = 7
    ROUND_W = 12
    TRUNC_W = 13
    CEIL_W = 14
    FLOOR_W = 15
    CVT_S = 32
    CVT_D = 33
    CVT_W = 36
    C_F = 48
    C_UN = 49
    C_EQ = 50
    C_UEQ = 51
    C_OLT = 52
    C_ULT = 53
    C_OLE = 54
    C_ULE = 55
    C_SF = 56
    C_NGLE = 57
    C_SEQ = 58
    C_NGL = 59
    C_LT = 60
    C_NGE = 61
    C_LE = 62
    C_NGT = 63
    
class SpecialOp(Enum):
    NOP = 0
    BLTZ = 10
    BGEZ = 11
    BLTZL = 12
    BGEZL = 13

class Register(Enum):
    R0, AT, V0, V1, A0, A1, A2, A3 = range(8)
    T0, T1, T2, T3, T4, T5, T6, T7 = range(8, 16)
    S0, S1, S2, S3, S4, S5, S6, S7 = range(16, 24)
    T8, T9, K0, K1, GP, SP, S8, RA = range(24, 32)

# I'm deeply, deeply sorry for this. I didn't want to require 3.5 just for "start",
# though I guess I'm requiring 3.4 just for enums 
class FloatRegister(Enum):
    exec(';'.join(('F%s = %s' % (i,i)) for i in range(32)))

SpecialRegister = Enum('SpecialRegister', 'Compare MultLo MultHi')
    
class Instruction:
    branchOPs = set([MainOp[x] for x in "BEQ BNE BLEZ BGTZ BEQL BNEL BLEZL BGTZL".split()] + [CopOp[x] for x in "BCF BCT BCFL BCTL".split()])
    J_format = set([MainOp.J,MainOp.JAL])
    I_format = set([CopOp.BCF,CopOp.BCT,CopOp.BCFL,CopOp.BCTL])
    D_format = set([RegOp.MFLO, RegOp.MFHI])
    R_format = set([RegOp.JALR,RegOp.JR,RegOp.MFHI,RegOp.MTHI,RegOp.MFLO,RegOp.MTLO])
    RI_format = set([MainOp.LUI, MainOp.BLEZL,MainOp.BGTZL])
    SI_format = set([MainOp.BLEZ, MainOp.BGTZ, SpecialOp.BLTZ,SpecialOp.BGEZ,SpecialOp.BLTZL,SpecialOp.BGEZL])
    RR_format = set([RegOp.MULT,RegOp.MULTU,RegOp.DIV,RegOp.DIVU])
    RRI_format = set([MainOp[x] for x in "BEQ BNE ADDI ADDIU SLTI SLTIU ANDI ORI XORI BEQL BNEL".split()])
    RRS_format = set([RegOp[x] for x in "SLL SRL SRA".split()])
    RIR_format = set([MainOp[x] for x in "LB LH LWL LW LBU LHU LWR SB SH SWL SW SWR".split()])
    RRR_format = set([RegOp[x] for x in "SLLV SRLV SRAV ADD ADDU SUB SUBU AND OR XOR NOR SLT SLTU".split()])
    FIR_format = set([MainOp[x] for x in "LWC1 LWC2 LDC1 LDC2 SWC1 SWC2 SDC1 SDC2".split()])
    FF_format = set([FloatOp[x] for x in "SQRT ABS MOV NEG ROUND_W TRUNC_W CEIL_W FLOOR_W CVT_S CVT_D CVT_W".split()])
    FsF_format = set([FloatOp[x] for x in "C_EQ C_LT C_LE".split()])
    FFF_format = set([FloatOp[x] for x in "ADD SUB MUL DIV".split()])
    RF_format = set([CopOp.MFC,CopOp.CFC,CopOp.MTC,CopOp.CTC])
    
    def __init__(self, word):
        self.raw = word
                                    #________********________********
        op = word >> 26             #111111..........................
        rs = (word>>21) & 0x1f      #......11111.....................
        rt = (word>>16) & 0x1f      #...........11111................
        rd = (word>>11) & 0x1f      #................11111...........
        imm = word & 0xffff         #................1111111111111111
        spec = word & 0x3f          #..........................111111
        try:
            self.opcode = MainOp(op)
        except ValueError:  #need further specification
            if op == 0:
                if word == 0:
                    self.opcode = SpecialOp.NOP
                    return
                else:
                    self.opcode = RegOp(spec)
            elif op == 1:
                self.opcode = SpecialOp(rt+10)
                self.sourceReg = Register(rs)
                self.immediate = imm
                return
            elif op in [16,17,18]:
                self.cop = op - 16
                if rs == 16:
                    if self.cop == 0:
                        raise Exception("cop 0 mostly unimplemented")
                    elif self.cop == 1:
                        self.fmt = basicTypes.single
                        self.opcode = FloatOp(spec)
                    else:
                        raise Exception("cop > 1 unimplemented")
                elif rs == 17 and self.cop == 1:
                    self.fmt = basicTypes.double
                    self.opcode = FloatOp(spec)
                elif rs == 20 and spec == 32:
                    self.fmt = basicTypes.word
                    self.opcode = FloatOp(spec)
                elif rs == 8:
                    self.opcode = CopOp(((word>>16) & 0x3)+10)
                    self.target = imm
                else:
                    self.opcode = CopOp(rs)
                    self.targetReg = Register(rt)
                    self.fs = FloatRegister(rd)
            else:
                raise Exception("op " + str(op) + " unimplemented",hex(word))
        if isinstance(self.opcode, FloatOp):
            self.ft = FloatRegister(rt)
            self.fs = FloatRegister(rd)
            self.fd = FloatRegister((word>>6) & 0x1f)
        elif self.opcode in [MainOp.J, MainOp.JAL]:
            self.target = 4*(word & 0x3ffffff)
        elif self.opcode in Instruction.FIR_format:
            self.sourceReg = Register(rs)
            self.targetReg = FloatRegister(rt)
            self.immediate = imm
        elif isinstance(self.opcode, MainOp):
            self.sourceReg = Register(rs)
            self.targetReg = Register(rt)
            self.immediate = imm
        elif self.opcode in [RegOp.SLL,RegOp.SRL,RegOp.SRA]:
            self.targetReg = Register(rt)
            self.destReg = Register(rd)
            self.shift = (word>>6) & 0x1f
        elif isinstance(self.opcode, RegOp) or isinstance(self.opcode, CopOp):
            self.sourceReg = Register(rs)
            self.targetReg = Register(rt)
            self.destReg = Register(rd)
        elif isinstance(self.opcode, SpecialOp):
            pass
        else:
            raise Exception(str(self.opcode) + " is uncategorized")
            
    def __repr__(self):
        return "Instruction(raw = %r, opcode = %r)" % (self.raw, self.opcode)
                
    def __str__(self):
        if self.opcode in Instruction.J_format:
            return '%s %#X' % (self.opcode.name, self.target)
        if self.opcode in Instruction.D_format:
            return '%s %s' % (self.opcode.name, self.destReg.name)
        if self.opcode in Instruction.R_format:     
            return '%s %s' % (self.opcode.name, self.sourceReg.name)
        if self.opcode in Instruction.I_format:
            return '%s%d %#X' % (self.opcode.name, self.cop, self.target)
        if self.opcode in Instruction.RI_format:
            return '%s %s, %#x' % (self.opcode.name, self.targetReg.name, self.immediate)
        if self.opcode in Instruction.SI_format:
            return '%s %s, %#x' % (self.opcode.name, self.sourceReg.name, self.immediate)
        if self.opcode in Instruction.RR_format:
            return '%s %s, %s' % (self.opcode.name, self.sourceReg.name, self.targetReg.name)
        if self.opcode in Instruction.RIR_format:
            return '%s %s, %#x (%s)' % (self.opcode.name, self.targetReg.name, self.immediate, self.sourceReg.name)
        if self.opcode in Instruction.RRI_format:   
            return '%s %s, %s, %#x' % (self.opcode.name, self.targetReg.name, self.sourceReg.name, self.immediate)
        if self.opcode in Instruction.RRR_format:
            return '%s %s, %s, %s' % (self.opcode.name, self.destReg.name, self.sourceReg.name, self.targetReg.name)
        if self.opcode in Instruction.RRS_format:
            return '%s %s, %s, %#x' % (self.opcode.name, self.destReg.name, self.targetReg.name, self.shift)
        if self.opcode in Instruction.FIR_format:
            return '%s %s, %#x (%s)' % (self.opcode.name, self.targetReg.name, self.immediate, self.sourceReg.name)
        if self.opcode in Instruction.FF_format:
            return '%s_%s %s, %s' % (self.opcode.name, self.fmt.name[0].upper(), self.fd.name, self.ft.name)
        if self.opcode in Instruction.FsF_format:
            return '%s_%s %s, %s' % (self.opcode.name, self.fmt.name[0].upper(), self.fs.name, self.ft.name)
        if self.opcode in Instruction.FFF_format:
            return '%s_S %s, %s, %s' % (self.opcode.name, self.fd.name, self.fs.name, self.ft.name)
        if self.opcode in Instruction.RF_format:
            return '%s%d %s, %s' % (self.opcode.name, self.cop, self.targetReg.name, self.fs.name)
        return self.opcode.name
        
def isBranch(instr):
    op = instr >> 26
    try:
        if MainOp(op) in Instruction.branchOPs:
            return True
    except:
        pass
    if op == 1:
        return ((instr >> 16) & 0x1f) in [0, 1, 2, 3]
    else:
        return op == 16 and (instr >> 21) & 0x1f == 8
