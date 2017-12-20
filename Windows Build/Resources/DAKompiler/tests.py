from __future__ import print_function
import unittest
import basicTypes
import symbolify
import algebra

Context = symbolify.Context

class TestContexts(unittest.TestCase):

    def test_basicBranching(self):
        base = symbolify.Branch().branchOff(0,True)
        left = Context([base.branchOff(1,True)])
        right = Context([base.branchOff(1,False)])
        self.assertFalse(left.isCompatibleWith(right))
        imp, rel = left.implies(Context([base]))
        self.assertTrue(imp)
        self.assertEqual(len(rel.cnf), 1)
        self.assertEqual(len(rel.cnf[0]), 1)
        self.assertEqual(rel.cnf[0][1], True)

    def test_smallMerge(self):
        zero = symbolify.Branch().branchOff(0,True)
        one = symbolify.Branch().branchOff(0,False).branchOff(1,True)
        two = symbolify.Branch().branchOff(0,False).branchOff(1,False).branchOff(2,True)
        either = Context([zero, one, two])
        self.assertEqual(len(either.cnf), 3)
        self.assertEqual(len(either.cnf[0]), 1)
        self.assertEqual(len(either.cnf[1]), 1)
        self.assertEqual(len(either.cnf[2]), 1)

    def test_completeMerge(self):
        for L in range(3,8):
            base = []
            for i in range(1<<L):
                temp = symbolify.Branch()
                for j in range(L):
                    temp = temp.branchOff(j, i &(1 << j))
                base.append(temp)
            self.assertTrue(Context(base).isTrivial())

    @unittest.skip('takes a while')
    def test_bigMerge(self):
        #currently takes ~2 seconds on my machine
        base = []
        L = 8
        for i in range(1<<L):
            temp = symbolify.Branch()
            for j in range(L):
                temp = temp.branchOff(j, i &(1 << j))
            base.append(temp)
        self.assertTrue(Context(base).isTrivial())

build = algebra.Expression.build
Lit = algebra.Literal
Sym = algebra.Symbol

class TestExpressions(unittest.TestCase):
    def test_adding(self):
        three = build('+', Lit(1), Lit(2))
        self.assertEqual(three.value, 3)
        bar = build('+', Sym('bar'), Lit(0))
        self.assertTrue(isinstance(bar, Sym))
        self.assertEqual('{}'.format(bar), 'bar')
        #constants are grouped and placed at the end
        together = build('+',three, bar)
        self.assertEqual('{}'.format(together),'bar + 3')
        self.assertEqual('{}'.format(build('+',together,together)), 'bar + bar + 6')

    def test_arithMerge(self):
        rep = build('*', Sym('rep'), Lit(2))
        cubed = algebra.Expression.arithmeticMerge('*', [rep,rep,rep])
        self.assertEqual('{}'.format(cubed), 'rep*rep*rep*8')

    def test_hexFormat(self):
        self.assertEqual('{:h}'.format(Lit(255)), '0xff')
        self.assertEqual('{}'.format(build('|', Sym('x'), Lit(21))), 'x | 0x15')

    def test_constantMultiply(self):
        foo = Sym('foo')
        timesFour = build('<<', foo, Lit(2))
        timesFive = build('+', timesFour, foo)
        timesForty = build('<<', timesFive, Lit(3))
        timesThirtyNine = build('-', timesForty, foo)
        self.assertEqual('{}'.format(timesThirtyNine), 'foo*39')

Var = basicTypes.Variable

class TestStructLookup(unittest.TestCase):
    def setUp(self):
        bindings = {'structs':{
                        'testStruct':basicTypes.Struct('testStruct',4+4+4+8+4, None, {
                            0:Var('zero', basicTypes.word),
                            4:Var('sub', 'subStruct'),
                            #8: word (missing)
                            0xc:Var('array', basicTypes.Array(basicTypes.short, 4)),
                            0x14:Var('smallNumber', 'testEnum'),

                        }),
                        'subStruct':basicTypes.Struct('subStruct', 4, None, {
                            0:Var('a', basicTypes.short),
                            2:Var('b', basicTypes.byte),
                            3:Var('c', basicTypes.byte),
                        })
                    },
                    'enums':{
                        'testEnum':basicTypes.Enum('testEnum',basicTypes.word, {
                            0:'zero',
                            1:'one',
                            2:'two'
                        })
                    },
                    'globals':{
                    }
                }
        self.foo = Sym('foo', 'testStruct')
        self.history = symbolify.VariableHistory(bindings)
        self.ptr = Sym('structPointer', basicTypes.Pointer('testStruct', 'bar'))

    def test_sizes(self):
        gs = self.history.getSize
        self.assertEqual(gs(basicTypes.ushort), 2)
        self.assertEqual(gs(basicTypes.Flag(basicTypes.byte,{})), 1)
        self.assertEqual(gs(self.ptr.type), 4)
        self.assertEqual(gs(basicTypes.EnumType('testEnum')), 4)
        self.assertEqual(gs(basicTypes.Array(basicTypes.short, 5)), 10)
        self.assertEqual(gs(basicTypes.Array(self.ptr.type, 3)), 12)
        self.assertEqual(gs('testStruct'), 24)


    def test_member(self):
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.word, self.foo, 0)), 'foo.zero')

    def test_subMember(self):
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.short, self.foo, 4)), 'foo.sub.a')
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.byte, self.foo, 7)), 'foo.sub.c')

    def test_array(self):
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.short, self.foo, 0xc)), 'foo.array[0]')
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.short, self.foo, 0xe)), 'foo.array[1]')

    def test_varArray(self):
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.short, self.foo, 0xc, [build('<<',Sym('bar'), Lit(1))])),
            'foo.array[bar]')
        self.assertEqual('{}'.format(
                            self.history.subLookup(
                                basicTypes.short, self.foo, 0xc, [
                                    build('<<',Sym('bar'), Lit(1)),
                                    build('*',build('+',Sym('extra'), Lit(1)), Lit(2))
                                ])),
                        'foo.array[bar + extra + 1]')

    def test_unknown(self):
        self.assertEqual('{}'.format(self.history.subLookup(basicTypes.short, self.foo, 8)), 'foo.h_0x8')

    def test_tooFar(self):
        self.assertRaises(Exception,
            self.history.lookupAddress, basicTypes.word, build('+', self.ptr, Lit(0x40)))

    def test_pointer(self):
        indirect = self.history.lookupAddress(basicTypes.word, self.ptr)
        self.assertEqual('{}'.format(indirect), 'bar.zero')

    @unittest.skip('still figuring out how this should work')
    def test_flag(self):
        testFlags = basicTypes.Flag(basicTypes.short, {0:'zero', 1:'one', 2:'two'})
        flagVar = Sym('flags', testFlags)
        self.assertEqual('{}'.format(build('&', 'flags', Lit(2))), 'flags[one]')
        self.assertEqual('{}'.format(build('&', 'flags', Lit(5))), 'flags[zero] or flags[two]')

    def test_enums(self):
        e = basicTypes.EnumType('testEnum')
        self.assertEqual('{}'.format(self.history.getEnumValue(e, 0)), 'testEnum.zero')

if __name__ == '__main__':
    unittest.main()