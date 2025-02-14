﻿#Region "Microsoft.VisualBasic::ecdfebe5ed97ac2e69cc4512e9b9f7ec, Data\BinaryData\BinaryData\Stream\BinaryDataFormats.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Enum BinaryDateTimeFormat
    ' 
    '     	CTime, 	NetTicks
    ' 
    '  
    ' 
    ' 
    ' 
    ' Enum BinaryStringFormat
    ' 
    '     	ByteLengthPrefix, 	WordLengthPrefix, DwordLengthPrefix, NoPrefixOrTermination, UInt32LengthPrefix
    '     ZeroTerminated
    ' 
    '  
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' Represents the set of formats of binary date and time encodings.
''' </summary>
Public Enum BinaryDateTimeFormat
	''' <summary>
	''' The <see cref="System.DateTime"/> has the time_t format of the C library.
	''' </summary>
	CTime

	''' <summary>
	''' The <see cref="System.DateTime"/> is stored as the ticks of a .NET <see cref="System.DateTime"/> instance.
	''' </summary>
	NetTicks
End Enum

''' <summary>
''' Represents the set of formats of binary string encodings.
''' </summary>
Public Enum BinaryStringFormat
	''' <summary>
	''' The string has a prefix of 1 byte determining the length of the string and no postfix.
	''' </summary>
	ByteLengthPrefix

	''' <summary>
	''' The string has a prefix of 2 bytes determining the length of the string and no postfix.
	''' </summary>
	WordLengthPrefix

    ''' <summary>
    ''' The string has a prefix of 4 bytes determining the length of the string and no postfix.
    ''' (<see cref="Integer"/>)
    ''' </summary>
    DwordLengthPrefix
    ''' <summary>
    ''' The string has a prefix of 4 bytes determining the length of the string and no postfix.
    ''' (<see cref="UInteger"/>)
    ''' </summary>
    UInt32LengthPrefix

    ''' <summary>
    ''' The string has no prefix and is terminated with a byte of the value 0.
    ''' </summary>
    ZeroTerminated

    ''' <summary>
    ''' The string has neither prefix nor postfix. This format is only valid for writing strings. For reading
    ''' strings, the length has to be specified manually.
    ''' (经常使用这种模式用于写入Magic Header字符串)
    ''' </summary>
    NoPrefixOrTermination
End Enum
