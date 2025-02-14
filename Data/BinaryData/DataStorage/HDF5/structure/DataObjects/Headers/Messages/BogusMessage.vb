﻿#Region "Microsoft.VisualBasic::f93b684e6685f08eaecaec566f62a9ac, Data\BinaryData\DataStorage\HDF5\structure\DataObjects\Headers\Messages\BogusMessage.vb"

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

    '     Class BogusMessage
    ' 
    '         Properties: value
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: printValues
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO

Namespace HDF5.struct.messages

    ''' <summary>
    ''' This message is used for testing the HDF5 Library’s response to an “unknown” message 
    ''' type and should never be encountered in a valid HDF5 file.
    ''' 
    ''' For testing only; should never be stored in a valid file.
    ''' </summary>
    Public Class BogusMessage : Inherits Message

        Public ReadOnly Property value As Integer

        Public Sub New(sb As Superblock, address As Long)
            Call MyBase.New(address)

            value = sb.FileReader(address).readInt
        End Sub

        Protected Friend Overrides Sub printValues(console As TextWriter)
            Call console.WriteLine($"Bogus Value: {value}")
        End Sub
    End Class
End Namespace
