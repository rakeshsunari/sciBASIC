# BinaryStringFormat
_namespace: <a href="#" onClick="load('/docs/Microsoft.VisualBasic.Data.IO/index.md')">Microsoft.VisualBasic.Data.IO</a>_

Represents the set of formats of binary string encodings.




### Properties

#### ByteLengthPrefix
The string has a prefix of 1 byte determining the length of the string and no postfix.
#### DwordLengthPrefix
The string has a prefix of 4 bytes determining the length of the string and no postfix.
#### NoPrefixOrTermination
The string has neither prefix nor postfix. This format is only valid for writing strings. For reading
 strings, the length has to be specified manually.
#### WordLengthPrefix
The string has a prefix of 2 bytes determining the length of the string and no postfix.
#### ZeroTerminated
The string has no prefix and is terminated with a byte of the value 0.
