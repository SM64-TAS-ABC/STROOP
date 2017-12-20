from __future__ import print_function
versionCode = '1.0'
import basicTypes
from instruction import Register, FloatRegister
from collections import namedtuple

verboseFileLoading = True

def clean(line):
    indent = 0
    for c in line:
        if c == ' ':
            indent += 1
        elif c == '\t':
            indent += 4
        else:
            break
    indent = indent // 4
    if '#' in line:
        line = line[:line.index('#')]
    return indent, line.split()

class MismatchError(Exception):
    def __init__(self, data, shouldbe, found):
        self.data = data
        self.should = shouldbe
        self.found = found

def verbosePrint(*args, **kwargs):
    if verboseFileLoading:
        print(*args, **kwargs)

def base(tokens, curr, bindings):
    category = tokens[0].lower()
    if category in ['version', 'rom', 'region']:
        if category == 'version':
            if tokens[1] != versionCode:
                print('file is version',tokens[1],', not',versionCode,' may be incompatible')
        elif category == 'rom':
            name = ' '.join(tokens[1:])
            if 'rom' in bindings:
                if name.lower() != bindings['rom'].lower():
                    raise MismatchError('rom', bindings['rom'], name)
            else:
                bindings['rom'] = name
    else:
        parserList = {
            'structs'   : structs,
            'functions' : functions,
            'globals'   : globalVars,
            'enums'     : enums,
            'trigtables': trig
            }
        try:
            new = bindings[category]
            if len(tokens) >= 2:    #check for a region specifier
                if tokens[1].upper() != bindings['region']:
                    return None, ignoreData

            return new, parserList[category]
        except:
            verbosePrint('unknown binding type',tokens[0])
        
def badData(tokens, curr, bindings):
    verbosePrint('ignoring',' '.join(tokens), ', could not interpret')

def ignoreData(tokens, curr, bindings):
    verbosePrint('ignoring', ' '.join(tokens), ', wrong region/not implemented')

def trig(tokens, curr, bindings):
    curr[int(tokens[0],16)] = tokens[1]

def simpleType(bindings, typeString):
    try:
        return basicTypes.Primitive.lookup[typeString], None
    except:
        try:
            flagType = {'bflag':basicTypes.ubyte, 'hflag':basicTypes.ushort, 'wflag':basicTypes.word}
            newFlag = basicTypes.Flag(flagType[typeString.lower()], {})
            return newFlag, (newFlag, flagBit)
        except:
            canon = typeString.lower().split()
            if canon[0] == 'enum':
                if canon[1] not in bindings['enums']:
                    bindings['undefined'].add(canon[1])
                return basicTypes.EnumType(canon[1]), None           
            if canon[0] not in bindings['structs']:
                bindings['undefined'].add(canon[0])
            return canon[0], None

def flagBit(tokens, curr, bindings):
    curr.bits[int(tokens[0],  16)] = tokens[1]

def makeType(bindings, typeString):
    typeString = typeString.strip()
    if typeString[0] == '(':
        return makeType(typeString[1:typeString.rindex(')')])
    if typeString[0] == '*':
        if ' ' in typeString and typeString[-1] not in ')]':
            target = typeString[typeString.rindex(' ')+1:]
            base, _ = makeType(bindings, typeString[1:typeString.rindex(' ')])
        else:
            target = None
            base, _ = makeType(bindings, typeString[1:])
        return basicTypes.Pointer(base, target), None
    if typeString[0] == '[':
        inner, _ = makeType(bindings, typeString[1:typeString.rindex(']')])
        if typeString[-1] == ']':
            # unknown length
            return basicTypes.Array(inner, -1), None
        else:
            return basicTypes.Array(inner, int(typeString[typeString.rindex(']')+1:],16)), None
    return simpleType(bindings, typeString)

def globalVars(tokens, curr, bindings):
    newType, data = makeType(bindings, ' '.join(tokens[2:]))
    curr[int(tokens[0], 16)] = basicTypes.Variable(tokens[1], newType)
    return data

