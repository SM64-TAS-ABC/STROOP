def to_bytes(n, length, byteorder='big'):
    h = '%x' % n
    s = ('0'*(len(h) % 2) + h).zfill(length*2).decode('hex')
    return s if byteorder == 'big' else s[::-1]