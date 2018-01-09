from symbolify import InstrResult as IR, Context
import basicTypes
import algebra

indent = ' '*4

def cformat(exp, spec):
    if exp.op == '.' and isinstance(exp.args[0].type, basicTypes.Pointer):
        return '{}->{}'.format(exp.args[0], exp.args[1])

def dummy(*args):
    return str(args)

def renderReg(state):
    return '{0.name} = {0.value};'.format(state) if state.explicit else None

def renderFunc(title, args, val):
    return ('{0} = {1}({2});' if val.type != basicTypes.bad else '{1}({2});').format(val, title,
        ', '.join('{}'.format(v) for r,v in args))

def renderWrite(value, target):
    return '{} = {};'.format(target, value)

def renderReturn(value = None):
    return 'return {};'.format(value) if value else None

renderList = {
    IR.register : renderReg,
    IR.write    : renderWrite,
    IR.function : renderFunc,
    IR.end      : renderReturn,
    IR.unhandled: lambda x:'unhandled opcode: {}'.format(x)
    }

def renderFunctionToC(name, codeTree, history, booleans):
    algebra.Expression.specialFormat = cformat
    text = ['void {}({}){{'.format(name,', '.join(history.states[arg][0].value.name for arg in history.argList))]
    text.extend(renderToC(codeTree,booleans,1))
    text.append('}')
    return text

def renderToC(codeTree, booleans, level = 0):
    text = []
    for line in codeTree.code:
        result = renderList[line[0]](*line[1:])
        if result:
            text.append((indent*level)+result)
    previousWasShown = False
    for block in codeTree.children:
        if block.relative.isTrivial():
            newLevel = level
            prefix = None
        else:
            newLevel = level + 1
            if block.elseRelative and previousWasShown:
                keyword = '{}else {{' if block.elseRelative.isTrivial() else '{}else if ({}) {{'
                toShow = block.elseRelative
            else:
                keyword = '{}if ({}) {{'
                toShow = block.relative

            prefix = keyword.format(indent*level,
                    ' || '.join(
                        ' && '.join(
                            ('{}' if val else '{:!}').format(booleans[ch]) for ch, val in br.items()
                        )
                    for br in toShow.cnf)
                )
        inner = renderToC(block, booleans, newLevel)
        if inner:
            previousWasShown = True
            if prefix:
                text.append(prefix)
            text.extend(inner)
            if prefix:
                text.append('{}}}'.format(indent*level))
        else:
            previousWasShown = False
    return text

