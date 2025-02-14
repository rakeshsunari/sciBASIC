﻿#Region "Microsoft.VisualBasic::24b13cc75929d945706caddec5b92b1f, Microsoft.VisualBasic.Core\Language\Value\CharStream.vb"

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

    '     Class CharStream
    ' 
    '         Function: GetEnumerator, IEnumerable_GetEnumerator
    ' 
    '         Sub: Add
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace Language

    Public Class CharStream : Implements IEnumerable(Of Char)

        Dim chars As New List(Of Char)

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Add(c As Char)
            chars.Add(c)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(chars As CharStream) As SByte()
            Return chars.Select(Function(c) CSByte(AscW(c))).ToArray
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(chars As CharStream) As Byte()
            Return chars.Select(Function(c) CByte(Asc(c))).ToArray
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(chars As CharStream) As String
            Return New String(chars.ToArray)
        End Operator

        Public Iterator Function GetEnumerator() As IEnumerator(Of Char) Implements IEnumerable(Of Char).GetEnumerator
            For Each c As Char In chars
                Yield c
            Next
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Class
End Namespace
