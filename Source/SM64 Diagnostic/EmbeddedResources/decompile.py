import dakompiler

b = dakompiler.loadBindings('Resources\DAKompiler\sm64 ram map.txt', 'J')
marioRam = dakompiler.RAMSnapshot('Resources\DAKompiler\marioRam',0x80000000)

# Return output to IronPython
dakompiler.decompileFunction(marioRam,b, function_address, args = ['A0 mario *Mario'])