def enums(tokens, curr, bindings):
    canon = tokens[0].lower()
    if canon not in curr:
        curr[canon] = basicTypes.Enum(tokens[0], simpleType(bindings, tokens[1])[0], {})
        bindings['undefined'].discard(canon)
    return curr[canon], enumValue

def enumValue(tokens, curr, bindings):
    value = int(tokens[0],  16)
    if value in curr.values:
        verbosePrint('%s.%s is already bound to %s' % (curr.name, curr.values[value], tokens[0]))
    else:
        curr.values[value] = tokens[1]

def functions(tokens, curr, bindings):
    curr[int(tokens[0],16)] = basicTypes.FunctionSignature(tokens[1], [])
    return curr[int(tokens[0],16)].args, arguments

def arguments(tokens, curr, bindings):
    #TODO stack registers
    try:
        reg = Register[tokens[0].upper()]
    except:
        try:
            reg = FloatRegister[tokens[0].upper()]
        except:
            if tokens[0][:2].upper() == 'SP':
                reg = basicTypes.Stack(int(tokens[0][2:], 16))
            else:
                verbosePrint("couldn't understand argument", *tokens)
                return None
    if len(tokens) >= 3:
        name = tokens[1]
        t, _ = makeType(bindings, ' '.join(tokens[2:]))
    else:
        name = None
        t, _ = makeType(bindings, ' '.join(tokens[1:]))
    
    curr.append((reg, name, t))

def structs(tokens, curr, bindings):
    canon = tokens[0].lower()
    if canon not in curr:
        parentStruct = tokens[2].lower() if len(tokens) == 3 else None
        curr[canon] = basicTypes.Struct(tokens[0], int(tokens[1],16), parentStruct, {})
        bindings['undefined'].discard(canon)
        if parentStruct:
            curr[canon].members.update(curr[parentStruct].members)
    return curr[canon], members

def members(tokens, curr, bindings):
    newType, data = makeType(bindings, ' '.join(tokens[2:]))
    curr.members[int(tokens[0], 16)] = basicTypes.Variable(tokens[1], newType)
    return data

def loadBindings(filename, region = None, bindings = None, verbose = True):
    global verboseFileLoading
    verboseFileLoading = verbose
    if not bindings:
        bindings = {'functions':{}, 'globals':{}, 'structs':{}, 'enums':{}, 'trigtables':{}, 'undefined':set()}
    if 'region' in bindings:
        if region and bindings['region'] != region:
            raise MismatchError('region', bindings['region'], region)
    elif region in ['U','J']:
        bindings['region'] = region
    else:
        raise Error('must specify a valid region: U or J')


    index = 0
    # 5 ought to be enough for anyone
    levelStack = [0, -1, -1, -1, -1]
    parsers = [base, None, None, None, None]
    entities = [None, None, None, None, None]

    with open(filename, 'r') as f:
        for line in f:
            indent, tokens = clean(line)
            if not tokens:
                continue

            while indent > levelStack[index]:
                if levelStack[index] == -1:
                    levelStack[index] = indent
                    break
                index += 1
                if index >= 4:
                    verbosePrint('line',' '.join(tokens),'is indented too far')
                    index = 3
                    break
            while indent < levelStack[index]:
                #indents in between two layers round down/outward/to the left
                levelStack[index] = -1
                index -= 1

            try:
                inner = parsers[index](tokens, entities[index], bindings)
                if inner:
                    entities[index + 1], parsers[index + 1] = inner
                else:
                    entities[index + 1], parsers[index + 1] = None, badData
            except MismatchError as m:
                print('Mismatch between file and expected',m.data,': expected',m.should,'but found', m.found)
                print('aborting load')
                return None
            except:
                verbosePrint('error parsing',' '.join(tokens))
                raise

    if bindings['undefined']:
        verbosePrint('the following types are still undefined:')
        for t in bindings['undefined']:
            verbosePrint('\t'+t)
    return bindings



