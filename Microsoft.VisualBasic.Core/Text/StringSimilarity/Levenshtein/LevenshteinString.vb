﻿#Region "Microsoft.VisualBasic::414e29f5596cf5bd5a370c34dea2a9f8, Microsoft.VisualBasic.Core\Text\StringSimilarity\Levenshtein\LevenshteinString.vb"

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

    '     Structure LevenshteinString
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    '         Operators: (+6 Overloads) Like
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace Text.Levenshtein

    ''' <summary>
    ''' The string like helper
    ''' </summary>
    Public Structure LevenshteinString

        Private _string$
        Private _chars%()

        Sub New(s$)
            _string = s
            _chars = s _
                .Select(AddressOf AscW) _
                .ToArray
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return _string
        End Function

        ''' <summary>
        ''' String similarity compares
        ''' </summary>
        ''' <param name="s$"></param>
        ''' <param name="subject"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator Like(s$, subject As LevenshteinString) As DistResult
            Return Levenshtein.ComputeDistance(
                s.CharCodes,
                subject._chars,
                Function(a, b) a = b,
                AddressOf ChrW)
        End Operator

        ''' <summary>
        ''' String similarity compares
        ''' </summary>
        ''' <param name="query"></param>
        ''' <param name="s$"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator Like(query As LevenshteinString, s$) As DistResult
            Return Levenshtein.ComputeDistance(
                query._chars,
                s.CharCodes,
                Function(a, b) a = b,
                AddressOf ChrW)
        End Operator

        ''' <summary>
        ''' String similarity compares
        ''' </summary>
        ''' <param name="query"></param>
        ''' <param name="subject"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator Like(query As LevenshteinString, subject As LevenshteinString) As DistResult
            Return Levenshtein.ComputeDistance(
                query._chars,
                subject._chars,
                Function(a, b) a = b,
                AddressOf ChrW)
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(s As LevenshteinString) As String
            Return s._string
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(s$) As LevenshteinString
            Return New LevenshteinString With {
                ._string = s,
                ._chars = s.Select(AddressOf AscW).ToArray
            }
        End Operator
    End Structure
End Namespace
