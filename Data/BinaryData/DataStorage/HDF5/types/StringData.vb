﻿#Region "Microsoft.VisualBasic::b8b7a5d7cdd7444734d36cb556058c2a, Data\BinaryData\DataStorage\HDF5\types\StringData.vb"

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

    '     Class StringData
    ' 
    '         Properties: TypeInfo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace HDF5.type

    Public Class StringData : Inherits DataType

        Public Overrides ReadOnly Property TypeInfo As System.Type
            Get
                Return GetType(String)
            End Get
        End Property
    End Class
End Namespace
