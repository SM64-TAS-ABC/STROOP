from __future__ import print_function
from python27_comp import to_bytes
import basicTypes
import struct

class Literal:
    def __init__(self, x, h = False):
        self.value = x
        self.type = basicTypes.word
        self.isHex = h

    def __or__(self,other):
        return Literal(self.value | other.value, self.isHex or other.isHex)

    def __add__(self,other):
        return Literal(self.value + other.value, self.isHex or other.isHex)

    def __lshift__(self,other):
        return Literal(self.value << other.value, self.isHex)

    def __lt__(self,other):
        return self.value < other

    def __neg__(self):
        return Literal(-self.value, self.isHex)

    def __eq__(self, other):
        try:
            return self.value == other.value
        except:
            return self.value == other


    def __format__(self, spec):
        if self.type == basicTypes.address and self.value == 0:
            return 'None'

        topbyte = self.value >> 24
        if topbyte | 0x80 in range(0xbd,0xc8):
            return '{:.4f}'.format(struct.unpack('>f',to_bytes(self.value,4,byteorder='big'))[0])

        if 'h' in spec or topbyte == 0x80 or self.type == basicTypes.address:
            return hex(self.value)

        # silly heuristic
        h = hex(self.value)
        d = str(self.value)
        return h if (h.count('0')-1)/len(h) > d.count('0')/len(d) else d

    def __repr__(self):
        return 'Literal({:#x})'.format(self.value)

class Symbolic:
    pass

class Symbol(Symbolic):
    def __init__(self, sym, d = basicTypes.unknown):
        self.name = sym
        self.type = d

    def negated(self):
        return Symbol('not {}'.format(self))    # not a good solution

    def toHex(self):
        return self

    def __eq__(self,other):
        return isinstance(other,Symbol) and self.name == other.name

    def __format__(self, spec):
        return self.name

    def __repr__(self):
        return 'Symbol({!r}, {!r})'.format(self.name,self.type)

class Expression(Symbolic):

    logicOpposite = {'==':'!=', '!=':'==', '>':'<=', '<':'>=', '<=':'>', '>=':'<'}

    def __init__(self, op, args, fmt = basicTypes.unknown, constant = None):
        self.op = op
        self.args = args
        self.type = fmt
        self.constant = constant

    specialFormat = None

    def __format__(self, spec):
        if Expression.specialFormat:
            specialResult = Expression.specialFormat(self, spec)
            if specialResult:
                return specialResult

        if self.op == '@':
            fmtString = '{}({:h})' if isinstance(self.args[0], basicTypes.Primitive) else '{}({})'
            return fmtString.format(self.args[0], self.args[1])

        if self.op == '.':
            return '{}.{}'.format(self.args[0], self.args[1])

        if self.op == '[':
            return '{}[{}]'.format(self.args[0], self.constant)

        if self.op == '-r':
            self.op = '-'
            self.args = self.args[::-1]

        if '!' in spec and self.type == basicTypes.boolean:
            sep = ' {} '.format(Expression.logicOpposite[self.op])
        elif self.op in '* / ** .'.split():
            sep = self.op
        else:
            sep = ' {} '.format(self.op)

        if 'h' in spec or self.op in '|&^' or self.type == basicTypes.address:
            inner = '{:ph}'
        else:
            inner = '{:p}'

        try:
            return ('({}{})' if 'p' in spec else '{}{}').format(sep.join(inner.format(a) for a in self.args),
                                                '{{}}{}'.format(inner).format(sep,self.constant) if self.constant else '')
        except:
            print('error formatting', repr(self))
            raise

    def negated(self):
        if self.type == basicTypes.boolean:
            return Expression(Expression.logicOpposite[self.op], self.args, self.type)
        raise Exception("Can't negate non-logical expression")

    def __repr__(self):
        return "Expression({}, {}{})".format(self.op, ', '.join(repr(a) for a in self.args),
                                                    '; {}'.format(self.constant) if self.constant else '')

    opLambdas = {
        '+' : lambda x, y: x + y,
        '*' : lambda x, y: x * y,
        '-' : lambda x, y: x - y,
        '-r' : lambda x, y: y - x,
        '/' : lambda x, y: x / y,
        '>>': lambda x, y: x >> y,
        '<<': lambda x, y: x << y,
        '|' : lambda x, y: x | y,
        '^' : lambda x, y: x ^ y,
        '&' : lambda x, y: x & y,
    }

    opIdentities = {
        '+': 0,
        '*': 1,
        '|': 0
    }

    @classmethod
    def build(cls, op, left, right, flop = False):

        if op == 'NOR':     #why is this a thing
            return cls('~',[cls.build('|', left, right, flop)])

        if op == '[':
            return cls('[', [left], fmt = left.type.pointedType, constant = right)

        if isinstance(left, Literal):
            if isinstance(right, Literal):
                #two literals, completely reducible
                return Literal(cls.opLambdas[op](left.value, right.value))
            #swap move literal to right side
            left, right = right, left
            if op in '< > <= >=':
                op = {'<':'>', '>':'<', '<=':'>=', '>=':'<='}[op]
            elif op == '-':
                # reverse subtraction
                op = '-r'
        #left is not a literal, right may be

        if op == '*' and left == right:
            return cls('**', [left], constant=Literal(2), fmt=basicTypes.single if flop else basicTypes.word)

        if op in ['==', '!='] and isinstance(right, Literal):
            if left.type == basicTypes.boolean and right == 0:
                return left if op == '!=' else left.negated()
            if basicTypes.isIndexable(left.type) and right == 0:
                return left
            if isinstance(left.type, basicTypes.EnumType):
                right.type = left.type

        # simplify multiplications by constants
        if op == '<<' and isinstance(right, Literal) and right.value < 8:   #assume real shifts are by more
            op, right = '*', Literal(2**right.value)
        elif op in '+--r' and isinstance(left, cls) and left.op == '*':
            if isinstance(left, cls) and len(left.args) == 1 and left.args[0] == right:
                return cls('*', [right], left.type, Literal(cls.opLambdas[op](left.constant.value,1)))

        if op in '+*|':
            return cls.arithmeticMerge(op, [left, right], flop)
        else:
            new = cls(op, [left, right])

        if op in '< > <= >= == !='.split():
            new.type = basicTypes.boolean

        if op == '.':
            new.type = right.type

        return new

    @classmethod
    def arithmeticMerge(cls, op, args, flop = False):
        symbols = []
        newConstant = cls.opIdentities[op]
        for a in args:
            if isinstance(a, cls) and a.op == op:
                symbols.extend(a.args)
                if a.constant:
                    newConstant = cls.opLambdas[op](newConstant, a.constant.value)
            elif isinstance(a, Literal):
                newConstant = cls.opLambdas[op](newConstant, a.value)
            else:
                symbols.append(a)
        newConstant = None if newConstant == cls.opIdentities[op] else Literal(newConstant)
        if symbols:     #in case I add symbolic cancellation later
            if len(symbols) == 1 and not newConstant:
                return symbols[0]
            else:
                #multiple expressions summed
                if flop:
                    newType = basicTypes.single
                else:
                    newType = basicTypes.word
                    for s in symbols:
                        if basicTypes.isIndexable(s.type):
                            newType = basicTypes.address
                            break
                return cls(op, symbols, constant=newConstant, fmt=newType)
        elif newConstant:
            return newConstant
        else:
            return Literal(cls.opIdentities[op])
