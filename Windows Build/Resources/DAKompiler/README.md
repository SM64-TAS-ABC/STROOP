# DAKompiler
a ~~simple~~ needlessly complicated mips decompiler 

## Basic usage
This decompiler doesn't target a particular version of MIPS, mostly because
I haven't implemented many opcodes that would have to differ by version. Nor does it try to accurately decompile arbitrary MIPS; I'm targeting code that was written by a non-very-aggressive compiler, which seems to be enough for most game logic in Super Mario 64.

Python >= 3.4 required, no plans to implement a command line interface mostly 
because I've found this to be a rather interactive process most of the time.

Load a 'bindings' object from a ram map file via
```
bindings = loadBindings('sm64 ram map.txt', 'J')
```
where the region code is required (function and global variable addresses will generally differ by region).
The included file is far from complete.

You'll also need binary data, either a dump of the game's RAM at some point or the ROM itself (although the latter may eventually cause problems because absolute addresses will be wrong)
```
marioRam = RAMSnapshot('marioRam', 0x80000000)
```
the number is the address the binary data starts at, so that later you can use absolute addresses.

Access the main functionality through
```
decompileFunction(marioRam, bindings, address = 0x8027D14C, args = ['A0 obj Object'])
```
where `address` is the absolute address your function of interest starts at, and `args` is a list of strings specifying which registers are used as arguments, the names that should be used, and their types, in a way I hope is clear. Other arguments DAKompiler discovers will be noted. 

With any luck, you'll see something along the lines of
```
def fn8027d14c(obj):
    if obj.b_0x18 == ubyte(word(0x8032cf90) + 0x14):
        if obj.transform:
            fn80379f60(A0 = (short(0x8033a770) << 6) + 0x8033a7b8, A1 = obj.transform, A2 = (short(0x8033a770) << 6) + 0x8033a778)
        else:
            if (obj.gfxFlags & 0x4) != 0:
                fn80379798(A0 = (short(0x8033a770) << 6) + 0x8033a7b8, A1 = (short(0x8033a770) << 6) + 0x8033a778, A2 = obj + 0x20, A3 = short(word(0x8032cf9c) + 0x38))
            else:
                fn80379440(A0 = SP + -0x40, A1 = obj + 0x20, A2 = obj + 0x1a)
                fn80379f60(A0 = (short(0x8033a770) << 6) + 0x8033a7b8, A1 = SP + -0x40, A2 = (short(0x8033a770) << 6) + 0x8033a778)
        fn8037a29c(A0 = (short(0x8033a770) << 6) + 0x8033a7b8, A1 = (short(0x8033a770) << 6) + 0x8033a7b8, A2 = obj + 0x2c)
        short(0x8033a770) = ((short(0x8033a770) + 1) << 0x10) >a 0x10
        obj.transform = ((((short(0x8033a770) + 0x1) << 0x10) >a 0x10) << 0x6) + 0x8033a778
        obj.posOffset.x = single((short(0x8033a770) << 0x6) + 0x8033a7a8)
        obj.posOffset.y = single((short(0x8033a770) << 0x6) + 0x8033a7ac)
        obj.posOffset.z = single((short(0x8033a770) << 0x6) + 0x8033a7b0)
        if obj.w_0x3c != 0:
            fn8027c988(A0 = obj + 0x38, A1 = (obj.gfxFlags & 0x20) < 0)
        returnValue_218 = fn8027cf68(A0 = obj, A1 = (short(0x8033a770) << 6) + 0x8033a778)
        if returnValue_218 != 0:
            returnValue_228 = fn8027897c(A0 = 0x40)
            fn8037a434(A0 = returnValue_228, A1 = (short(0x8033a770) << 6) + 0x8033a778)
            word((short(0x8033a770) << 0x2) + 0x8033af78) = returnValue_228
            if obj.w_0x14 != 0:
                word(0x8032cfa0) = obj
                word(obj.w_0x14 + 0xc) = obj
                fn8027d8f8(A0 = obj.w_0x14)
                word(obj.w_0x14 + 0xc) = 0
                word(0x8032cfa0) = 0
            if obj.gfxChild:
                fn8027d8f8(A0 = obj.gfxChild)
        short(0x8033a770) = short(0x8033a770) + -1
        byte(0x8033b008) = 0
        obj.transform = None
    return V0
```
which, y'know, could be *less* readable.

## Planned features/fixes

### Major TODO items
- [ ] Recognize loop types
- [ ] Deal with JR and address tables
- [ ] Be more careful about types (maybe support C-style output)
- [x] Deal with branching more intelligently, allow for `elif`
- [ ] Add 'likely' branches
- [ ] Add missing instructions (MULT, double precision math)
- [ ] Recognize flag and enum values

### Minor stuff
- [ ] Process branches better so merging doesn't take forever
- [x] Simplify shift-based multiply-by-constant stuff
- [ ] Support for += etc.
- [ ] Other output options, like assembly offsets
- [ ] Display absolute branch destinations in mips instead of relative
- [ ] Handle functions that return a value more cleverly
- [ ] Make output formatting more extensible, all in its own `languageForm` file

### Ideas that sound cool but may take a while
- [ ] Identify memory areas that are constants and display them in the code
- [ ] Search for modifications to a given memory address
- [ ] Attempt to match unknown addresses with known struct types
- [ ] Smart recursive decompilation, maybe building a call tree
