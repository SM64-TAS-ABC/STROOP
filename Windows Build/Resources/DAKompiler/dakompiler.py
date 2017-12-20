from __future__ import print_function
import struct

import instruction
from ramMapLoader import arguments as parseArgument
from symbolify import makeSymbolic, extend
from ramMapLoader import loadBindings
from pythonForm import renderFunctionToPython as render
#from Cform import renderFunctionToC as render

def chunks(seq, n):
    for lineNum in range(0, len(seq), n):
        yield seq[lineNum:lineNum + n]

class RAMSnapshot:
    def __init__(self, filename, startAddress, length = 0):
        with open(filename, 'rb') as f:
            self.data = f.read(length) if length > 0 else f.read()
        self.start = startAddress
        self.end = startAddress + len(self.data)

def findFunction(start, ram):
    """Produces the disassembly for the function starting at the given address"""
    loops = {}
    if start < ram.start or start >= ram.end:
        raise Exception("function start address not found")  # function not in range
    current = start
    latest = start
    for word in chunks(ram.data[start - ram.start:], 4):
        if word == b'\x03\xe0\x00\x08' and latest <= current:  # JR RA
            if ram.end > current + 8:  # do we have both JR RA and delay slot?
                return start, disassembleBlock(ram.data[start - ram.start:current + 8 - ram.start]), loops
            else:
                return None
        instr = struct.unpack('>L', word)[0]
        if instruction.isBranch(instr):
            target = 4 + 4 * extend(instr & 0xffff)
            if target < current:
                loops[(target - start) // 4] = (current - start) // 4
            else:
                latest = max(target, latest)
                if latest >= ram.end:
                    return None
        current += 4
    return None  # function did not end

def disassembleBlock(data):
    """Converts binary data into MIPS"""
    return [instruction.Instruction(w) for w in struct.unpack('>' + 'L'*(len(data)//4), data)]

def decompileFunction(ram, bindings, address, name=None, args=[]):
    """"
        Produce Python code (roughly) equivalent to the specified function.

        bindings is a nested dictionary object produced by loadBindings
        args is a list of strings of form "regName argName argType" where argType can be an
            arbitrary type string as in the RAM map specification
    """
    data = findFunction(address, ram)
    if not data:
        print("function did not end in the given data")
    if not name:
        name = 'fn%06x' % address
    niceArgs = []
    if args:
        for spec in args:
            # DRY taken to a bit of an extreme, maybe
            parseArgument(spec.split(), niceArgs, bindings)
    elif address in bindings['functions']:
        niceArgs = bindings['functions'][address].args
    symbolified = makeSymbolic(name, data, bindings, niceArgs)
    return '\n'.join(render(name, *symbolified))

def showDisassembly(ram, address):
    """Prints the disassembly of the function at address in ram"""
    mips = findFunction(address, ram)[1]
    for i,x in enumerate(mips):
        print(hex(4*i)[2:].zfill(3),x)

def showMIPSblock(ram, address, length):
    """Prints the disassembly of an arbitrary block

        length is the number of instructions
    """
    mips = disassembleBlock(ram.data[address - ram.start:address - ram.start + 4 * length])
    for i,x in enumerate(mips):
        print(hex(4*i + start)[2:].zfill(3),x)

if __name__ == "__main__":
    b = loadBindings('sm64 ram map.txt', 'J')
    marioRam = RAMSnapshot('marioRam',0x80000000)
    print(decompileFunction(marioRam,b, 0x8026BFC8, args = ['A0 mario *Mario']))
